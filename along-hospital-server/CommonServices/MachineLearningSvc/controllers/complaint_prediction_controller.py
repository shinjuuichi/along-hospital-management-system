from . import bp_complaint
from flask import jsonify, request
from models.complaint_prediction_model import ComplaintPredictionModel

@bp_complaint.route("/predict", methods=["POST"])
def predict_complaint():
    data = request.get_json()
    if not data or 'complaint' not in data:
        raise Exception("Missing required field: 'complaint'")

    complaint_text = data['complaint']

    model = ComplaintPredictionModel()
    prediction, proba = model.predict(complaint_text)
    return jsonify({"prediction": prediction, "probabilities": str(proba.tolist())}), 200

@bp_complaint.route("/retrain", methods=["POST"])
def retrain_complaint():
    model = ComplaintPredictionModel()
    trained_model = model.train()

    json_model = {
        "vectorizer": str(trained_model.get("vectorizer")),
        "model": str(trained_model.get("model")),
        "best_score": trained_model.get("best_score"),
    }

    return jsonify({"status": "Complaint model retrained successfully", "model": json_model}), 200