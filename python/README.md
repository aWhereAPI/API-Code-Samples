# aWhere API Python Sample Application

## Getting started
1. Install dependencies
* Requests: The `requests` library is used for making HTTP requests in Python

```
$ pip install requests
```

2. Clone this repo
```
$ git clone https://github.com/aWhereAPI/API-Code-Samples.git
```
3. Navigate to `API-Code-Samples/python`. Open the `awhere_demo.py` file. 
    * Enter your app's secret and key accordingly
    ``` python
    # Enter your Api Key and Secret below:
    api_key = '' 
    api_secret = '' 
    ```
    and save the file. 
4. From your command line, type `python awhere_demo.py`

## Troubleshooting

Q: The following error appears when I attempt to run the demo:
```
Error: Missing API Key.
Please place your API key in the main script and try again.
```
A: Enter your key and secret inside of the appropriate variables located in the `awhere_demo.py` file. Make sure you are not including any whitespace.

