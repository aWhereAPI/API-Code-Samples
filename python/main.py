
import header
#import mscvrt


api_key = "wwJN2yTlGEVzB2SAfnN7A5215B2Eshjj'"
api_secret = "ksyXJbpbzRbGDObw"
host = "http://api.awhere.com"
auth_token = header.GetOAuthToken(api_key, api_secret)


#These variables are used for creating data later in the sample script. 
new_field_id             = 'mynewfield' 
new_field_name           = "My New Field" 
new_field_farm_id        = 'F-1234-14-B' 
new_field_latitude       = 39.8282
new_field_longitude      = -98.5795 
new_field_acres          = 120 

observed_weather_start = "2015-07-24"
observed_weather_end = "2015-07-31" 

forecast_weather_start = date("Y-m-d") 
forecast_weather_end = date("Y-m-d",strtotime("+ 3 days")) 

while True:
    print 'Select a request option'
    print '--'
    print '1. Get All Fields'
    print '2. Create A Field'
    print '3. Get Weather Observations'
    print '4. Get Weather Forecasts'
    print '5. Get Three Year Weather Norms'
    print '6. Get Ten Year Weather Norms'
    print 'q - Quit'

    switch = input()
    
    if value == 'q':
        break;