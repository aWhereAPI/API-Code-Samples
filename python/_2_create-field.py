"""This script adds a field to your app.

It uses 
- functions defined in _0_header.py
- data defined in testvars.py
"""

import json

import testvars
from _0_header import MakeAPICall, auth_token

print "<h2>attempting to create a field</h2>"
print "<hr>"

host = 'https://api.awhere.com/'

try:
   new_field_post = makeAPICall('POST', 
            url = host + '/v2/fields', 
            token = auth_token, 
            body = testvars.test_field_dict)

except Exception as responseException:
    print responseException
    
    
if new_field_post.statusCode == 201 :
    print "request returned success!"
    print "<hr>"
    print "Request: " new_field_post.request.method, new_field_post.request.body
elif statusCode == 409 :
    print "<hr>"
    print "A field with ID ",testvars.test_field_dict,
    print "already exists in your account, so it could not be created again." 
    print new_field_post.content
else:
    print "ERROR: ", new_field_post.status_code," \n - \n ", new_field_post.content
