import requests as rq
import base64
import pprint
import json
import random


class AWhereAPI:
    def __init__(self, api_key, api_secret):
        self._fields_url = 'https://api.awhere.com/v2/fields'
        self.api_key = api_key
        self.api_secret = api_secret
        self.base_64_encoded_secret_key = self.encode_secret_and_key(self.api_key, self.api_secret)
        self.auth_token = self.get_oauth_token(self.base_64_encoded_secret_key)

    def create_test_field(self):
        # Each Field requires a unique ID
        # http://developer.awhere.com/api/reference/fields/create-field
        testField = 'TestField-'
        testField += str(random.randint(1,999))

        fieldBody = {'id': testField,
                    'name': testField,
                    'farmId': 'Farm1Test',
                    'centerPoint': {'latitude': 39.82,
                                    'longitude': -98.56},
                    'acres': 100}

        auth_headers = {
            "Authorization": "Bearer %s" % self.auth_token,
            "Content-Type": 'application/json'
        }
        # Make the POST request to create your Field
        response = rq.post(self._fields_url,
                        headers=auth_headers,
                        json=fieldBody)
        print('Attempting to create new field....\n')
        print('The server responded with a status code of %d' % response.status_code)
        pprint.pprint(response.json())


    def encode_secret_and_key(self, key, secret):
        #Base64 Encode the Secret and Key
        key_secret = '%s:%s' % (key, secret)
        #print('\nKey and Secret before Base64 Encoding: %s' % key_secret)

        encoded_key_secret = base64.b64encode(
            bytes(key_secret, 'utf-8')).decode('ascii')

        #print('Key and Secret after Base64 Encoding: %s' % encoded_key_secret)
        return encoded_key_secret


    def get_fields(self):

        auth_headers = {
            "Authorization": "Bearer %s" % self.auth_token,
        }

        fields_response = rq.get(self._fields_url,
                                headers=auth_headers)

        responseJSON = fields_response.json()
        print('You have %s fields registered on your account' % len(
            responseJSON["fields"]))  # Access the "Fields" key in the dictionary
        for field in responseJSON["fields"]:
            print(field["name"])
        # pprint.pprint(responseJSON)


    def get_oauth_token(self, encoded_key_secret):
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
