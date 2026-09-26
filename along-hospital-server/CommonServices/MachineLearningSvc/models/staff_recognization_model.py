import os
import pickle
import uuid
import time
import logging
import shutil
import numpy as np
import faiss
import cv2
import math
from threading import Lock
from concurrent.futures import ThreadPoolExecutor
from deepface import DeepFace
from utils.image_util import detect_and_crop_faces, augment_images
from sklearn.model_selection import train_test_split
from collections import Counter

script_dir = os.path.dirname(__file__)


class StaffRecognizationModel:
    def __init__(self):
        self.model_name = "Facenet512"
        self.detector_backend = "opencv"
        self.max_images = 150

        self.index_path = os.path.join(
            script_dir, "../model_artifacts/staff_recognization.index"
        )
        self.meta_path = os.path.join(
            script_dir, "../model_artifacts/staff_recognization_meta.pkl"
        )
        self.dataset_path = os.path.join(script_dir, "../images/staff")

        self.index = None
        self.metadata = []
        self._index_lock = Lock()
        self._pending_staff_ids = set()
        self._pending_lock = Lock()
        self._executor = ThreadPoolExecutor(max_workers=1, thread_name_prefix="staff-enroll")

        if os.path.exists(self.index_path) and os.path.exists(self.meta_path):
            self._load_index()

    def _get_embedding_from_path(self, img_path) -> np.ndarray:
        rep = DeepFace.represent(
            img_path=img_path,
            model_name=self.model_name,
            detector_backend=self.detector_backend,
            enforce_detection=False,
        )
        embedding = np.array(rep[0]["embedding"], dtype="float32")
        return embedding

    def _save_index(self):
        if self.index is None or not self.metadata:
            raise Exception("Index or metadata is empty. Cannot save.")

        os.makedirs(os.path.dirname(self.index_path), exist_ok=True)
        faiss.write_index(self.index, self.index_path)
        with open(self.meta_path, "wb") as f:
            pickle.dump(self.metadata, f)

    def _load_index(self):
        self.index = faiss.read_index(self.index_path)
        with open(self.meta_path, "rb") as f:
            self.metadata = pickle.load(f)

    def _ensure_index_loaded(self):
        if self.index is None or not self.metadata:
            if os.path.exists(self.index_path) and os.path.exists(self.meta_path):
                self._load_index()
            else:
                raise Exception(
                    "FAISS index is not loaded. Build index from dataset first."
                )

    def build_index_from_dataset(self):
        if not os.path.isdir(self.dataset_path):
            raise Exception(f"Dataset path does not exist: {self.dataset_path}")

        embeddings = []
        metadata = []

        for root, _, files in os.walk(self.dataset_path):
            staff_id = os.path.basename(root)
            if not staff_id or staff_id == os.path.basename(self.dataset_path):
                continue

            for f in files:
                if f.lower().endswith((".png", ".jpg", ".jpeg")):
                    img_path = os.path.join(root, f)
                    try:
                        embedding = self._get_embedding_from_path(img_path)
                        embeddings.append(embedding)
                        metadata.append({"staff_id": staff_id, "path": img_path})
                    except Exception as e:
                        continue

        if not embeddings:
            raise Exception("No embeddings created from dataset folder")

        embeddings = np.vstack(embeddings)
        d = embeddings.shape[1]

        index = faiss.IndexFlatL2(d)
        index.add(embeddings)

        self.index = index
        self.metadata = metadata
        self._save_index()

    def search_from_path(self, file_path):
        self._ensure_index_loaded()

        query_embedding = self._get_embedding_from_path(file_path).reshape(1, -1)

        distances, indices = self.index.search(query_embedding, k=5)

        results = []
        for i, idx in enumerate(indices[0]):
            results.append(
                {
                    "staff_id": self.metadata[idx]["staff_id"],
                    "path": self.metadata[idx]["path"],
                    "distance": float(distances[0][i]),
                }
            )

        return results

    def enqueue_staff_images(self, staff_id: str, file_bytes_list: list[bytes]) -> dict:
        staff_id_str = str(staff_id)

        with self._pending_lock:
            if staff_id_str in self._pending_staff_ids:
                return {"acceptedFaces": 0, "isAlreadyProcessing": True}

            self._pending_staff_ids.add(staff_id_str)

        try:
            valid_faces = self._extract_valid_faces(file_bytes_list)
        except Exception:
            with self._pending_lock:
                self._pending_staff_ids.discard(staff_id_str)
            raise

        self._executor.submit(self._run_background_enroll, staff_id_str, valid_faces)
        return {"acceptedFaces": len(valid_faces), "isAlreadyProcessing": False}

    def _extract_valid_faces(self, file_bytes_list: list[bytes]) -> list[np.ndarray]:
        valid_faces = []

        for file_bytes in file_bytes_list:
            img_array = np.frombuffer(file_bytes, np.uint8)
            img = cv2.imdecode(img_array, cv2.IMREAD_COLOR)

            if img is None:
                continue

            try:
                face = detect_and_crop_faces(img)
                valid_faces.append(face)
            except Exception:
                continue

        if not valid_faces:
            raise Exception("No valid face image found for this staff")

        return valid_faces

    def _run_background_enroll(self, staff_id: str, valid_faces: list[np.ndarray]):
        try:
            self._append_faces_to_index(staff_id=staff_id, valid_faces=valid_faces)
        except Exception as e:
            logging.exception("Background enroll failed for staff_id=%s: %s", staff_id, e)
        finally:
            with self._pending_lock:
                self._pending_staff_ids.discard(str(staff_id))

    def _append_faces_to_index(self, staff_id: str, valid_faces: list[np.ndarray]):
        staff_folder = os.path.join(self.dataset_path, str(staff_id))
        os.makedirs(staff_folder, exist_ok=True)

        augmented_faces = []
        per_face = max(1, math.ceil(self.max_images / len(valid_faces)))

        for face in valid_faces:
            augmented_faces.extend(augment_images(face, per_face))

        augmented_faces = augmented_faces[: self.max_images]

        new_embeddings = []
        new_metadata = []

        for idx, face in enumerate(augmented_faces):
            unique_id = uuid.uuid4().hex
            timestamp = int(time.time() * 1000)
            file_name = f"{staff_id}_{timestamp}_{unique_id}_{idx}.jpg"
            file_path = os.path.join(staff_folder, file_name)

            cv2.imwrite(file_path, face)

            try:
                embedding = self._get_embedding_from_path(file_path)
                new_embeddings.append(embedding)
                new_metadata.append({"staff_id": str(staff_id), "path": file_path})
            except Exception as e:
                continue

        if not new_embeddings:
            return

        embeddings_np = np.vstack(new_embeddings)

        with self._index_lock:
            if self.index is None or not self.metadata:
                d = embeddings_np.shape[1]
                index = faiss.IndexFlatL2(d)
                index.add(embeddings_np)

                self.index = index
                self.metadata = new_metadata
            else:
                self.index.add(embeddings_np)
                self.metadata.extend(new_metadata)

            self._save_index()

    def _rebuild_index_from_metadata(self, metadata: list[dict]):
        if not metadata:
            self.index = None
            self.metadata = []

            if os.path.exists(self.index_path):
                os.remove(self.index_path)
            if os.path.exists(self.meta_path):
                os.remove(self.meta_path)
            return

        embeddings = []
        cleaned_metadata = []

        for item in metadata:
            img_path = item.get("path")
            if not img_path or not os.path.isfile(img_path):
                continue

            try:
                embedding = self._get_embedding_from_path(img_path)
                embeddings.append(embedding)
                cleaned_metadata.append(item)
            except Exception:
                continue

        if not embeddings:
            self.index = None
            self.metadata = []

            if os.path.exists(self.index_path):
                os.remove(self.index_path)
            if os.path.exists(self.meta_path):
                os.remove(self.meta_path)
            return

        embeddings_np = np.vstack(embeddings)
        d = embeddings_np.shape[1]
        index = faiss.IndexFlatL2(d)
        index.add(embeddings_np)

        self.index = index
        self.metadata = cleaned_metadata
        self._save_index()

    def reset_staff_identification(self, staff_id: str) -> dict:
        staff_id_str = str(staff_id)

        with self._pending_lock:
            if staff_id_str in self._pending_staff_ids:
                raise Exception(
                    "Enrollment is processing for this staff. Please wait 5-10 minutes before reset."
                )

        removed_count = 0

        with self._index_lock:
            if self.metadata:
                current_metadata = list(self.metadata)
                remaining_metadata = [
                    item
                    for item in current_metadata
                    if str(item.get("staff_id")) != staff_id_str
                ]
                removed_count = len(current_metadata) - len(remaining_metadata)

                if removed_count > 0:
                    self._rebuild_index_from_metadata(remaining_metadata)

        staff_folder = os.path.join(self.dataset_path, staff_id_str)
        if os.path.isdir(staff_folder):
            shutil.rmtree(staff_folder, ignore_errors=True)

        return {"removedCount": removed_count}

    def recognize_staff_from_bytes(self, file_bytes, threshold=350.0):
        img_array = np.frombuffer(file_bytes, np.uint8)
        img = cv2.imdecode(img_array, cv2.IMREAD_COLOR)

        if img is None:
            raise Exception("Invalid image")

        face = detect_and_crop_faces(img)

        results = self.search_from_path(face)
        if not results:
            return {"staffId": None, "distance": None}

        best = results[0]
        if best["distance"] > threshold:
            return {"staffId": None, "distance": best["distance"]}

        return {"staffId": best["staff_id"], "distance": best["distance"]}

    def has_images_for_staff(self, staff_id):
        staff_id_str = str(staff_id)

        with self._pending_lock:
            if staff_id_str in self._pending_staff_ids:
                return True

        staff_folder = os.path.join(self.dataset_path, str(staff_id))
        if not os.path.isdir(staff_folder):
            return False

        for f in os.listdir(staff_folder):
            if f.lower().endswith((".png", ".jpg", ".jpeg")):
                return True
        return False

    def evaluate_model(self, test_size=0.2, k=1, threshold=350.0):
        embeddings = []
        labels = []

        for root, _, files in os.walk(self.dataset_path):
            staff_id = os.path.basename(root)
            if not staff_id or staff_id == os.path.basename(self.dataset_path):
                continue

            for f in files:
                if f.lower().endswith((".png", ".jpg", ".jpeg")):
                    img_path = os.path.join(root, f)
                    try:
                        emb = self._get_embedding_from_path(img_path)
                        embeddings.append(emb)
                        labels.append(staff_id)
                    except:
                        continue

        if not embeddings:
            return {"accuracy": 0, "total_samples": 0, "correct": 0}

        embeddings = np.vstack(embeddings)

        X_train, X_test, y_train, y_test = train_test_split(
            embeddings, labels, test_size=test_size, stratify=labels, random_state=42
        )

        virtual_index = faiss.IndexFlatL2(X_train.shape[1])
        virtual_index.add(X_train)

        correct = 0
        total = len(X_test)

        for emb, true_label in zip(X_test, y_test):
            distances, indices = virtual_index.search(emb.reshape(1, -1), k=k)

            preds = []
            for i, idx in enumerate(indices[0]):
                if distances[0][i] <= threshold:
                    preds.append(y_train[idx])

            if preds:
                most_common = Counter(preds).most_common(1)[0][0]
                if most_common == true_label:
                    correct += 1

        accuracy = correct / total if total > 0 else 0.0

        return {"accuracy": accuracy, "total_samples": total, "correct": correct}