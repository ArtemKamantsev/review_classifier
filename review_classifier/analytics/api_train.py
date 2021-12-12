import argparse
import json
import sys

import pandas as pd

from domain.train import train_model
from service.utils import print_output


def parse_data(data_str='[{"text": "the worst app", "score": 1},{"text": "the best app", "score": 5}]'):
    data_obj = json.loads(data_str)
    df = pd.DataFrame(data_obj)

    return df


if __name__ == '__main__':
    parser = argparse.ArgumentParser('Review classifier')
    group = parser.add_argument('-d', required=False, type=str)
    args = parser.parse_args()

    working_directory = sys.path[0]
    input_data = args.d

    result_data = None
    error = None
    try:
        df = parse_data(input_data)
        result_data = train_model(df, working_directory)
    except Exception as e:
        error = str(e)

    print_output(result_data, error)
