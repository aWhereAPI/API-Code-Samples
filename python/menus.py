from __future__ import print_function
from __future__ import unicode_literals
from __future__ import division
from __future__ import absolute_import
from builtins import int
from builtins import input
from future import standard_library
standard_library.install_aliases()
from builtins import object
import os
import platform


class Menus(object):
    def __init__(self, clear='clear', readLine='read'):
        # Set the clear and pause command based on the running OS
        self.clear = clear
        self.readLine = readLine

        operatingSystem = platform.system()

        if operatingSystem == 'Windows':
            self.clear = 'cls'
            self.readLine = 'pause'

    def clear_screen(self):
        os.system(self.clear)

    def get_delete_field_id(self):
        print('Please type in the ID of the field you wish to delete.')
        user_choice = input()
        if not user_choice:
            print('Input cannot be empty/null. Please try again.')
            self.get_delete_field_id()
        return user_choice

    def get_weather_field_id(self):
        print('Please type in the ID of the field you wish to view.')
        user_choice = input()
        if not user_choice:
            print('Input cannot be empty/null. Please try again.')
            self.get_delete_field_id()
        return user_choice

    def display_title(self):
        # Clear the terminal screen and displays a title bar
        os.system(self.clear)
        print('\t\t**********************************************')
        print('\t\t****  aWhere API Python Demo Application  ****')
        print('\t\t**********************************************')
        print('\n')
        print('This python demo is intended to display basic examples of how you\r')
        print('can interact with the aWhere API Platform. \n')

        print('The application was written in Python 3 but it should be backwards \r')
        print('compatible with Python 2. It uses the "requests" library to perform HTTP requests.\n')

        print('To continue, you\'ll need to have already created a Developer \r')
        print('profile via our Developer Community.')

        print('\n\t Register here ---> https://developer.awhere.com/api/get-started\n')

        print('Next, you\'ll need to login to your profile and create an app.\r')
        print('Upon creation of your app you\'ll be provided two keys, a secret key and an API key.\n\n')
        print('Be sure to enter your keys in the awhere_demo.py file!')

        os.system(self.readLine)
        os.system(self.clear)

    def display_menu(self):
        print('\n')
        print('Please input one of the following options and then press "Enter"\n')
        print('\t 1. [GET] Get all fields associated with your account')
        print('\t 2. [GET] Get Weather (Forecast, Norms and Observations) for an existing field')
        print('\t 3. [POST] Create a test field')
        print('\t 4. [DELETE] Delete a field')
        print('\t 0. Quit ')

    def get_user_input(self):
        user_choice = input()
        return int(user_choice)

    def os_pause(self):
        os.system(self.readLine)
