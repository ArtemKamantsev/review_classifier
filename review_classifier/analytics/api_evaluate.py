from os.path import splitext, exists

import base64
import pandas as pd
# import pydotplus
from joblib import load
from sklearn.tree import export_graphviz

from service.constants import VECTORIZER_PATH_TEMPLATE, VECTORIZER_PARAMS_PATH_TEMPLATE, MODEL_PATH_TEMPLATE, \
    VECTORIZER_DEFAULT_PATH_TEMPLATE, VECTORIZER_PARAMS_DEFAULT_PATH_TEMPLATE, MODEL_DEFAULT_PATH_TEMPLATE, \
    MODEL_IMAGE_TEMPLATE
from service.utils import load_count_vectorizer, print_output
from service.utils import vocabulary_to_features

classes_mapping = {
    0: 'NEGATIVE',
    1: 'POSITIVE',
}


def get_model_paths(working_directory):
    vectorizer_path = VECTORIZER_PATH_TEMPLATE.substitute(working_directory=working_directory)
    vectorizer_params_path = VECTORIZER_PARAMS_PATH_TEMPLATE.substitute(working_directory=working_directory)
    model_path = MODEL_PATH_TEMPLATE.substitute(working_directory=working_directory)
    if not exists(vectorizer_path) or not exists(vectorizer_params_path) or not exists(model_path):
        vectorizer_path = VECTORIZER_DEFAULT_PATH_TEMPLATE.substitute(working_directory=working_directory)
        vectorizer_params_path = VECTORIZER_PARAMS_DEFAULT_PATH_TEMPLATE.substitute(working_directory=working_directory)
        model_path = MODEL_DEFAULT_PATH_TEMPLATE.substitute(working_directory=working_directory)

    model_image_path = MODEL_IMAGE_TEMPLATE.substitute(working_directory=working_directory)

    return vectorizer_path, vectorizer_params_path, model_path, model_image_path


def classify(comment_list, working_directory, create_images=True):
    vectorizer_path, vectorizer_params_path, model_path, model_image_path = get_model_paths(working_directory)

    vectorizer = load_count_vectorizer(vectorizer_path, vectorizer_params_path)
    model = load(model_path)

    comment_vectorized = vectorizer.transform(comment_list)
    prediction_list = model.predict(comment_vectorized)

    result_images = []
    # if create_images:
    #     for comment in comment_vectorized:
    #         features = vocabulary_to_features(vectorizer.vocabulary_)
    #         dot_data = export_graphviz(
    #             model,
    #             feature_names=features,
    #             class_names=['Negavite', 'Positive'],
    #             filled=True
    #         )
    #
    #         graph = pydotplus.graph_from_dot_data(dot_data)
    #         graph.del_node('"\\n"')
    #
    #         decision_path = model.decision_path(comment)
    #
    #         for n, node_value in enumerate(decision_path.toarray()[0]):
    #             if node_value == 0:
    #                 node = graph.get_node(str(n))[0]
    #                 node.set_fillcolor('white')
    #             else:
    #                 node = graph.get_node(str(n))[0]
    #                 node.set_fillcolor('green')
    #
    #         graph.write_png(model_image_path)
    #
    #         result_images.append(base64.b64encode(open(model_image_path, "rb").read()).decode())

    return list(map(lambda p: classes_mapping[p], prediction_list)), result_images


def get_comments_from_path(path):
    _, extension = splitext(path)
    if extension != '.csv':
        raise Exception('Only .csv files are acceptable')

    df = pd.read_csv(path)
    if df.shape[0] == 0:
        raise Exception('.csv file should contain at least 1 row')

    return df.iloc[:, 0]  # return first column


def evaluate(working_directory, comment, path):
    data = None
    error = None
    try:
        if comment:
            result_list, result_images = classify([comment], working_directory)

            data = {
                "result": result_list[0],
                "image_base64": result_images[0] if len(result_images) > 0 else "",
            }
        else:
            comment_list = get_comments_from_path(path)
            result_list, _ = classify(comment_list, working_directory, False)
            data = {
                "result": result_list,
                "image_base64": None,
            }
    except Exception as e:
        error = str(e)

    print_output(data, error)
