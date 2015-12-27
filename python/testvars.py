import datetime
from datetime import timedelta

#Test variables

#Key, secret
api_key = ""
api_secret = ""

#These variables are used for creating data later in the sample script. 
new_field_id 			= 'mynewfield' 
new_field_name 			= "My New Field" 
new_field_farm_id 		= 'F-1234-14-B' 
new_field_latitude 		= 39.8282
new_field_longitude 	        = -98.5795 
new_field_acres 		= 120 

observed_weather_start          = "2015-07-24"
observed_weather_end            = "2015-07-31" 

forecast_start                  = datetime.date.today()
forecast_end                    = forecast_start + timedelta(days = 3)

forecast_weather_start          = forecast_start.strftime('%Y-%m-%d') 
forecast_weather_end            = forecast_end.strftime('%Y-%m-%d')
