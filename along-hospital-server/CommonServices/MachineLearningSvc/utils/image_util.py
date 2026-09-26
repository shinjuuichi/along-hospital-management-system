import cv2
import os
import numpy as np
import mediapipe as mp
from mediapipe.tasks import python as mp_python
from mediapipe.tasks.python import vision

script_dir = os.path.dirname(os.path.abspath(__file__))
default_model_path = os.path.join(script_dir, "blaze_face_short_range.tflite")


_FACE_DETECTOR = None


def _get_face_detector() -> vision.FaceDetector:
    global _FACE_DETECTOR

    if _FACE_DETECTOR is None:
        if not os.path.isfile(default_model_path):
            raise Exception(f"Face detector model not found: {default_model_path}")

        base_options = mp_python.BaseOptions(model_asset_path=default_model_path)
        options = vision.FaceDetectorOptions(base_options=base_options)
        _FACE_DETECTOR = vision.FaceDetector.create_from_options(options)

    return _FACE_DETECTOR


def detect_and_crop_faces(img: np.ndarray) -> np.ndarray:
    detector = _get_face_detector()

    h, w = img.shape[:2]
    rgb = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)

    mp_image = mp.Image(image_format=mp.ImageFormat.SRGB, data=rgb)
    results = detector.detect(mp_image)

    if not results.detections:
        raise Exception("No face detected")

    def to_pixel_bbox(detection) -> tuple[int, int, int, int]:
        bbox = detection.bounding_box
        x1 = max(int(bbox.origin_x), 0)
        y1 = max(int(bbox.origin_y), 0)
        x2 = min(x1 + int(bbox.width), w)
        y2 = min(y1 + int(bbox.height), h)
        return x1, y1, x2, y2

    x1, y1, x2, y2 = max(
        (to_pixel_bbox(det) for det in results.detections),
        key=lambda b: max((b[2] - b[0]), 0) * max((b[3] - b[1]), 0),
    )

    if x2 <= x1 or y2 <= y1:
        raise Exception("No face detected")

    face_color = img[y1:y2, x1:x2]
    face_color = cv2.resize(face_color, (160, 160))

    return face_color


def augment_images(img: np.ndarray, target_count: int) -> list[np.ndarray]:
    h, w = img.shape[:2]
    augmented = [img]

    while len(augmented) < target_count:
        aug = img.copy()

        if np.random.rand() < 0.5:
            aug = cv2.flip(aug, 1)

        if np.random.rand() < 0.5:
            crop_ratio = np.random.uniform(0.85, 0.95)
            ch, cw = int(h * crop_ratio), int(w * crop_ratio)
            y = np.random.randint(0, h - ch + 1)
            x = np.random.randint(0, w - cw + 1)
            aug = aug[y : y + ch, x : x + cw]
            aug = cv2.resize(aug, (w, h))

        if np.random.rand() < 0.5:
            scale = np.random.uniform(0.9, 1.1)
            nh, nw = int(h * scale), int(w * scale)
            resized = cv2.resize(aug, (nw, nh))

            if scale > 1:
                y = (nh - h) // 2
                x = (nw - w) // 2
                aug = resized[y : y + h, x : x + w]
            else:
                pad_y = (h - nh) // 2
                pad_x = (w - nw) // 2
                aug = cv2.copyMakeBorder(
                    resized,
                    pad_y,
                    h - nh - pad_y,
                    pad_x,
                    w - nw - pad_x,
                    cv2.BORDER_REFLECT_101,
                )

        if np.random.rand() < 0.5:
            alpha = np.random.uniform(0.9, 1.1)
            beta = np.random.randint(-15, 15)
            aug = cv2.convertScaleAbs(aug, alpha=alpha, beta=beta)

        if np.random.rand() < 0.3:
            shift = np.random.randint(-10, 10, size=3)
            aug = np.clip(aug.astype(np.int16) + shift, 0, 255).astype(np.uint8)

        if np.random.rand() < 0.3:
            noise = np.random.normal(0, 5, aug.shape).astype(np.int16)
            aug = np.clip(aug.astype(np.int16) + noise, 0, 255).astype(np.uint8)

        augmented.append(aug)

    return augmented[:target_count]
