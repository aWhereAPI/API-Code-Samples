import header
import string
import json

from main import api_key, api_secret

url = 'https://api.awhere.com/v2/fields'

print "<h1>Get Access Token</h1>"

try:
	access_token = GetOAuthToken(api_key, api_secret)
	
except Exception as accessException
	print responseException
	sys.exit(0)  			   

print "<p>Access Token = $access_token</p>"

try:
	statusCode, headers, response = makeAPICall('GET', url,	access_token)
except Exception as responseException
	traceback.print_exc(file=sys.stdout)
	print responseException
	sys.exit(0)  	

print "<hr><h1>Get List of Fields</h1>"
	
if statusCode == 200 :
	print "<p>There are ",len(response.fields)," field locations in the current page of results.</p>" 
	print "<p>Request:</p><pre>GET",url,"</pre>"
	print "<p>Content-Range Header (shows pagination and total results):</p>"	
	print "<pre>".parseHTTPHeaders(headers,array('Content-Range'))."</pre>"; 

	print "<p>Response Body:</p>"
	print "<pre>"
#print  stripslashes(json_encode(response,JSON_PRETTY_PRINT)); 	//Note: Stripslashes() is used just for prettier 
	print "</pre>" 	

else:
	print "<p>ERROR: ",statusCode," - ",response.simpleMessage,"<br>"
	print response.detailedMessage,"</p>"

		   


