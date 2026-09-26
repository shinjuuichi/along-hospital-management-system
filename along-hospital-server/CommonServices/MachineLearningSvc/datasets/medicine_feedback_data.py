import os
import pandas as pd
from entities import Feedback
from entities.feedback import FeedbackTypeEnum
from utils.dataset_util import preprocess_text

script_dir = os.path.dirname(os.path.abspath(__file__))


def _load_medicine_feedback_dataset():
    cleaned_dataset_path = os.path.normpath(
        os.path.join(script_dir, "./processed/cleaned_medicine_feedback_data.csv")
    )
    if os.path.exists(cleaned_dataset_path):
        return pd.read_csv(cleaned_dataset_path)

    dataset_path = os.path.normpath(
        os.path.join(script_dir, "./raw/medicine_feedback_data.csv")
    )

    df = pd.read_csv(dataset_path)

    df = df[df["rating"].notnull()].copy()
    df["sentiment"] = df["rating"].apply(
        lambda x: 1 if x >= 8 else (-1 if x <= 4 else 0)
    )
    df = df[df["sentiment"] != 0].copy()
    df["is_positive"] = (df["sentiment"] == 1).astype(int)

    df = df.rename(columns={"text": "comment"})
    df = df[["comment", "is_positive"]]

    df["comment"] = df["comment"].apply(preprocess_text)
    df.to_csv(cleaned_dataset_path, index=False)

    return df


def _load_medicine_feedback_from_database():
    feedbacks: list[Feedback] = Feedback.query.all()
    data = [
        {"comment": f.content, "is_positive": 1 if f.rating >= 4 else 0}
        for f in feedbacks
        if f.rating >= 4 or f.rating <= 2
    ]
    df = pd.DataFrame(data)
    df["comment"] = df["comment"].apply(preprocess_text)
    return df


def get_cleaned_medicine_feedback_dataset():
    df1 = _load_medicine_feedback_from_database()
    df2 = _load_medicine_feedback_dataset()

    merged = pd.concat([df1, df2], ignore_index=True)
    merged = merged.dropna(subset=["comment"])
    merged = merged[merged["comment"].str.strip() != ""]

    return merged