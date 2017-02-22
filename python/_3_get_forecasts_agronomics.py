"""This script gets field agronomic and weather data.

It uses
- functions defined in _0_header.py
- field data retrieved by the call in _1_get_fields_list
"""

import json
import datetime

from _0_header import MakeAPICall, auth_token
from _1_get_fields_list import get_fields
from testvars import forecast_start, forecast_end

field_data_urls = get_fields.json()['fields'][0]['_links']

try:
    print '<h2> you have access to...'
    print field_data_urls
    print '</h2>'

except Exception as fields_except:
    print 'Not much, since get fields failed or is undefined,'+\
          'create a field then get fields first.\n--\n'
    print 'here is the error \n'
    print fields_except

print "<hr>"
print "<hr>"

try:
    agronms = MakeAPICall('GET',
        url = 'https://api.awhere.com'+\
                field_data_urls['awhere:agronomics']['href']+\
                '/' + forecast_start + ',' + forecast_end,
        token = auth_token)
    print agronms.url
    print agronms.content

except Exception as agronms_except:
    print 'get for agronimic data failed with the following \n--\n'
    print agronms_except


try:
    forecasts = MakeAPICall('GET',
        url ='https://api.awhere.com'+\
                field_data_urls['awhere:forecasts']['href'],
        token = auth_token)

    print forecasts.url
    print forecasts.content

except Exception as forecasts_except:
    print 'get for forecast data failed with the following \n--\n'
    print forecasts_except




''' Other fields for more advanced data sets
{
    u'awhere:agronomics': 
        {u'href': u'/v2/agronomics/fields/mynewfield/agronomicvalues'},
      
    u'awhere:forecasts': 
        {u'href': u'/v2/weather/fields/mynewfield/forecasts'},

    u'awhere:observations': 
        {u'href': u'/v2/weather/fields/mynewfield/observations'},

    u'awhere:plantings': 
        {u'href': u'/v2/agronomics/fields/mynewfield/plantings'},

    u'curies': 
        [
            {u'href': u'http://awhere.com/rels/{rel}',
            u'name': u'awhere',
            u'templated': True}
        ],
    u'self': 
        {u'href': u'/v2/fields/mynewfield'}
}
'''
