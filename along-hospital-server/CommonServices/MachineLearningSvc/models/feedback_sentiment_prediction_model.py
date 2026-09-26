import joblib
import os
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.model_selection import train_test_split
from sklearn.naive_bayes import MultinomialNB 
from datasets import get_cleaned_medicine_feedback_dataset
from imblearn.over_sampling import SMOTE
from utils.dataset_util import preprocess_text
from entities.feedback import FeedbackTypeEnum

script_dir = os.path.dirname(__file__)
model_path = os.path.join(script_dir, "../model_artifacts/feedback_sentiment_model.joblib")


class FeedbackSentimentPredictionModel:
    def __init__(self):
        self.vectorizer = None
        self.model = None

    def train(self):
        df = get_cleaned_medicine_feedback_dataset()
        self.vectorizer = TfidfVectorizer(
            ngram_range=(1, 2),
            max_df=0.9,
            min_df=3,
        )

        X = self.vectorizer.fit_transform(df["comment"])
        y = df["is_positive"]

        smote = SMOTE(sampling_strategy="minority")
        X, y = smote.fit_resample(X, y)

        X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

        model = MultinomialNB()
        model.fit(X_train, y_train)

        self.model = model
        self.save_model()

        return { "vectorizer": self.vectorizer, "model": self.model, "best_score": model.score(X_test, y_test) }

    def load_model(self):
        if os.path.exists(model_path):
            data = joblib.load(model_path)
            self.vectorizer = data["vectorizer"]
            self.model = data["model"]

    def save_model(self):
        if not os.path.exists(os.path.dirname(model_path)):
            os.makedirs(os.path.dirname(model_path))

        if self.model is not None:
            joblib.dump(
                {"vectorizer": self.vectorizer, "model": self.model}, model_path
            )

    def predict(self, text):
        if self.model is None:
            self.load_model()

        if self.model is None:
            self.train()

        text = preprocess_text(text)
        X = self.vectorizer.transform([text])

        proba = self.model.predict_proba(X)[0]

        neg_score = float(proba[0])
        pos_score = float(proba[1])

        NEUTRAL_CONFIDENCE_THRESHOLD = 0.2

        diff = abs(pos_score - neg_score)

        sentiment = None
        if diff < NEUTRAL_CONFIDENCE_THRESHOLD:
            sentiment = FeedbackTypeEnum.Neutral.name
        elif pos_score > neg_score:
            sentiment = FeedbackTypeEnum.Positive.name
        else:
            sentiment = FeedbackTypeEnum.Negative.name

        return sentiment, proba