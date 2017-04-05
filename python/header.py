from __future__ import print_function
from __future__ import unicode_literals
from __future__ import division
from __future__ import absolute_import
from builtins import str
from builtins import bytes
from future import standard_library
standard_library.install_aliases()
from builtins import object
import requests as rq
import base64
import pprint
import json
import random
from menus import Menus


class AWhereAPI(object):
    def __init__(self, api_key, api_secret):
        """
        Initializes the AWhereAPI class, which is used to perform HTTP requests 
        to the aWhere V2 API.

        Docs:
            http://developer.awhere.com/api/reference
        """
        self._fields_url = 'https://api.awhere.com/v2/fields'
        self._weather_url = 'https://api.awhere.com/v2/weather/fields'
        self.api_key = api_key
        self.api_secret = api_secret
        self.base_64_encoded_secret_key = self.encode_secret_and_key(
            self.api_key, self.api_secret)
        self.auth_token = self.get_oauth_token(self.base_64_encoded_secret_key)
        self._menu = Menus()

    def create_test_field(self):
        """
        Performs a HTTP POST request to create and add a Field to your aWhere App.AWhereAPI

        Docs: 
            http://developer.awhere.com/api/reference/fields/create-field
        """
        # Each Field requires a unique ID
        testField = 'TestField-'
        testField += str(random.randint(1, 999))

        # Next, we build the request body. Please refer to the docs above for
        # more info.
        fieldBody = {'id': testField,
                     'name': testField,
                     'farmId': 'Farm1Test',
                     'centerPoint': {'latitude': 39.82,
                                     'longitude': -98.56},
                     'acres': 100}

        # Setup the HTTP request headers
        auth_headers = {
            "Authorization": "Bearer %s" % self.auth_token,
            "Content-Type": 'application/json'
        }

        # Perform the POST request to create your Field
        print('Attempting to create new field....\n')
        response = rq.post(self._fields_url,
                           headers=auth_headers,
                           json=fieldBody)
        # A successful request will return a 201 status code
        print('The server responded with a status code of %d \n' %
              response.status_code)
        pprint.pprint(response.json())
        print('\n\n\n')
        if response.status_code == 201:
            print(
                'Your field "{0}" was successfully created!'.format(testField))
        else:
            print('An error occurred. Please review the above resonse and try again.')

    def delete_field_by_id(self, field_id):
        """
        Performs a HTTP DELETE request to delete a Field from your aWhere App.
        Docs: http://developer.awhere.com/api/reference/fields/delete-field
        Args: 
            field_id: The field to be deleted
        """
        # Setup the HTTP request headers
        auth_headers = {
            "Authorization": "Bearer %s" % self.auth_token,
            "Content-Type": 'application/json'
        }
        # Perform the POST request to Delete your Field
        response = rq.delete(self._fields_url + '/{0}'.format(field_id),
                             headers=auth_headers)
        print('The server responded with a status code of %d' %
              response.status_code)

    def encode_secret_and_key(self, key, secret):
        """
        Docs:
            http://developer.awhere.com/api/authentication
        Returns:
            Returns the base64-encoded {key}:{secret} combination, seperated by a colon.
        """
        # Base64 Encode the Secret and Key
        key_secret = '%s:%s' % (key, secret)
        #print('\nKey and Secret before Base64 Encoding: %s' % key_secret)

        encoded_key_secret = base64.b64encode(
            bytes(key_secret, 'utf-8')).decode('ascii')

        #print('Key and Secret after Base64 Encoding: %s' % encoded_key_secret)
        return encoded_key_secret

    def get_fields(self):
        """
        Performs a HTTP GET request to obtain all Fields you've created on your aWhere App.
        
        Docs: 
            http://developer.awhere.com/api/reference/fields/get-fields
        """
        # Setup the HTTP request headers
        auth_headers = {
            "Authorization": "Bearer %s" % self.auth_token,
        }

        # Perform the HTTP request to obtain a list of all Fields
        fields_response = rq.get(self._fields_url,
                                 headers=auth_headers)

        responseJSON = fields_response.json()

        # Display the count of Fields the user has on their account
        print('You have %s fields registered on your account' %
              len(responseJSON["fields"]))

        # Iterate over the fields and display their name and ID
        print('{0}  {1} \t\t {2}'.format('#', 'Field Name', 'Field ID'))
        print('-------------------------------------------')
        count = 0
        for field in responseJSON["fields"]:
            count += 1
            print('{0}. {1} \t {2}\r'.format(
                count, field["name"], field["id"]))

    def get_weather_by_id(self, field_id):
        """
        Performs a HTTP GET request to obtain Forecast, Historical Norms and Forecasts
        
        Docs: 
            1. Forecast: http://developer.awhere.com/api/forecast-weather-api 
            2. Historical Norms: http://developer.awhere.com/api/reference/weather/norms
            3. Observations: http://developer.awhere.com/api/reference/weather/observations
        """
        # Setup the HTTP request headers
        auth_headers = {
            "Authorization": "Bearer %s" % self.auth_token,
        }

        # Perform the HTTP request to obtain the Forecast for the Field
        response = rq.get(self._weather_url + '/{0}/forecasts?blockSize=24'.format(field_id),
                          headers=auth_headers)
        pprint.pprint(response.json())
        print('\nThe above response from the Forecast API endpoint shows the forecast for your field location ({0}).'.format(field_id))
        self._menu.os_pause()

        # Next, let's obtain the historic norms for a Field
        response = rq.get(self._weather_url + '/{0}/norms/04-04'.format(field_id),
                          headers=auth_headers)
        pprint.pprint(response.json())
        print('\nThe above response from the Norms API endpoint shows the averages of the last 10 for an arbitrary date, April 4th.')
        self._menu.os_pause()
        
        # Finally, display the observed weather. Returns the last 7 days of data for the provided Field.
        response = rq.get(self._weather_url + '/{0}/observations'.format(field_id),
                          headers=auth_headers)
        pprint.pprint(response.json())
        print('\nThe above response from the Observed Weather API endpoint shows the last 7 days of data for the provided field ({0})'.format(field_id))

    def get_oauth_token(self, encoded_key_secret):
        """
        Demonstrates how to make a HTTP POST request to obtain an OAuth Token
        
        Docs: 
            http://developer.awhere.com/api/authentication
        
        Returns: 
            The access token provided by the aWhere API
        """
        auth_url = 'https://api.awhere.com/oauth/token'

        auth_headers = {
            "Authorization": "Basic %s" % encoded_key_secret,
            'Content-Type': 'application/x-www-form-urlencoded'
        }

        body = "grant_type=client_credentials"

        response = rq.post(auth_url,
                           headers=auth_headers,
                           data=body)

        # .json method is a requests lib method that decodes the response
        return response.json()['access_token']
