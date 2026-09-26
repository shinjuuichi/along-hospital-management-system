import os
import pandas as pd
from entities import Feedback
from utils.dataset_util import preprocess_text

script_dir = os.path.dirname(os.path.abspath(__file__))


def _load_toxic_comment_dataset():
    cleaned_dataset_path = os.path.normpath(
        os.path.join(script_dir, "./processed/cleaned_toxic_comment_data.csv")
    )
    if os.path.exists(cleaned_dataset_path):
        return pd.read_csv(cleaned_dataset_path)

    dataset_path = os.path.normpath(
        os.path.join(script_dir, "./raw/toxic_comment_data.csv")
    )

    df = pd.read_csv(dataset_path)

    weights = {
        "toxic": 1.0,
        "severe_toxic": 2.0,
        "obscene": 1.5,
        "threat": 3.0,
        "insult": 2.0,
        "identity_hate": 3.0,
    }
    df["toxicity_score"] = df[list(weights.keys())].fillna(0).dot(pd.Series(weights))
    df["is_toxic"] = df["toxicity_score"] >= 1.0
    df = df[["comment_text", "is_toxic"]].rename(columns={"comment_text": "comment"})

    df["comment"] = df["comment"].apply(preprocess_text)
    df.to_csv(cleaned_dataset_path, index=False)

    return df


def _load_toxic_comment_from_database():
    feedbacks = Feedback.query.all()
    data = [
        {
            "comment": f.content,
            "is_toxic": f.is_ban,
        }
        for f in feedbacks
    ]
    df = pd.DataFrame(data)
    df["comment"] = df["comment"].apply(preprocess_text)
    return df


def get_cleaned_toxic_comment_dataset():
    df1 = _load_toxic_comment_from_database()
    df2 = _load_toxic_comment_dataset()

    merged = pd.concat([df1, df2], ignore_index=True)
    merged = merged.dropna(subset=["comment"])
    merged = merged[merged["comment"].str.strip() != ""]

    return merged