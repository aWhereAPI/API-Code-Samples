import os
import platform

class Menus:
    def __init__(self, clear = 'clear', readLine = 'read'):
        # Set the clear and pause command based on the running OS 
        self.clear = clear
        self.readLine = readLine 

        operatingSystem = platform.system()

        if operatingSystem == 'Windows':
            self.clear = 'cls'
            self.readLine = 'pause'

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
        print('Please select one of the following options and then press "Enter"')
        print('\t 1. [GET] Get all Fields associated with your account')
        print('\t 2. [GET] Get Weather for an existing field')
        print('\t 3. [POST] Create a test field')
        print('\t 4. [DELETE] Delete a field')
        print('\t 0. Quit ')

    def get_user_input(self):
        user_choice = input()
        return int(user_choice)

