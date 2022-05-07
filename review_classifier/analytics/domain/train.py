import base64
import numpy as np
# import pydotplus
from joblib import dump
from sklearn.feature_extraction.text import CountVectorizer
from sklearn.metrics import recall_score, precision_score, roc_auc_score
from sklearn.model_selection import train_test_split
from sklearn.tree import DecisionTreeClassifier, export_graphviz

from service.constants import RANDOM_STATE, TEST_SET_SIZE, MIN_TEST_ITEMS_COUNT, VECTORIZER_PATH_TEMPLATE, \
    VECTORIZER_PARAMS_PATH_TEMPLATE, MODEL_PATH_TEMPLATE, MODEL_IMAGE_TEMPLATE
from service.utils import vocabulary_to_features

TEXT_COLUMN_KEY = 'text'
SCORE_COLUMN_KEY = 'score'

default_params = {
    'criterion': 'gini',
    'max_depth': 5,
}


def check_params(model_params):
    """Ensure all keys from 'default_params' are present in 'model_params'"""
    for key, value in default_params.items():
        if key not in model_params:
            model_params[key] = value

    return model_params


def train_model(df, model_params, working_directory):
    if TEXT_COLUMN_KEY not in df.columns or SCORE_COLUMN_KEY not in df.columns:
        raise Exception(f'Data should contain "{TEXT_COLUMN_KEY}" and "{SCORE_COLUMN_KEY}" columns')

    if len(df) == 0:
        raise Exception('Data should contain at least 1 entry')

    path_vectorizer = VECTORIZER_PATH_TEMPLATE.substitute(working_directory=working_directory)
    path_vectorizer_params = VECTORIZER_PARAMS_PATH_TEMPLATE.substitute(working_directory=working_directory)
    path_model = MODEL_PATH_TEMPLATE.substitute(working_directory=working_directory)
    model_image_path = MODEL_IMAGE_TEMPLATE.substitute(working_directory=working_directory)

    labels = (df['score'] >= 3).astype(np.int32).values

    min_df = min(round(len(df) * 0.0001), 1000)  # not greater when 1000
    vectorizer = CountVectorizer(
        strip_accents='unicode',
        lowercase=True,
        stop_words='english',
        token_pattern=r'(?u)(\b[a-z]{2,}\b|[\u263a-\U0001f645])',
        ngram_range=(1, 2),
        min_df=min_df,
        binary=True
    )
    dump(vectorizer, path_vectorizer)

    text_vectorized = vectorizer.fit_transform(df['text'])

    vectorizer_params = {
        'vocabulary_': vectorizer.vocabulary_,
    }
    dump(vectorizer_params, path_vectorizer_params)

    if len(df) * TEST_SET_SIZE >= MIN_TEST_ITEMS_COUNT:
        x_train, x_test, y_train, y_test = train_test_split(text_vectorized, labels, test_size=TEST_SET_SIZE,
                                                            random_state=RANDOM_STATE)
        test_dataset_type = 'test'
    else:
        x_train, x_test, y_train, y_test = text_vectorized, text_vectorized, labels, labels
        test_dataset_type = 'train'

    model_params = check_params(model_params)
    model = DecisionTreeClassifier(
        class_weight='balanced',
        **model_params,
    )
    model.fit(x_train, y_train)
    dump(model, path_model)

    prediction_probas = model.predict_proba(x_test)
    predictions = np.argmax(prediction_probas, axis=1)
    recall = recall_score(y_test, predictions)
    precision = precision_score(y_test, predictions)
    roc_auc = roc_auc_score(y_test, prediction_probas[:, 1])

    # features = vocabulary_to_features(vectorizer.vocabulary_)
    # dot_data = export_graphviz(
    #     model,
    #     feature_names=features,
    #     class_names=['Negavite', 'Positive'],
    #     filled=True
    # )
    # graph = pydotplus.graph_from_dot_data(dot_data)
    #
    # graph.del_node('"\\n"')
    # graph.write_png(model_image_path)
    # model_image_base64 = base64.b64encode(open(model_image_path, "rb").read()).decode()

    result_message = f'Model has been trained successfully! \nTested on {test_dataset_type} dataset of size: {x_test.shape[0]}. \nRecall: {round(recall, 4)} \nPrecision: {round(precision, 4)} \nRoc-Auc: {round(roc_auc, 4)}.'

    return {
        'result': result_message,
        'image_base64': '',
    }
