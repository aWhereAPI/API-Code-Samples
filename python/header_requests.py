#why use requests vs httplib2
#http://docs.python-requests.org/en/master/community/faq/


''' Once the following is tested and resolved I will run a pull request.
I am still experiencing problemst with the API token
see -

So I am keeping this on my for for now
'''

import requests as rq
import base64


Key = 'wwJN2yTlGEVzB2SAfnN7A5215B2Eshjj'
Secret = 'ksyXJbpbzRbGDObw'

d3dKTjJ5VGxHRVZ6QjJTQWZuTjdBNTIxNUIyRXNoamo6a3N5WEpicGJ6UmJHRE9idw==


Token = GetOAuthToken(Key, Secret)




def GetOAuthToken(Key, Secret):

    encoded_key_secret = base64.b64encode('%s:%s' % (Key, Secret)).replace('\n', '')

    auth_url = 'https://api.awhere.com/oauth/token'

    auth_headers = {'Authorization':"Basic %s" % encoded_key_secret, 
    'Content-Type':'application/x-www-form-urlencoded\\'}


    authr = rq.Request('POST',
     auth_url,
     data="grant_type=client_credentials\\", 
     headers=auth_headers)

    authrp  = authr.prepare()
    s = rq.Session()
    resp = s.send(authrp)


"""line by line run -
-- Out[17]: '{"access_token":"","expires_in":}'

this is the same when Key and Secret are gibberish, so something is wrong with the 
encoding or something.....
""" 

#meta data http://developer.awhere.com/integration/tutorials
#calls http://developer.awhere.com/api/get-started
