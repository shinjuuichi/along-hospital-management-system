from sklearn.linear_model import LinearRegression, Ridge, Lasso, LogisticRegression
from sklearn.tree import DecisionTreeRegressor, DecisionTreeClassifier
from sklearn.ensemble import RandomForestRegressor, RandomForestClassifier
from sklearn.svm import SVC
from sklearn.naive_bayes import GaussianNB, MultinomialNB
from sklearn.model_selection import GridSearchCV, ShuffleSplit
from sklearn.preprocessing import FunctionTransformer, StandardScaler
from sklearn.pipeline import make_pipeline
import pandas as pd
import numpy as np

_regression_model_params = {
    "linear_regression": {
        "model": LinearRegression(),
        "params": {},
    },
    "ridge": {
        "model": Ridge(),
        "params": {"alpha": [1, 2, 5, 10], "solver": ["auto", "svd", "cholesky"]},
    },
    "lasso": {
        "model": Lasso(),
        "params": {"alpha": [1, 2], "selection": ["cyclic", "random"]},
    },
    "decision_tree_regressor": {
        "model": DecisionTreeRegressor(),
        "params": {
            "criterion": ["squared_error", "absolute_error", "friedman_mse"],
            "splitter": ["best", "random"],
        },
    },
    "random_forest_regressor": {
        "model": RandomForestRegressor(),
        "params": {
            "criterion": ["squared_error", "absolute_error", "friedman_mse"],
            "n_estimators": [1, 5, 10, 20, 40],
        },
    },
}

_classification_model_params = {
    "logistic_regression": {
        "model": LogisticRegression(max_iter=10000),
        "params": {"C": [1, 5, 10, 20, 40]},
    },
    "svc": {
        "model": SVC(gamma="auto", probability=True),
        "params": {"C": [1, 10, 40, 100, 1000], "kernel": ["rbf", "linear", "sigmoid"]},
    },
    "decision_tree_classifier": {
        "model": DecisionTreeClassifier(),
        "params": {"criterion": ["gini", "entropy"]},
        "needs_dense": True,
    },
    "random_forest": {
        "model": RandomForestClassifier(),
        "params": {
            "criterion": ["gini", "entropy"],
            "n_estimators": [1, 5, 10, 20, 40],
        },
        "needs_dense": True,
    },
    "gaussian_nb": {"model": GaussianNB(), "params": {}, "needs_dense": True},
    "multinomial_nb": {"model": MultinomialNB(), "params": {}},
}

_densify = FunctionTransformer(lambda X: X.toarray(), accept_sparse=True)


def _find_best_model(X, y, model_params, with_standard_scaler=False):
    scores = []
    cv = ShuffleSplit(n_splits=5, test_size=0.2, random_state=10)

    for model_name, mp in model_params.items():
        model = mp["model"]
        params = mp["params"]
        dense_required = mp.get("needs_dense", False)

        steps = []
        if dense_required:
            steps.append(("densify", _densify))
        if with_standard_scaler:
            steps.append(("scaler", StandardScaler(with_mean=not dense_required)))

        steps.append(("model", model))

        pipeline = make_pipeline(*[m for _, m in steps])

        params = {f"model__{k}": v for k, v in params.items()}

        try:
            clf = GridSearchCV(
                pipeline,
                params,
                cv=cv,
                return_train_score=False,
                n_jobs=-1,
                error_score=np.nan,
            )
            clf.fit(X, y)

            scores.append(
                {
                    "model": model_name,
                    "best_params": clf.best_params_,
                    "best_score": clf.best_score_,
                    "estimator": clf.best_estimator_,
                }
            )
        except Exception as e:
            scores.append(
                {
                    "model": model_name,
                    "best_params": None,
                    "best_score": np.nan,
                    "estimator": None,
                }
            )

    results_df = pd.DataFrame(scores)
    results_df = results_df.dropna(subset=["best_score"])
    if results_df.empty:
        return None, 0

    best_row = results_df.loc[results_df["best_score"].idxmax()]
    return best_row["estimator"], float(best_row["best_score"])


def find_best_classification_model(X, y, with_standard_scaler=False):
    return _find_best_model(
        X, y, _classification_model_params, with_standard_scaler=with_standard_scaler
    )


def find_best_regression_model(X, y, with_standard_scaler=False):
    return _find_best_model(
        X, y, _regression_model_params, with_standard_scaler=with_standard_scaler
    )