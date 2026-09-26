"""
WSGI entrypoint for production deployment with gunicorn.
Do not include development-related code (venv setup, hash checks, etc.).
"""
from configurations.server import create_app

app = create_app()
