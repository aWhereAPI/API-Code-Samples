# Getting Started with the Demo Application
To get started, you'll need to have Visual Studio 2012+ installed on your machine.

1. Inside this directory, click on the Visual Studio Solution `aWhere.Api.CodeSample.sln` to open VS.
2. You'll notice that there are 3 Projects:
```
aWhere.Api.Business
aWhere.Api.ConsoleDemo
aWhere.Api.Services
```
The `aWhere.Api.ConsoleDemo` is the main C# Console Application project.
Make sure that this Project is marked as your Startup Project. To do so, right-click on the `aWhere.Api.ConsoleDemo` project and select "Set as Startup Project".

3. Enter your API Key and Secret in the `Program.cs` class file, located in the `ConsoleDemo` Project. Save the file.
4. Click on `Build` then `Build Solution`. This will fetch the project dependencies from NuGet.
5. Run the App.


# Using the Application
You will be able to launch the application once you have completed the above steps. The application allows you to test out some of the features of the aWhere API. We encourage you to further research the API by visiting [the API Reference Guide](http://developer.awhere.com/api/reference).

### Obtaining a OAuth Token
The app first showcases obtaining an OAuth token. This is displayed to you once you start the application. You'll see the following:

```
First, let's check for your API Key and Secret.....
Found your API Key and Secret! Now, let's obtain an access token.

...........

OAuth Token Obtained! Your token is [YOURTOKENAPPEARSHERE] and will expire after 1 hour.

Press any key to continue to the main menu
```

* For more details, please see http://developer.awhere.com/api/authentication

### Performing Requests
Next, you'll be presented with a list of options you can perform. Simply press the number of the option you'd like to demo.

More information regarding each request and API can be found on the API Documentation: http://developer.awhere.com/api/reference
