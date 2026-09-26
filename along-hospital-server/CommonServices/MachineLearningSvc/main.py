import os, sys, subprocess, pathlib, hashlib

ROOT = pathlib.Path(__file__).resolve().parents[0]
VENV = ROOT / ".venv"
REQ = ROOT / "requirements.txt"


def ensure_and_install():
    if not VENV.exists():
        subprocess.check_call([sys.executable, "-m", "venv", str(VENV)])

    if not REQ.exists():
        return

    python_exe = str(VENV / ("Scripts/python.exe" if os.name == "nt" else "bin/python"))
    hash_file = ROOT / ".requirements.hash"

    def file_hash(p):
        h = hashlib.sha256()
        with p.open("rb") as f:
            for chunk in iter(lambda: f.read(8192), b""):
                h.update(chunk)
        return h.hexdigest()

    cur = file_hash(REQ)
    prev = hash_file.read_text().strip() if hash_file.exists() else None

    if cur != prev:
        subprocess.check_call([python_exe, "-m", "pip", "install", "--upgrade", "pip"])
        subprocess.check_call([python_exe, "-m", "pip", "install", "-r", str(REQ)])
        hash_file.write_text(cur)


if __name__ == "__main__":
    ensure_and_install()

    from configurations.server import create_app
    app = create_app()
    app.run(host=app.config["FLASK_RUN_HOST"], port=int(app.config["FLASK_RUN_PORT"]))

