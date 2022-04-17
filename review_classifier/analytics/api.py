import argparse

import sys

from api_evaluate import evaluate
from api_train import train
from service.utils import print_output

if __name__ == '__main__':
    parser = argparse.ArgumentParser('Review classifier')
    parser.add_argument('-v', type=str, required=True)
    group = parser.add_mutually_exclusive_group(required=False)
    group.add_argument('-c', type=str)
    group.add_argument('-p', type=str)
    args = parser.parse_args()

    verb = args.v
    if verb == 'train':
        train()
    elif verb == 'evaluate':
        evaluate(sys.path[0], args.c, args.p)
    else:
        print_output(None, f'Unsupported verb: {verb}')
