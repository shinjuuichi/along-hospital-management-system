import re
import nltk
import pandas as pd
import unicodedata
from nltk.corpus import stopwords, wordnet
from nltk.stem import WordNetLemmatizer


def ensure_nltk_resource(resource_path, download_name):
    try:
        nltk.data.find(resource_path)
    except LookupError:
        nltk.download(download_name)


ensure_nltk_resource("corpora/stopwords", "stopwords")
ensure_nltk_resource("tokenizers/punkt", "punkt")
ensure_nltk_resource("tokenizers/punkt_tab", "punkt_tab")
ensure_nltk_resource("corpora/wordnet", "wordnet")

try:
    wordnet.ensure_loaded()
except Exception:
    wordnet.synsets("dog")


_NEGATION_WORDS = {
    "no",
    "not",
    "none",
    "neither",
    "never",
    "nobody",
    "nothing",
    "nowhere",
    "without",
    "against",
    "deny",
    "reject",
    "refuse",
    "decline",
    "unhappy",
    "sad",
    "miserable",
    "hopeless",
    "worthless",
    "useless",
    "futile",
    "disagree",
    "oppose",
    "contrary",
    "contradict",
    "dissatisfied",
    "unsatisfactory",
    "unpleasant",
    "regret",
    "resent",
    "lament",
    "mourn",
    "grieve",
    "bemoan",
    "despise",
    "loathe",
    "fear",
    "worry",
    "anxiety",
    "sorrow",
    "gloom",
    "melancholy",
    "despair",
    "hate",
}


def _expand_suffix_contractions(text):
    text = re.sub(r"can't\b", "can not", text)
    text = re.sub(r"won't\b", "will not", text)
    text = re.sub(r"n't\b", " not", text)
    text = re.sub(r"'re\b", " are", text)
    text = re.sub(r"'s\b", " is", text)
    text = re.sub(r"'d\b", " would", text)
    text = re.sub(r"'ll\b", " will", text)
    text = re.sub(r"'ve\b", " have", text)
    text = re.sub(r"'m\b", " am", text)
    return text


def clean_text(text):
    text = text.lower()
    text = re.sub(r"http\S+|www\S+", "", text)
    text = re.sub(r"<.*?>", "", text)
    text = re.sub("&#039;", "'", text)
    text = _expand_suffix_contractions(text)
    text = re.sub(r"(.)\1{2,}", r"\1\1", text)
    text = unicodedata.normalize("NFKD", text)
    text = "".join(ch for ch in text if ch.isascii())
    text = re.sub(r"[^a-z\s]", "", text)
    text = re.sub(r"\b[a-z]\b", "", text)
    text = re.sub(r"\s+", " ", text).strip()
    return text


_SW = set(stopwords.words("english"))
_SW = _SW.difference(_NEGATION_WORDS)
_LEMMATIZER = WordNetLemmatizer()


def preprocess_text(text):
    if text is None or (isinstance(text, float) and pd.isna(text)):
        return ""

    text = str(text).strip()
    if not text:
        return ""

    text = clean_text(text)

    tokens = nltk.word_tokenize(text)

    tokens = [t for t in tokens if t not in _SW]

    try:
        tokens = [_LEMMATIZER.lemmatize(t) for t in tokens]
    except AttributeError:
        pass

    return " ".join(tokens)
