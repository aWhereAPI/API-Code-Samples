import urllib2
import httplib
import requests
import base64
import string
import json

host = "http://api.awhere.com"

def GetOAuthToken(key, secret):
	conn = httplib.HTTP(host)
	conn.putrequest('POST', '/oauth/token')
	conn.putheader("Content-type", "application/x-www-form-urlencoded")
	
	auth = base64.encodestring('%s:%s' % (key, secret)).replace('\n', '')
	conn.putheader("Authorization", "Basic %s" % auth)
	conn.endheaders()
	conn.send(message)
	statuscode, statusmessage, header = conn.getreply()	
	
	res = conn.getfile().read()
	#return the access token from the json response
	return res

def MakeAPICall(verb, url, token, responseStatusCode = None, responseHeaders = None, body = None, headers = None):
	if headers == None: 
		headers = []
		
	headers[0] = 'Authorization: Bearer '.token 
	#headers[1] = 
	
	conn = httplib.HTTP(host)
	conn.putrequest(verb, url)
	conn.putrequest(verb, url)
	conn.putheader("Host", host)
	conn.putheader("User-Agent", "Python http auth")
	conn.putheader("Content-type", "text/html; charset=\"UTF-8\"")
#	conn.putheader("Content-length", "%d" % len(message))
	auth = base64.encodestring('%s:%s' % (username, password)).replace('\n', '')
	conn.putheader("Authorization", "Basic %s" % auth)
	
	conn.endheaders()
	conn.send('')
	
	statuscode, statusmessage, header = conn.getreply()
	#print "Response: ", statuscode, statusmessage
	#print "Headers: ", header
	res = conn.getfile().read()
	#return the deserialized json response
	return res
	
	
def parseHTTPHeaders(raw, desired, returnType = 'string'): 

	headers = raw.split('\n')
	parsedHeaders = []
	
	for header in headers:		
		for desiredHeader in desired:
			if desiredHeader in header:
				parsedHeaders.append(header)
				break
			
			
	if returnType == 'string':
		return ",".join(parsedHeaders)
	else 
		return parsedHeaders
		
		
	