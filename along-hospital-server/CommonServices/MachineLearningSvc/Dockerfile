# syntax=docker/dockerfile:1.6

FROM python:3.12-slim-bookworm AS builder

ENV PYTHONDONTWRITEBYTECODE=1 \
    PYTHONUNBUFFERED=1 \
    NLTK_DATA=/usr/local/nltk_data \
    PATH="/venv/bin:$PATH"

RUN apt-get update && apt-get install -y --no-install-recommends \
    unixodbc-dev \
    curl \
    ca-certificates \
 && rm -rf /var/lib/apt/lists/*

RUN python -m venv /venv

COPY CommonServices/MachineLearningSvc/requirements.txt /tmp/requirements.txt

RUN --mount=type=cache,target=/root/.cache/pip \
    pip install --upgrade pip wheel setuptools \
 && pip install torch torchvision torchaudio --index-url https://download.pytorch.org/whl/cpu \
 && pip install --prefer-binary --no-cache-dir -r /tmp/requirements.txt \
 && pip install --no-cache-dir gunicorn \
 && pip cache purge

RUN python -m nltk.downloader -d ${NLTK_DATA} punkt stopwords wordnet

RUN find /venv -type d -name "__pycache__" -exec rm -rf {} + \
 && find /venv -type f -name "*.pyc" -delete

# =========================

FROM python:3.12-slim-bookworm

ENV PYTHONDONTWRITEBYTECODE=1 \
    DEBIAN_FRONTEND=noninteractive \
    PYTHONUNBUFFERED=1 \
    PYTHONPATH=/app \
    NLTK_DATA=/usr/local/nltk_data \
    PATH="/venv/bin:$PATH" \
    TF_CPP_MIN_LOG_LEVEL=2 \
    CUDA_VISIBLE_DEVICES=""

RUN set -eux; \
    apt-get update; \
    apt-get install -y --no-install-recommends \
    ca-certificates \
    curl \
    gnupg; \
    curl -fsSL -o /tmp/packages-microsoft-prod.deb https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb; \
    dpkg -i /tmp/packages-microsoft-prod.deb; \
    rm /tmp/packages-microsoft-prod.deb; \
    mkdir -p /opt/microsoft/msodbcsql18; \
    touch /opt/microsoft/msodbcsql18/ACCEPT_EULA; \
    apt-get update; \
    apt-get install -y --no-install-recommends \
    msodbcsql18 \
    unixodbc \
    libgssapi-krb5-2 \
    libgomp1 \
    libglib2.0-0 \
    libgl1 \
    libgles2-mesa \
    libegl1; \

    rm -rf /var/lib/apt/lists/*

COPY --from=builder /venv /venv
COPY --from=builder ${NLTK_DATA} ${NLTK_DATA}

WORKDIR /app
COPY CommonServices/MachineLearningSvc .

RUN adduser -u 5678 --disabled-password --gecos "" appuser \
 && chown -R appuser /app ${NLTK_DATA}

USER appuser

EXPOSE 4996

CMD ["gunicorn","--bind","0.0.0.0:4996","--workers","2","--threads","4","--timeout","600","wsgi:app"]