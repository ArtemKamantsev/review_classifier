import glob
import json
import os

import numpy as np
import pandas as pd
from joblib import load
from tqdm import tqdm


def read_data(path='../../../local_data/raw'):
    search_path = os.path.join(path, '*.json')
    data_files_list = glob.glob(search_path)
    data_frame_list = []
    for path in tqdm(data_files_list):
        df = pd.read_json(path, orient='records')
        data_frame_list.append(df)

    df = pd.concat(data_frame_list, axis=0)
    print(df.head())

    df['text'] = df['text'].fillna('')
    df['labels'] = (df['score'] >= 3).astype(np.int32).values

    return df


def load_count_vectorizer(vectorizer_path, vectorizer_params_path):
    vectorizer = load(vectorizer_path)
    vectorizer_params = load(vectorizer_params_path)
    vectorizer.vocabulary_ = vectorizer_params['vocabulary_']

    return vectorizer


def load_tf_idf_vectorizer(vectorizer_path, vectorizer_params_path):
    vectorizer = load(vectorizer_path)
    vectorizer_params = load(vectorizer_params_path)
    vectorizer.vocabulary_ = vectorizer_params['vocabulary_']
    vectorizer.idf_ = vectorizer_params['idf_']

    return vectorizer


def print_output(data, error):
    res = {'data': data, 'error': error}
    res_str = json.dumps(res)

    print(res_str)
