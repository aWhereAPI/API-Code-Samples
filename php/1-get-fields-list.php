<?php
/* aWhere Code Samples
 * Copyright (C) 2015 aWhere Inc.
 * License: MIT 
 * Author: Jeof Oyster (jeofoyster@awhere.com) 
 * 
 * These code samples show a variety of different use cases and demonstrate how to 
 * make API calls in PHP. Each file shows a different use case. And each file 
 * is designed so that if you load the file to a browser and access it from a server, 
 * you will see prettified results in HTML. 
 */ 


/* CODE SAMPLE: GETTING A LIST OF FIELDS */ 



// Include Header 
// Be sure to change the variables in this header.php file, especially adding your 
// API Key and Secret or else the API calls will not run. This file uses three helper
// functions--GetOAuthToken(), makeAPICall(), and parseHTTPHeaders()--to streamline
// basic API operations. 

include("header.php"); 


// GET A TOKEN 
// First, you always need to generate a token. We built the GetOAuthToken 
// function (in header.php) to streamline that part

echo "<h1>Get Access Token</h1>"; 

try{ 	//if there is a cURL problem and the API call can't execute at all, 
		//the function throws an exception which we can catch to fail gracefully.
		
	$access_token = GetOAuthToken($api_key,$api_secret); 

} catch(Exception $e){ 
	echo $e->getMessage(); // For this script we're just echoing the error and stopping the rest of the script. 
	exit();  			   // in your code you'll want to handle the error and recover appropriately.
} 

echo "<p>Access Token = $access_token</p>"; 


// MAKE API CALL 
// Now we'll make a simple API Call to get a list of fields in our account. 
// If this is the first time you've used the APIs this list will be empty. 

echo "<hr><h1>Get List of Fields</h1>"; 

try{ 

$fieldsListResponse = makeAPICall('GET', 								//verb
								  'https://api.awhere.com/v2/fields',	//URL
								  $access_token,						//Access Token
								  $fieldsListStatusCode,				//Status Code (returned from function)
								  $fieldsListResponseHeaders			//We want to capture the response HTTP headers
								 ); 
} catch(Exception $e){ 
	echo $e->getMessage(); 
	exit();  			   
} 


if($fieldsListStatusCode==200){  	// Code 200 means the request was successful
	
	echo '<p>There are '.count($fieldsListResponse->fields)." field locations in the current page of results.</p>"; 
	echo '<p>Request:</p><pre>GET https://api.awhere.com/v2/fields</pre>'; 
	echo '<p>Content-Range Header (shows pagination and total results):</p>';
	
	// HTTP transactions return a lot of headers, but in this example we only want the Content-Range header
	// (the parseHTTPHeaders function returns just the headers you want)
	// This API returns a ranged result, which are paginated by default to 50 results per page. The 
	// Content-Range header shows which of the results are on this page (e.g., 1-10) and the total number
	// of results. It looks something like this: 
	// Content-Range: fields 0-5/5
	echo "<pre>".parseHTTPHeaders($fieldsListResponseHeaders,array('Content-Range'))."</pre>"; 
	
	echo '<p>Response Body:</p>';
	echo '<pre>'; 
	echo stripslashes(json_encode($fieldsListResponse,JSON_PRETTY_PRINT)); 	//Note: Stripslashes() is used just for prettier 
	echo '</pre>'; 															//output in the browser. Not needed normally.
	
	
} else { 							// If there is any other response code, there was a problem.
									// this code shows how to extract the two different error messages
									// You should not use the error messages themselves to drive behavior
									// (don't test them in if() or switch() statements)
									// use the status code for that. See developer.awhere.com/api/conventions 
									
	echo "<p>ERROR: ".$fieldsListStatusCode." - ".$fieldsListResponse->simpleMessage."<br>"; 
	echo $fieldsListResponse->detailedMessage."</p>"; 
	
}

