import header
import string
import json

api_key = ''
api_secret = ''
url = 'https://api.awhere.com/v2/fields'

print "<h1>Get Access Token</h1>"

try:
	access_token = GetOAuthToken(api_key, api_secret)
	
except Exception as accessException
	print responseException
	sys.exit(0)  	
	
print "<p>Access Token = $access_token</p>"	   
	
print "<hr><h1>Create a Field</h1>"	

fieldBody = {"id" : new_field_id,
			  	"name" : new_field_name,
			  	"farmId" : new_field_farm_id,
			  	"centerPoint" : { "latitude" : new_field_latitude,
								  "longitude" : new_field_longitude},
			  	"acres" : new_field_acres }

#encode with json
jsonEncodedFieldBody = fieldBody 

try:
	statusCode, headers, response = makeAPICall('POST', url, access_token, jsonEncodedFieldBody, {"Content-Type: application/json"})
	
except Exception as responseException
	traceback.print_exc(file=sys.stdout)
	print responseException
	sys.exit(0)  
	
	
if statusCode == 201 :
	print "<p>A new field was created.</p>"
	print "<p>Request:</p><pre>POST ",url,"\n\n",jsonEncodedFieldBody,"</pre>" 
	print "<p>Location Header (shows the URI of the new object):</p>"
	
	print "<pre>".parseHTTPHeaders(headers, {'Location'})."</pre>"
	print "<p>Response Body: (as a matter of convenience we send back the data that was created)</p>"
	print "<pre>"
	#echo stripslashes(json_encode(response,JSON_PRETTY_PRINT)); 	//Note: Stripslashes() is used just for prettier 
	print "</pre>" 															
	
	
	#get the newly created field
	
	try:
		statusCode, headers, response = makeAPICall('GET', url,	access_token)
	except Exception as responseException
		traceback.print_exc(file=sys.stdout)
		print responseException
		sys.exit(0)  	
	
	print "<p>Get Fields List with Newly Created Field</p>"
	
	if statusCode == 200 :  
		print "<pre>" 
		#echo stripslashes(json_encode($fieldsListResponse,JSON_PRETTY_PRINT)); 	
		print "</pre>"
	else 
		print "<p>ERROR: ",statusCode," - ",response.simpleMessage,"<br>"
		print response.detailedMessage,"</p>"
	
else if statusCode == 409 :

	print "<p>A field with ID ",new_field_id," already exists in your account, so it could not be created again.</p>" 
	
else 
	print "<p>ERROR: ",statusCode," - ",response.simpleMessage,"<br>"
	print response.detailedMessage,"</p>"

		   


