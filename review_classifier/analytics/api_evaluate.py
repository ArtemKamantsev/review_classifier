import argparse
import sys
from os.path import splitext
from string import Template

import pandas as pd
from joblib import load

from service.utils import load_count_vectorizer

VECTORIZER_PATH_TEMPLATE = Template('$working_directory/models/vectorizer.sav')
VECTORIZER_VOCABULARY_PATH_TEMPLATE = Template('$working_directory/models/vocabulary.sav')
MODEL_PATH_TEMPLATE = Template('$working_directory/models/model.sav')

classes_mapping = {
    0: 'NEGATIVE',
    1: 'POSITIVE',
}


def classify(comment_list, working_directory):
    vectorizer_path = VECTORIZER_PATH_TEMPLATE.substitute(working_directory=working_directory)
    vocabulary_path = VECTORIZER_VOCABULARY_PATH_TEMPLATE.substitute(working_directory=working_directory)
    model_path = MODEL_PATH_TEMPLATE.substitute(working_directory=working_directory)

    vectorizer = load_count_vectorizer(vectorizer_path, vocabulary_path)
    model = load(model_path)

    comment_vectorized = vectorizer.transform(comment_list)
    prediction_list = model.predict(comment_vectorized)

    return list(map(lambda p: classes_mapping[p], prediction_list))


def get_comments_from_path(path):
    _, extension = splitext(path)
    if extension != '.csv':
        raise Exception('Only .csv files are acceptable')

    df = pd.read_csv(path)
    if df.shape[0] == 0:
        raise Exception('.csv file should contain at least 1 row')

    return df.iloc[:, 0]  # return first column


def main(working_directory, comment, path):
    if comment:
        res = classify([comment], working_directory)[0]
    else:
        try:
            comment_list = get_comments_from_path(path)
            res = classify(comment_list, working_directory)
        except Exception as e:
            res = str(e)

    print(res)


if __name__ == '__main__':
    parser = argparse.ArgumentParser('Review classifier')
    group = parser.add_mutually_exclusive_group(required=True)
    group.add_argument('-c', type=str)
    group.add_argument('-p', type=str)
    args = parser.parse_args()

    main(sys.path[0], args.c, args.p)

# USAGE FROM C#
# ProcessStartInfo start = new ProcessStartInfo();
# start.FileName = "D:\\Education\\Programming\\review_classifier\\analytics\\venv\\Scripts\\python.exe";
# start.Arguments = string.Format("{0} -c \"{1}\"", "D:\\Education\\Programming\\review_classifier\\analytics\\api.py", "worst app!");
# start.UseShellExecute = false;
# start.RedirectStandardOutput = true;
# using(Process process = Process.Start(start))
# {
#     using(StreamReader reader = process.StandardOutput)
#     {
#         string result = reader.ReadToEnd();
#         MessageBox.Show(result);
#     }
# }
