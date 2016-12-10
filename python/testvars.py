import datetime
import json

# The below dict is the valid jason required for the body in creation.
# Constraints and other info at 
# http://developer.awhere.com/api/reference/fields/create-field
test_field_json_load = json.dumps({
                                    "id": 'mynewfield' ,
                                    "name": 'My New Field',
                                    "farmId": 'F-1234-14-B',
                                    "acres": 120,
                                    "centerPoint":{
                                        "latitude": 39.8282,
                                        "longitude": -98.5795,
                                        }
                                })



forecast_start = str(datetime.date.today() + datetime.timedelta(days = -8))
forecast_end = str(datetime.date.today() + datetime.timedelta(days = -1))
