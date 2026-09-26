from . import bp_vector_embedding
from flask import jsonify, request
from sentence_transformers import SentenceTransformer

vector_model = SentenceTransformer("intfloat/multilingual-e5-base")

@bp_vector_embedding.route("/embed", methods=["POST"])
def embed_text():
    data = request.get_json()
    text = data.get("text")
    if not text:
        raise Exception("No text provided")

    vector = vector_model.encode(f"query: {text}", normalize_embeddings=True)

    return jsonify({"vector": vector.tolist()}), 200