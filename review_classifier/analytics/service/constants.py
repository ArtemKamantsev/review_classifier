from string import Template

RANDOM_STATE = 42
TEST_SET_SIZE = 0.1
MIN_TEST_ITEMS_COUNT = 1

VECTORIZER_PATH_TEMPLATE = Template('$working_directory/models/vectorizer.sav')
VECTORIZER_PARAMS_PATH_TEMPLATE = Template('$working_directory/models/vectorizer_params.sav')
MODEL_PATH_TEMPLATE = Template('$working_directory/models/model.sav')

VECTORIZER_DEFAULT_PATH_TEMPLATE = Template('$working_directory/models/vectorizer_default.sav')
VECTORIZER_PARAMS_DEFAULT_PATH_TEMPLATE = Template('$working_directory/models/vectorizer_params_default.sav')
MODEL_DEFAULT_PATH_TEMPLATE = Template('$working_directory/models/model_default.sav')
