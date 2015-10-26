<html><head><style type="text/css">body{ font-size:0.9em; font-family:arial; } h1{ font-size:1.5em; } hr{ margin-top:20px;} pre{  border: 1px solid #ddd; white-space: pre; word-wrap: normal; max-height: 240px; overflow:auto; background:#f5f5f5; font-size:0.9em; } table{ width:100%;} th{ vertical-align:top; background:#CCC; padding:5px 0px 5px;} td{ padding:2px 1px 2px; border-bottom:1px solid #f5f5f5;}  th small{ font-weight:normal;} tr.date-row td{ background:#f5f5f5; padding:7px 0px 7px 7px; }</style></head><body> 
<?php

/* **************************************************************************
 * First let's set up some test variables. You'll need to put your API Key 
 * and Secret (also called Consumer Key and Secret) in the first two variables. 
 * If you don't have your credentials yet, follow the steps at 
 * developer.awhere.com/api/get-started
 * **************************************************************************
 */

$api_key 		= "";
$api_secret 	= "";


// These variables are used for creating data later in the sample script. 

$new_field_id 			= 'mynewfield'; 
$new_field_name 		= "My New Field"; 
$new_field_farm_id 		= 'F-1234-14-B'; 
$new_field_latitude 	= 39.8282;
$new_field_longitude 	= -98.5795; 
$new_field_acres 		= 120; 

$observed_weather_start = "2015-07-24";
$observed_weather_end = "2015-07-31"; 

$forecast_weather_start = date("Y-m-d"); 
$forecast_weather_end = date("Y-m-d",strtotime("+ 3 days")); 




/* **************************************************************************
 * These next two functions are used to streamline the actual work of 
 * making an API call in PHP. Review makeAPICall() to see how to configure
 * cURL commands in PHP. This approach is compatible with all recent versions 
 * of PHP and doesn't require any special libraries.
 * **************************************************************************
 */

// MAKE API CALL
// Configures and uses cURL to open an HTTP transaction with the URL provided 
// and then converts the response to a PHP object. 
// If there is a problem with cURL and the API call can't execute, this function
// throws an Exception
//
// Function Parameters: 
// $verb = GET | POST | PATCH | PUT | DELETE
// $url = fully qualified URL (including domain)
// $token = the previously generated OAuth Access Token (see below) 
// $responseStatusCode = a variable to return the HTTP Status Code in
// $body = a payload to send with POST, PATCH, and PUT calls 
// $headers = an array of additional headers (you don't need to include Authorization here)

function makeAPICall($verb,$url,$token,&$responseStatusCode=null,&$responseHeaders=null,$body=null,$headers=null){ 
	
	if($headers!==null){ 
		$headers[] = "Authorization: Bearer $token"; 
	} else { 
		$headers = array("Authorization: Bearer $token"); 
	} 
	
	$ch = curl_init($url);
	curl_setopt($ch, CURLOPT_CUSTOMREQUEST, $verb);
	curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
	curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
	curl_setopt($ch, CURLOPT_HEADER, true);

	if(($verb=='POST' || $verb=='PUT' || $verb=='PATCH') && !is_null($body))
		curl_setopt($ch, CURLOPT_POSTFIELDS, $body);
											
	// Normally you should not use these cURL options. They disable the SSL Cert 
	// verification. But many local development environments are not built with
	// the standard chain authority certificates, and so cannot verify the Cert.
	// If you have troubles making cURL requests, you can uncomment the next two
	// lines, but don't put them into production. 
	 curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, false);
	 curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
	
	$response = curl_exec($ch);
	if($response === false){
		throw new Exception('cURL Transport Error (HTTP request failed): '.curl_error($ch));
		$responseStatusCode = false; 
	} else { 
		$info = curl_getinfo($ch);
		$responseStatusCode = $info['http_code']; 
		list($responseHeaders, $body) = explode("\r\n\r\n", $response, 2); 
		$result = json_decode($body); 
	}	
	curl_close($ch);
	
	return $result;
} 

// GET OAUTH TOKEN 
// Uses the Token API to retrieve an access token. 
// Note: the token will expire after an hour. This API returns an 'expires_in' property
//       with the number of seconds until it expires, but that is not captured in this example. 
//       API calls with an expired token also return 401 Unauthorized HTTP error. 
// If there is a problem with cURL and the API call can't execute, this function
// throws an Exception
//
// Function Parameters: 
// $api_key = the API Key 
// $api_secret = your specific API Secret

function GetOAuthToken($api_key,$api_secret){ 
	
	$ch = curl_init("https://api.awhere.com/oauth/token");
	curl_setopt($ch, CURLOPT_CUSTOMREQUEST, "POST");
	curl_setopt($ch, CURLOPT_POSTFIELDS, "grant_type=client_credentials");
	curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
	curl_setopt($ch, CURLOPT_HTTPHEADER, array(
												"Content-Type: application/x-www-form-urlencoded",
												"Authorization: Basic ".base64_encode($api_key.":".$api_secret)
											));


	// Normally you should not use these cURL options. They disable the SSL Cert 
	// verification. But many local development environments are not built with
	// the standard chain authority certificates, and so cannot verify the Cert.
	// If you have troubles making cURL requests, you can uncomment the next two
	// lines, but don't put them into production. 
	 curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, false);
	 curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
	
	$response = curl_exec($ch);
	if($response === false){
		throw new Exception('cURL Transport Error (HTTP request failed): '.curl_error($ch));
		$responseStatusCode = false; 
	} else { 
		$info = curl_getinfo($ch);
		$responseStatusCode = $info['http_code']; 
		$result = json_decode($response); 
	}	
	curl_close($ch);
	
	return $result->access_token;
}


// Parse HTTP Headers
// This function takes the raw HTTP response headers and parses them to return
// just the ones that are relevant or wanted. 
//
// Function Parameters:
// $raw = the raw string headers
// $desired = array of header names that you want returned. 
// $returnType = 'string' | 'array'

function parseHTTPHeaders($raw,$desired,$returnType = 'string'){ 
	$listed = explode("\n",$raw); 
	$parsed = array(); 
	$return = array(); 
	foreach($listed as $line){ 
		if(substr($line,0,4)=='HTTP') continue; 
		list($key,$value) = explode(': ',$line); 
		$parsed[$key] = $value; 
	} 
	
	foreach($desired as $header){ 
		$return[$header] = (array_key_exists($header,$parsed))?$parsed[$header]:null; 
	} 
	
	if($returnType=='array'){ 
		return $return; 
	} else { 
		$output = ''; 
		foreach($return as $header=>$value){ 
			$output.=$header.': '.$value."\n"; 
		} 
		return trim($output); 
	} 
	
} 


