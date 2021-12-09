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

# Run api.py
From 'analytics' folder:
```
.\venv\Scripts\python.exe .\api.py -c "The best app!"
```