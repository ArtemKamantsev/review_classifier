import json

from joblib import load


def load_count_vectorizer(vectorizer_path, vectorizer_params_path):
    vectorizer = load(vectorizer_path)
    vectorizer_params = load(vectorizer_params_path)
    vectorizer.vocabulary_ = vectorizer_params['vocabulary_']

    return vectorizer


def print_output(data, error):
    res = {'data': data, 'error': error}
    res_str = json.dumps(res)

    print(res_str)


def vocabulary_to_features(vocabulary):
    return list(map(lambda x: x[0], sorted(vocabulary.items(), key=lambda x: x[1])))
