import argparse
import sys
from string import Template

from joblib import load

LATEST_VECTORIZER_PATH_TEMPLATE = Template('$working_directory/models/vectorizer_17.sav')
LATEST_MODEL_PATH_TEMPLATE = Template('$working_directory/models/model_17.sav')

classes_mapping = {
    0: 'NEGATIVE',
    1: 'POSITIVE',
}


def classify(comment, working_directory):
    vectorizer_path = LATEST_VECTORIZER_PATH_TEMPLATE.substitute(working_directory=working_directory)
    model_path = LATEST_MODEL_PATH_TEMPLATE.substitute(working_directory=working_directory)

    vectorizer = load(vectorizer_path)
    model = load(model_path)

    comment_vectorized = vectorizer.transform([comment])
    prediction = model.predict(comment_vectorized)
    comment_prediction = prediction[0]

    return classes_mapping[comment_prediction]


if __name__ == '__main__':
    parser = argparse.ArgumentParser('Review classifier')
    parser.add_argument('-c', '--comment', required=True, type=str)
    args = parser.parse_args()
    comment = args.comment
    working_directory = sys.path[0]

    print(classify(comment, working_directory))

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
