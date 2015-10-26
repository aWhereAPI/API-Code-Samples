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


/* CODE SAMPLE: GETTING WEATHER OBSERVATIONS FOR A FIELD LOCATION */ 



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
// The next example gets the recent weather observations for the field location that you just created.
// Note we're creating the URL first, using the variables from the header.php file.

echo "<hr><h1>Get Recent Weather Observations</h1>"; 
			
$observedWeatherURL = 'https://api.awhere.com/v2/weather/fields/'
						.$new_field_id
						.'/observations/'
						.$observed_weather_start.','.$observed_weather_end; 

try{ 

$observedWeatherResponse = makeAPICall('GET', 						//verb			 
									$observedWeatherURL,			//URL
									$access_token,					//Access Token
									$observedWeatherStatusCode,		//Status Code (returned from function)
									$observedWeatherResponseHeaders //We want to capture the response HTTP headers
									); 
} catch(Exception $e){ 
	echo $e->getMessage(); 
	exit();  			   
} 



if($observedWeatherStatusCode==200){  	// Code 200 means the request was successful
	
	echo '<p>You requested '.count($observedWeatherResponse->observations)." days of historical "
			."observed weather. The weather on "
			.date("F j, Y",strtotime($observedWeatherResponse->observations[0]->date))
			." was a high temperature of "
			.$observedWeatherResponse->observations[0]->temperatures->max."&deg;"
			.$observedWeatherResponse->observations[0]->temperatures->units
			." and a low of "
			.$observedWeatherResponse->observations[0]->temperatures->min."&deg;"
			.$observedWeatherResponse->observations[0]->temperatures->units
			."</p>"; 
			
	echo '<p>Request:</p><pre>GET '.$observedWeatherURL.'</pre>'; 
	echo '<p>Content-Range Header:</p>';
	
	// HTTP transactions return a lot of headers, but in this example we only want the Content-Range header
	// (the parseHTTPHeaders function returns just the headers you want)
	// This API returns a ranged result, which are paginated by default to 50 results per page. The 
	// Content-Range header shows which of the results are on this page (e.g., 1-10) and the total number
	// of results. It looks something like this: 
	// Content-Range: observations 0-5/5
	echo "<pre>".parseHTTPHeaders($observedWeatherResponseHeaders,array('Content-Range'))."</pre>"; 
	echo '<p>Response Body:</p>';
	echo '<pre>'; 
	echo stripslashes(json_encode($observedWeatherResponse,JSON_PRETTY_PRINT)); //Note: Stripslashes() is used just for prettier 
	echo '</pre>'; 																//output in the browser. Not needed normally.
	
	
} else { 							// If there is any other response code, there was a problem.
									// this code shows how to extract the two different error messages
									// You should not use the error messages themselves to drive behavior
									// (don't test them in if() or switch() statements)
									// use the status code for that. See developer.awhere.com/api/conventions 
									
	echo "<p>ERROR: ".$observedWeatherStatusCode." - ".$observedWeatherResponse->simpleMessage."<br>"; 
	echo $observedWeatherResponse->detailedMessage."</p>"; 
	
}
