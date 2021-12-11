import glob
import os
import json
import numpy as np
import pandas as pd
from joblib import load
from tqdm import tqdm


def read_data(path='../../local_data/raw'):
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


def load_count_vectorizer(vectorizer_path, vocabulary_path):
    vectorizer = load(vectorizer_path)
    vocabulary = load(vocabulary_path)
    vectorizer.vocabulary_ = vocabulary

    return vectorizer


def print_output(data, error):
    res = {'data': data, 'error': error}
    res_str = json.dumps(res)

    print(res_str)
