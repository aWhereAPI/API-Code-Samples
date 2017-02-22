"""This script gets a list of fields you have under your app.

It uses functions defined in _0_header.py
"""

import json

from _0_header import MakeAPICall, auth_token

url = 'https://api.awhere.com/v2/fields'

try:
    get_fields = MakeAPICall('GET', 
        url = 'https://api.awhere.com/v2/fields',    
        token = auth_token)

except Exception as responseException:
    print responseException

print "<h2> attempting to get list of fields.... </h2>"

if get_fields.status_code == 200 :
    print "<p> got fields! </p>"
    print get_fields.content

else:
    print "ERROR: ", get_fields.status_code," \n - \n ", get_fields.content