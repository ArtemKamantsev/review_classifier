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
.\venv\Scripts\python.exe .\api_evaluate.py -c "The best app!"
```
Output: single string ('NEGATIVE' or 'POSITIVE')
### Recognize '....csv' file:
```
.\venv\Scripts\python.exe .\api_evaluate.py -p path_to_file\file_name.csv
```
Output: array of strings (each equal to 'NEGATIVE' or 'POSITIVE')
## Run api_train.py
```
.\venv\Scripts\python.exe .\api_train.py -d "[{\"text\": \"the worst app\", \"score\": 1},{\"text\": \"the best app\", \"score\": 5}]"
```
Output: string with result of trained model evaluation metrics