import numpy as np
from joblib import dump
from sklearn.feature_extraction.text import CountVectorizer
from sklearn.metrics import recall_score, precision_score, roc_auc_score
from sklearn.model_selection import train_test_split
from sklearn.naive_bayes import MultinomialNB

from service.constants import RANDOM_STATE, TEST_SET_SIZE, MIN_TEST_ITEMS_COUNT, VECTORIZER_PATH_TEMPLATE, \
    VECTORIZER_VOCABULARY_PATH_TEMPLATE, MODEL_PATH_TEMPLATE

TEXT_COLUMN_KEY = 'text'
SCORE_COLUMN_KEY = 'score'


def train_model(df, working_directory):
    if TEXT_COLUMN_KEY not in df.columns or SCORE_COLUMN_KEY not in df.columns:
        raise Exception(f'Data should contain "{TEXT_COLUMN_KEY}" and "{SCORE_COLUMN_KEY}" columns')

    if len(df) == 0:
        raise Exception('Data should contain at least 1 entry')

    path_vectorizer = VECTORIZER_PATH_TEMPLATE.substitute(working_directory=working_directory)
    path_vectorizer_vocabulary = VECTORIZER_VOCABULARY_PATH_TEMPLATE.substitute(working_directory=working_directory)
    path_model = MODEL_PATH_TEMPLATE.substitute(working_directory=working_directory)

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
    dump(vectorizer.vocabulary_, path_vectorizer_vocabulary)

    if len(df) * TEST_SET_SIZE >= MIN_TEST_ITEMS_COUNT:
        x_train, x_test, y_train, y_test = train_test_split(text_vectorized, labels, test_size=TEST_SET_SIZE,
                                                            random_state=RANDOM_STATE)
        test_dataset_type = 'test'
    else:
        x_train, x_test, y_train, y_test = text_vectorized, text_vectorized, labels, labels
        test_dataset_type = 'train'

    model = MultinomialNB()
    model.fit(x_train, y_train)
    dump(model, path_model)

    prediction_probas = model.predict_proba(x_test)
    predictions = np.argmax(prediction_probas, axis=1)
    recall = recall_score(y_test, predictions)
    precision = precision_score(y_test, predictions)
    roc_auc = roc_auc_score(y_test, prediction_probas[:, 1])
    result = f'Model has been trained successfully!\nTested on {test_dataset_type} dataset of size: {x_test.shape[0]}\nrecall: {recall}\nprecision: {precision}\nroc-auc: {roc_auc}'

    return result
