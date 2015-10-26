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



// MAKE API CALLS
// In this example, we'll combine the data from Observed Weather with data from the Weather Norms to show
// the difference between this year's weather and the long-term averages for the same days.


echo "<hr><h1>Compare Recent Weather Observations to Long-Term Normals</h1>"; 
	
//We're using the dates in $observed_weather_start and $observed_weather_end to configure
//the long-term-norms (LTN) requests. The LTN API uses the month-and-day combination first
//to get the averages for each individual day) and then specifies any range of years 
//to calculate the averages. This example will get both the three and ten year averages 
//using dates/years relative to the variables above. 
//Note this example uses variables set in header.php

$month_day_start = date("m-d",strtotime($observed_weather_start)); 
$month_day_end   = date("m-d",strtotime($observed_weather_end)); 

$three_year_start_year = date("Y",strtotime("-4 years",strtotime($observed_weather_start))); 
$ten_year_start_year = date("Y",strtotime("-11 years",strtotime($observed_weather_start))); 
$end_year   = date("Y",strtotime("-1 year",strtotime($observed_weather_end))); 


//We'll set up all three API URLs here. 
//Note that the sort order of responses is not guaranteed, and could change in the future.
//In this example we want to compare dates and days, so we'll force the API to sort the 
//results based on calendar order, so that we know each object in the range matches the 
//correct day and date. 

		
$observedWeatherURL = 'https://api.awhere.com/v2/weather/fields/'
						.$new_field_id
						.'/observations/'
						.$observed_weather_start.','.$observed_weather_end
						."?sort=-date"		// notice we're sorting by date-descending
						."&units=usa";      // and we're requesting data in units standard to the USA 
											// (e.g., Fahrenheit & inches)
	
$threeYearNormsURL = 'https://api.awhere.com/v2/weather/fields/'
						.$new_field_id
						.'/norms/'
						.$month_day_start.','.$month_day_end
						."/years/"
						.$three_year_start_year.','.$end_year
						."?sort=-day"		// sort
						."&units=usa"; 		// USA-units

$tenYearNormsURL = 'https://api.awhere.com/v2/weather/fields/'
						.$new_field_id
						.'/norms/'
						.$month_day_start.','.$month_day_end
						."/years/"
						.$ten_year_start_year.','.$end_year
						."?sort=-day"
						."&units=usa"; 

// Next we'll run all three API calls sequentially.						
					
try{ 

$observedWeatherResponse = makeAPICall('GET', 							//verb
									$observedWeatherURL,				//URL
									$access_token,						//Access Token
									$observedWeatherStatusCode,			//Status Code (returned from function)
									$observedWeatherResponseHeaders		//We want to capture the response HTTP headers
									); 
									
$threeYearNormsResponse  = makeAPICall('GET', 								 
									$threeYearNormsURL,	
									$access_token,						
									$threeYearNormsStatusCode,
									$threeYearNormsResponseHeaders); 
$tenYearNormsResponse    = makeAPICall('GET', 								 
									$tenYearNormsURL,	
									$access_token,						
									$tenYearNormsStatusCode,
									$tenYearNormsResponseHeaders); 
} catch(Exception $e){ 
	echo $e->getMessage(); 
	exit();  			   
} 

if($observedWeatherStatusCode==200 && $threeYearNormsStatusCode==200 && $tenYearNormsStatusCode==200){
	
	echo '<table>';
	echo "<tr><th>Weather Attribute</th><th>Current Actual</th><th>3-Year Norm<br><small>$three_year_start_year&ndash;$end_year</small></th><th>10-Year Norm<br><small>$ten_year_start_year&ndash;$end_year</small></th></tr>";
	 
	foreach($observedWeatherResponse->observations as $index=>$data){ 
		echo '<tr class="date-row"><td colspan="4">Comparing <b>'
			.date("n-j-Y",strtotime($data->date)).'</b> to the averages of <b>'
			.$threeYearNormsResponse->norms[$index]->day.'-'.$three_year_start_year.' through '
			.$threeYearNormsResponse->norms[$index]->day.'-'.$end_year.'</b> and <b>'
			.$tenYearNormsResponse->norms[$index]->day.'-'.$ten_year_start_year.' through '
			.$tenYearNormsResponse->norms[$index]->day.'-'.$end_year.'</b>'
			.'</td></tr>'; 
		echo '<tr>'; 
		echo '<td>Max Temperature</td>';
		echo '<td>'.round($data->temperatures->max,1).'&deg;'.$data->temperatures->units.'</td>'; 
		echo '<td>'.round($threeYearNormsResponse->norms[$index]->maxTemp->average,1).'&deg;'.$threeYearNormsResponse->norms[$index]->maxTemp->units.'</td>'; 
		echo '<td>'.round($tenYearNormsResponse->norms[$index]->maxTemp->average,1).'&deg;'.$tenYearNormsResponse->norms[$index]->maxTemp->units.'</td>'; 
		echo '</tr><tr>';
		echo '<td>Min Temperature</td>';
		echo '<td>'.round($data->temperatures->min,1).'&deg;'.$data->temperatures->units.'</td>'; 
		echo '<td>'.round($threeYearNormsResponse->norms[$index]->minTemp->average,1).'&deg;'.$threeYearNormsResponse->norms[$index]->minTemp->units.'</td>'; 
		echo '<td>'.round($tenYearNormsResponse->norms[$index]->minTemp->average,1).'&deg;'.$tenYearNormsResponse->norms[$index]->minTemp->units.'</td>'; 
		echo '</tr><tr>';
		echo '<td>Precipitation</td>';
		echo '<td>'.round($data->precipitation->amount,1).' '.$data->precipitation->units.'</td>'; 
		echo '<td>'.round($threeYearNormsResponse->norms[$index]->precipitation->average,1).' '.$threeYearNormsResponse->norms[$index]->precipitation->units.'</td>'; 
		echo '<td>'.round($tenYearNormsResponse->norms[$index]->precipitation->average,1).' '.$tenYearNormsResponse->norms[$index]->precipitation->units.'</td>'; 
		echo '</tr><tr>';
		echo '<td>Max Humidity</td>';
		echo '<td>'.round($data->relativeHumidity->max).'%</td>'; 
		echo '<td>'.round($threeYearNormsResponse->norms[$index]->maxHumidity->average).'%</td>'; 
		echo '<td>'.round($tenYearNormsResponse->norms[$index]->maxHumidity->average).'%</td>'; 
		echo '</tr><tr>';
		echo '<td>Min Humidity</td>';
		echo '<td>'.round($data->relativeHumidity->min).'%</td>'; 
		echo '<td>'.round($threeYearNormsResponse->norms[$index]->minHumidity->average).'%</td>'; 
		echo '<td>'.round($tenYearNormsResponse->norms[$index]->minHumidity->average).'%</td>'; 
		echo '</tr>';
	}
	echo '</table>'; 
	
	echo '<p>Response Body for Observations:</p>';
	echo '<pre>'; 
	echo stripslashes(json_encode($observedWeatherResponse,JSON_PRETTY_PRINT)); //Note: Stripslashes() is used just for prettier 
	echo '</pre>'; 																//output in the browser. Not needed normally.
	
	echo '<p>Response Body for Three-Year-Norms:</p>';
	echo '<pre>'; 
	echo stripslashes(json_encode($threeYearNormsResponse,JSON_PRETTY_PRINT)); 
	echo '</pre>'; 																
	
	echo '<p>Response Body for Ten-Year-Norms:</p>';
	echo '<pre>'; 
	echo stripslashes(json_encode($tenYearNormsResponse,JSON_PRETTY_PRINT)); 
	echo '</pre>'; 	
	
} else { 

	if($observedWeatherStatusCode!=200){ 
		echo "<p>Observed Weather ERROR: ".$observedWeatherStatusCode." - ".$observedWeatherResponse->simpleMessage."<br>"; 
		echo $observedWeatherResponse->detailedMessage."</p>"; 
	} 
	
	if($threeYearNormsStatusCode!=200){ 
		echo "<p>Three Year Norms ERROR: ".$threeYearNormsStatusCode." - ".$threeYearNormsResponse->simpleMessage."<br>"; 
		echo $threeYearNormsResponse->detailedMessage."</p>"; 
	} 
	
	if($tenYearNormsStatusCode!=200){ 
		echo "<p>Ten Year Norms ERROR: ".$tenYearNormsStatusCode." - ".$tenYearNormsResponse->simpleMessage."<br>"; 
		echo $tenYearNormsResponse->detailedMessage."</p>"; 
	} 
	
} 

