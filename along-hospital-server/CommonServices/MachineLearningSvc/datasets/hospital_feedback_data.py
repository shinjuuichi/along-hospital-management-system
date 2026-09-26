import glob
import pandas as pd
import os
from utils.dataset_util import preprocess_text
from entities import Complaint
from entities.complaint import ComplaintTypeEnum

script_dir = os.path.dirname(os.path.abspath(__file__))


def _read_all_hospital_feedback_dataset():
    cleaned_dataset_path = os.path.normpath(
        os.path.join(script_dir, "./processed/cleaned_hospital_feedback_data.csv")
    )
    if os.path.exists(cleaned_dataset_path):
        return pd.read_csv(cleaned_dataset_path)

    pattern = os.path.normpath(
        os.path.join(script_dir, "./raw/hospital_feedback_data_*.csv")
    )

    dfs = []
    for file_path in glob.glob(pattern):
        df = pd.read_csv(file_path)
        df = df.rename(
            columns={
                "feedback": "complaint",
                "comments": "complaint",
                "is_positive": "is_positive",
                "recommend_facility": "is_positive",
            }
        )
        dfs.append(df[["complaint", "is_positive"]])

    merged = pd.concat(dfs, ignore_index=True)
    merged["complaint"] = merged["complaint"].apply(preprocess_text)

    merged.to_csv(cleaned_dataset_path, index=False)

    return merged


def _load_hospital_feedback_from_database():
    complaints: list[Complaint] = Complaint.query.all()
    data = [
        {
            "complaint": c.content,
            "is_positive": c.complaint_type == ComplaintTypeEnum.Positive,
        }
        for c in complaints
        if c.complaint_type != ComplaintTypeEnum.Neutral
    ]
    df = pd.DataFrame(data)
    df["complaint"] = df["complaint"].apply(preprocess_text)
    return df


def get_cleaned_hospital_feedback_dataset():
    df1 = _load_hospital_feedback_from_database()
    df2 = _read_all_hospital_feedback_dataset()

    merged = pd.concat([df1, df2], ignore_index=True)
    merged = merged[merged["complaint"].str.strip() != ""]

    return merged
