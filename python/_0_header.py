"""
aWhere Code Samples
License: MIT 
Author: Vince Buscarello (vince.buscarello@gmail.com)  

These code samples show a variety of different use cases and demonstrate 
how to make API calls in Python using the Requests library. 

Why use requests vs httplib2 --
http://docs.python-requests.org/en/master/community/faq/

Each file shows a different use case. It is currently designed so that if you
load the file to a browser and access it from a server, you will see 
prettified results in HTML. I am hoping to add a flask example interface soon.

This file is the best place for your application specific configurations
like Key and Secret. Be sure to remove before commiting to any public repos!

This file also defines the functions to get the auth token and make API calls.
"""


import requests as rq
import base64

from _0_header_requests import GetOAuthToken

api_key = " "
api_secret = " "
auth_token = GetOAuthToken(api_key, api_secret)['access_token']

def GetOAuthToken(Key, Secret, test_it = False):
    '''This function returns the OAuth token and its expiration.
    it is only currently coded to define the function, which is imported 
    and used in other scripts.
    
    It features an optional switch to print the Token to the console for 
    debugging purposes.
    '''

    encoded_key_secret = base64.b64encode('%s:%s' % (Key, Secret)).replace('\n', '')

    auth_url = 'https://api.awhere.com/oauth/token'

    auth_headers = {
        "Authorization":"Basic %s" % encoded_key_secret, 
        'Content-Type':'application/x-www-form-urlencoded'
    }

    body="grant_type=client_credentials"

    get_the_token = rq.post('https://api.awhere.com/oauth/token', 
                            headers=auth_headers, 
                            data=body)

    # .json method is a requests lib method that decodes the response
    if test_it:
        print 'got token'
        print get_the_token.json()

    return  get_the_token.json()


def MakeAPICall(verb, url, token, headers=None, body=None):
    '''Takes result of the above as token, 
    and uses the verb to define a requests and return data.'''

    # headers are defaulted to the most commonly used, can be overwritten
    # by passing a headers keyword arg.
    if not headers:
        headers = { 
            'Authorization': 'Bearer %s' % token, 
            'Content-Type': 'application/json'
        }

    # I use lower() here for argument simplicity - if the user pases caps or
    # lower case, or some combo, the request still runs smoothly.
    if verb.lower() == 'get':
        call = rq.get(url, headers = headers, data = body)

    elif verb.lower() == 'post':
        call = rq.post(url, headers = headers, data = body)

    return call