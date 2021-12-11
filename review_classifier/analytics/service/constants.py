from string import Template

RANDOM_STATE = 42
TEST_SET_SIZE = 0.1
MIN_TEST_ITEMS_COUNT = 1

VECTORIZER_PATH_TEMPLATE = Template('$working_directory/models/vectorizer.sav')
VECTORIZER_VOCABULARY_PATH_TEMPLATE = Template('$working_directory/models/vocabulary.sav')
MODEL_PATH_TEMPLATE = Template('$working_directory/models/model.sav')
