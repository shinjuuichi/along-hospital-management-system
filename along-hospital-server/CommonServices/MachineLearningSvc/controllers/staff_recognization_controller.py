from models import staff_recognization_model
from . import bp_staff_recognization
from flask import jsonify, request
from models.staff_recognization_model import StaffRecognizationModel

staff_recognization_model = StaffRecognizationModel()

@bp_staff_recognization.route("/recognize", methods=["POST"])
def recognize_staff():
    file = request.files.get("file")
    if not file:
        raise Exception("No file provided")

    file_bytes = file.read()

    result = staff_recognization_model.recognize_staff_from_bytes(file_bytes)

    return jsonify({"staffId": result["staffId"]}), 200


@bp_staff_recognization.route("/check-exist/<staff_id>", methods=["GET"])
def check_staff_exist(staff_id):
    is_exist = staff_recognization_model.has_images_for_staff(staff_id)
    return jsonify({"exist": is_exist}), 200


@bp_staff_recognization.route("/enroll", methods=["POST"])
def enroll_staff():
    staff_id = request.args.get("staffId")
    if not staff_id:
        raise Exception("Missing staffId")

    files = request.files.getlist("images")
    if not files:
        raise Exception("No images provided")

    file_bytes_list = [f.read() for f in files]
    enqueue_result = staff_recognization_model.enqueue_staff_images(
        staff_id,
        file_bytes_list,
    )

    if enqueue_result["isAlreadyProcessing"]:
        return jsonify(
            {
                "message": "Enrollment is already processing for this staff. Please wait 5-10 minutes before trying again.",
                "acceptedFaces": 0,
            }
        ), 202

    return jsonify(
        {
            "message": "Valid face images accepted. Enrollment is processing in background, please wait 5-10 minutes.",
            "acceptedFaces": enqueue_result["acceptedFaces"],
        }
    ), 202


@bp_staff_recognization.route("/evaluate", methods=["GET"])
def evaluate_model():
    evaluation = staff_recognization_model.evaluate_model()
    return jsonify(evaluation), 200


@bp_staff_recognization.route("/reset-identification/<staff_id>", methods=["DELETE"])
def reset_identification(staff_id):
    reset_result = staff_recognization_model.reset_staff_identification(staff_id)
    return jsonify(
        {
            "message": "Identification reset successfully. You can enroll again.",
            "removedCount": reset_result["removedCount"],
        }
    ), 200