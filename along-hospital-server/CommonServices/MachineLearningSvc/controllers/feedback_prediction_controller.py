from . import bp_feedback
from flask import jsonify, request
from models import FeedbackSentimentPredictionModel, FeedbackToxicPredictionModel

feedback_toxic_model = FeedbackToxicPredictionModel()
feedback_sentiment_prediction_model = FeedbackSentimentPredictionModel()

@bp_feedback.route("/predict-sentiment", methods=["POST"])
def predict_feedback_sentiment():
    data = request.get_json()
    if not data or "feedback" not in data:
        raise Exception("Missing required field: 'feedback'")

    feedback_text = data["feedback"]
    sentiment, proba = feedback_sentiment_prediction_model.predict(feedback_text)
    return jsonify({"prediction": sentiment, "probability": str(proba.tolist())}), 200


@bp_feedback.route("/predict-toxic", methods=["POST"])
def predict_feedback_toxicity():
    data = request.get_json()
    if not data or "feedback" not in data:
        raise Exception("Missing required field: 'feedback'")

    feedback_text = data["feedback"]
    is_toxic = feedback_toxic_model.predict(feedback_text)
    return jsonify({"prediction": is_toxic}), 200


@bp_feedback.route("/retrain-sentiment", methods=["POST"])
def retrain_sentiment_model():
    trained_model = feedback_sentiment_prediction_model.train()

    json_model = {
        "vectorizer": str(trained_model.get("vectorizer")),
        "model": str(trained_model.get("model")),
        "best_score": trained_model.get("best_score"),
    }
        
    return jsonify({"status": "Feedback Sentiment model retrained successfully", "model": json_model}), 200


@bp_feedback.route("/retrain-toxic", methods=["POST"])
def retrain_toxic_model():
    trained_model = feedback_toxic_model.train()
    json_model = {
        "vectorizer": str(trained_model.get("vectorizer")),
        "model": str(trained_model.get("model")),
        "best_score": trained_model.get("best_score"),
    }
        
    return jsonify({"status": "Toxicity model retrained successfully", "model": json_model}), 200