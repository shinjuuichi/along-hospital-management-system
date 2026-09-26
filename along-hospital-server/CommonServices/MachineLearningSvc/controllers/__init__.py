import pkgutil
import importlib
import os
from flask import Blueprint

bp_complaint = Blueprint("complaint_prediction", __name__, url_prefix="/api/complaint")
bp_feedback = Blueprint("feedback_prediction", __name__, url_prefix="/api/feedback")
bp_staff_recognization = Blueprint(
    "staff_recognization", __name__, url_prefix="/api/staff-recognization"
)
bp_vector_embedding = Blueprint(
    "vector_embedding", __name__, url_prefix="/api/vector-embedding"
)

list_blueprints = [
    bp_complaint,
    bp_feedback,
    bp_staff_recognization,
    bp_vector_embedding,
]


for loader, module_name, is_pkg in pkgutil.iter_modules([os.path.dirname(__file__)]):
    importlib.import_module(f"{__name__}.{module_name}")
