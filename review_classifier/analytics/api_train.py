import json

import pandas as pd
import sys

from domain.train import train_model
from service.utils import print_output


def parse_params(params_str='{"max_depth":3, "criterion":"gini"}'):
    params_obj = json.loads(params_str)

    return params_obj


def parse_data(data_str='[{"text": "the worst app", "score": 1},{"text": "the best app", "score": 5}]'):
    data_obj = json.loads(data_str)
    df = pd.DataFrame(data_obj)

    return df


if __name__ == '__main__':
    working_directory = sys.path[0]
    input_params = input()
    input_data = input()

    result_data = None
    error = None
    try:
        model_params = parse_params(input_params)
        df = parse_data(input_data)
        result_data = train_model(df, model_params, working_directory)
    except Exception as e:
        error = str(e)

    print_output(result_data, error)
