# Run model 'analytics'
To be able to run any script from 'analytics' module you should perform following steps:
1. Go to 'analytics' folder
```cd ./analytics```
2. Create folder 'models'
3. Save files form https://drive.google.com/drive/folders/1Dwd2xeR5S7EAkM7I3btivcv_O302Ortr?usp=sharing to 'models' folder
4. Create virtual environment
```python -m venv venv```
5. Activate environment (Windows)
```.\venv\Scripts\activate```
6. Install dependencies
```pip install -r requirements.txt```
7. Install IPython kernel
```python -m ipykernel install --user --name review_classifier –display-name “review_classifier”```
8. To use Jupyter Notebooks: run any cell of any of Jupyter Notebooks and ensure:
   1. Click "Managed: http..." -> Configure Jupyter Server -> Python Interpreter in set to analytics\venv\Scripts\python.exe. If change was made - restart server ("Jupyter" window -> stop icon -> run any cell)
   2. In the box next to "Managed: http..." ensure "review_classifier" is selected. If change was made - restart kernel (button in top navigate panel)

# API
Each api method prints to standard output json-formatted string of following format:
```
{
    "data": output or null,
    "error": string or null,
}
```
where 'output' value differs for each of methods.

Following commands should be run from 'analytics' folder.
## Run api_evaluate.py
### Recognize single string:
```
.\venv\Scripts\python.exe .\api.py -v evaluate -c "The best app!"
```
Output: single string ('NEGATIVE' or 'POSITIVE')
Response example:
```
{"data": {"result": "POSITIVE", "image_base64": "todo"}, "error": null}
```
### Recognize '....csv' file:
```
.\venv\Scripts\python.exe .\api.py -v evaluate -p path_to_file\file_name.csv
```
Output: array of strings (each equal to 'NEGATIVE' or 'POSITIVE')
Response example:
```
{"data": {"result": ["POSITIVE", "NEGATIVE"], "image_base64": null}, "error": null}
```
## Run api_train.py
```
.\venv\Scripts\python.exe .\api.py -v train
{"max_depth":3, "criterion":"gini"}
[{"text": "the worst app", "score": 1},{"text": "the best app", "score": 5}]
```
Output: string with result of trained model evaluation metrics
Response example:
```
{"data": {"result": "Model has been trained successfully!\nTested on train dataset of size: 2\nrecall: 1.0\nprecision: 1.0\nroc-auc: 1.0", "image_base64": "todo"}, "error": null}
```