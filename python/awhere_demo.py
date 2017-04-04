from header import AWhereAPI
from menus import Menus

def user_has_credentials():
    global api_key
    global api_secret
    isValid = True

    if (not api_key):
        print('Error: Missing API Key. \nPlease place your API key in the main script and try again.')
        return False
    if (not api_secret):
        print('Error: Missing API Secret Key. \nPlease pace your Secret key in the main script and try again.')
        return False

    return isValid

def handle_user_input(user_input, aWhere):
    #print('\n\n')
    global quit_requested
    if user_input == 1:
        aWhere.get_fields()
    elif user_input == 2:
        aWhere.get_fields()
        field_id = menu.get_weather_field_id()
        aWhere.get_weather_by_id(field_id)
    elif user_input == 3:
        aWhere.create_test_field()
    elif user_input == 4:
        aWhere.get_fields()
        field_id = menu.get_delete_field_id()
        aWhere.delete_field_by_id(field_id)
    elif user_input == 0:
        quit_requested = True    
    else:
        print('Invalid Selection! Please try again')

quit_requested = False
menu = Menus()

# Enter your Api Key and Secret below:
api_key = '' 
api_secret = '' 

if user_has_credentials():
    aWhere = AWhereAPI(api_key, api_secret) # The AWhereAPI class
    # Main Entry Point 
    menu.display_title()

    while not quit_requested:
        menu.display_menu()
        user_input = menu.get_user_input()
        menu.clear_screen()
        handle_user_input(user_input, aWhere)
        
