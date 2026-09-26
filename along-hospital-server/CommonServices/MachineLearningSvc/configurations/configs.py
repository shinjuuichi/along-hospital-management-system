import os
from dotenv import load_dotenv
from urllib.parse import quote_plus

load_dotenv()


def sql_connection_string(host, port, user, password, db_name):
    raw = (
        f"DRIVER=ODBC Driver 18 for SQL Server;"
        f"SERVER={host},{port};"
        f"DATABASE={db_name};"
        f"UID={user};PWD={password};"
        f"Encrypt=yes;TrustServerCertificate=yes;"
    )
    return f"mssql+pyodbc:///?odbc_connect={quote_plus(raw)}"


class Config:
    DB_HOST = os.getenv("DB_HOST", "139.59.226.187")
    DB_PORT = os.getenv("DB_PORT", 1433)
    DB_USER = os.getenv("DB_USER", "sa")
    DB_PASSWORD = os.getenv("DB_PASSWORD", "Hospital@123")
    DB_NAME_MEDICAL_HISTORY = os.getenv(
        "DB_NAME_MEDICAL_HISTORY", "AlongHospital.MedicalHistoryDb"
    )
    DB_NAME_FEEDBACK = os.getenv("DB_NAME_FEEDBACK", "AlongHospital.FeedbackDb")

    FLASK_ENV = os.getenv("FLASK_ENV", "development")
    FLASK_RUN_HOST = os.getenv("FLASK_RUN_HOST", "127.0.0.1")
    FLASK_RUN_PORT = os.getenv("FLASK_RUN_PORT", "7353")

    SQLALCHEMY_BINDS = {
        "medical_history": sql_connection_string(
            DB_HOST, DB_PORT, DB_USER, DB_PASSWORD, DB_NAME_MEDICAL_HISTORY
        ),
        "feedback": sql_connection_string(
            DB_HOST, DB_PORT, DB_USER, DB_PASSWORD, DB_NAME_FEEDBACK
        ),
    }

    SQLALCHEMY_TRACK_MODIFICATIONS = True