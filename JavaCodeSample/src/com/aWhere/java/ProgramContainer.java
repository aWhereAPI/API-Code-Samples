package com.aWhere.java;

//TODO: Hardcode lat/long, 5th option to auto generate, aggregate by 24 hours

import org.json.*;
import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.io.Reader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.nio.charset.StandardCharsets;
import java.text.DecimalFormat;
import java.util.Base64;
import java.util.Iterator;
import java.util.Map;
import java.util.Scanner;

public class ProgramContainer {

	public static final String HOST = "https://api.awhere.com";
	
	private Map<String, String> parameters;
	private HttpURLConnection connection;
	private String oAuthToken;
	private String secret;
	private String key;
	private String request;
	private boolean isAuthenticated = false;
	
	public ProgramContainer() {
	}
	
	public ProgramContainer(String sec, String key) {
		this.secret = sec;
		this.key = key;
	}
	
	public void addParameter(String x, String y) {
		parameters.put(x, y);
	}
	
	public void resetParameters() {
		parameters = null;
	}
	
	public void setRequest(String x) {
		request = x;
	}
	
	/* boolean authenticate()
	 * Connects to https://api.awhere.com/oauth/token and receives an oauth token with the secret and key.
	 * Returns true if an ouath token was successfully received
	 */	
	public boolean authenticate() {
		String auth_att = key + ":" + secret;
		Base64.Encoder encoder = Base64.getEncoder();
		String encoded = encoder.encodeToString(auth_att.getBytes(StandardCharsets.UTF_8));
		try {
			URL url = new URL(HOST+"/oauth/token");
			System.out.println("Communicating with -> " + HOST + "/oauth/token");
			connection = (HttpURLConnection) url.openConnection();
			System.out.println("Connection established");
			
			connection.setRequestMethod("POST");
			connection.setDoOutput(true);
			connection.setDoInput(true);
			connection.setRequestProperty("Authorization","Basic "+encoded);
			connection.setRequestProperty("Content-Type", "application/x-www-form-urlencoded");
			connection.connect();
			
			OutputStream out = connection.getOutputStream();
			OutputStreamWriter outw = new OutputStreamWriter(out,"UTF-8");
			outw.write("grant_type=client_credentials");
			outw.flush();
			outw.close();
			out.close();
			
			System.out.println("Response received: \""+connection.getResponseMessage() + "\", With code: "+connection.getResponseCode());

			Reader in = new BufferedReader(new InputStreamReader(connection.getInputStream(), "UTF-8"));
			JSONObject json = formatInputStream(in);
			oAuthToken = (String) json.get("access_token");

			System.out.println("-> Oauth Token Request received.\n-> OAuth Token: "+oAuthToken);
			System.out.println("========================================================");
			return isAuthenticated = true;
		} catch(Exception e) {
			System.out.println("Error creating connection to host. Incorrect URL or HTTP Protocol.");
			e.printStackTrace();
			return isAuthenticated = false;
		}
	}
	
	/* boolean getFields()
	 * Returns true if successfully connected. Uses setRequest() and getResponse() to receive connection results.
	 * This method just formats and prints the JSON response from the GET request. 
	 */
	public boolean getFields() {
		setRequest("/v2/fields");
		JSONObject json = getResponse();
		if(json == null)
			return false;
		JSONArray fields = json.getJSONArray("fields");
		Iterator<Object> i = fields.iterator();
		System.out.println("========================================================");
		while(i.hasNext()) {
			JSONObject j = (JSONObject) i.next();
			System.out.println("--------------------");
			System.out.println("Field Name:   "+j.get("name"));
			System.out.println("--------------------");
			System.out.println("	Farm ID:      "+j.get("farmId"));
			System.out.println("	Field ID:     "+j.get("id"));
			System.out.println("	Acreage:      "+j.get("acres"));
			System.out.println("	Latitude:     "+j.getJSONObject("centerPoint").get("latitude"));
			System.out.println("	Longitude:    "+j.getJSONObject("centerPoint").get("longitude"));
		}
		System.out.println("========================================================");
		return true;
	}
	
	
	/* boolean getDailyObservations()
	 * Returns true if successfully connected. Uses setRequest() and getResponse() to receive connection results.
	 * This method just formats and prints the JSON response from the GET request. 
	 */
	public boolean getDailyObservations() {
		setRequest("/v2/weather/locations/44.390948,-110.604528/observations");
		JSONObject json = getResponse();
		if(json == null)
			return false;
		JSONArray observations = json.getJSONArray("observations");
		Iterator<Object> i = observations.iterator();
		DecimalFormat temp = new DecimalFormat("#.000");
		System.out.println("========================================================");
		System.out.println("Yellowstone National Park Observations: ");
		while(i.hasNext()) {
			JSONObject j = (JSONObject) i.next();
			System.out.println("Date: "+j.get("date"));
			System.out.println("--------------------");
			System.out.println("Latitude: "+j.getJSONObject("location").get("latitude") + ",	Longitude: " + j.getJSONObject("location").get("longitude"));
			System.out.println("Temperature Max: "+temp.format(j.getJSONObject("temperatures").getDouble("max")));
			System.out.println("            Min: "+temp.format(j.getJSONObject("temperatures").getDouble("min")));
			System.out.println("Relative Humidity Max: "+temp.format(j.getJSONObject("relativeHumidity").getDouble("max")));
			System.out.println("                  Min: "+temp.format(j.getJSONObject("relativeHumidity").getDouble("min")));
			System.out.println("Average Windspeed: "+temp.format(j.getJSONObject("wind").getDouble("average"))+"m/s");
			System.out.println("--------------------");
		}
		System.out.println();
		System.out.println("========================================================");
		return true;
	}
	
	/* boolean getForecast()
	 * Returns true if successfully connected. Uses setRequest() and getResponse() to receive connection results.
	 * This method just prints formatted results from the JSON response of the forecast. This gives a good example of how easy 
	 * the API responses are to parse given the right tools - namely a JSON library in this case. 
	 */
	public boolean getForecast() {
		setRequest("/v2/weather/locations/44.390948,-110.604528/forecasts?blockSize=24");
		JSONObject json = getResponse();
		if(json == null)
			return false;
		JSONArray forecasts = json.getJSONArray("forecasts");
		Iterator<Object> iter = forecasts.iterator();
		System.out.println("========================================================");
		System.out.println("Yellowstone National Park Forecast: ");
		while(iter.hasNext()) {
			System.out.println("--------------------");
			JSONObject forecast = (JSONObject) iter.next();
			JSONArray data = forecast.getJSONArray("forecast");
			System.out.println("Forecast Date:		" + forecast.getString("date"));
			System.out.println("Forecast Location: 	Latitude: " + forecast.getJSONObject("location").get("latitude") + "	Longitude: " + forecast.getJSONObject("location").get("longitude"));
			System.out.println("		Conditions:		"+data.getJSONObject(0).getString("conditionsText"));
			System.out.println("		Wind Speed:		"+data.getJSONObject(0).getJSONObject("wind").get("average")+" m/s");
			System.out.println("		Wind Speed:		"+data.getJSONObject(0).getJSONObject("relativeHumidity").get("average"));
			System.out.println("		Precip Chance:	"+data.getJSONObject(0).getJSONObject("precipitation").get("chance"));
		}
		System.out.println("========================================================");
		return true;
	}

	/* boolean createAutoField()
	 * generates a random field, requires no user input. Returns false on exceptions or failure.
	 */
	public boolean createAutoField() {
		if(!isAuthenticated) {
			System.out.println("!#!->No OAuth Token, cannot connect.<-!#!");
			return false;
		}
		try {
			URL url = new URL(HOST+"/v2/fields");
			System.out.println("Communicating with -> " + HOST + "/v2/fields");
			connection = (HttpURLConnection) url.openConnection();
			System.out.println("Connection established");
			
			connection.setRequestMethod("POST");
			connection.setDoOutput(true);
			connection.setDoInput(true);
			connection.setRequestProperty("Authorization","Bearer "+oAuthToken);
			connection.setRequestProperty("Content-Type", "application/json");
			
			JSONObject json = new JSONObject();
			JSONObject centerPoint = new JSONObject();
			String autoId = "ID" + (int)(Math.random()*10000);
			json.put("id", "Field-" + autoId);
			json.put("name","Generated Field " + autoId);
			json.put("farmId","Farm-"+autoId);
			json.put("acres", (int)(Math.random()*100));
			centerPoint.put("latitude", (int)(Math.random()*100) + Math.random());
			centerPoint.put("longitude", (int)(Math.random()*100) + Math.random());
			json.put("centerPoint", centerPoint);
			System.out.println("JSON of request: \n"+json.toString(4));
			OutputStream out = connection.getOutputStream();
			BufferedWriter writer = new BufferedWriter(new OutputStreamWriter(out,"UTF-8"));
	
			writer.write(json.toString());
			writer.flush();
			writer.close();
			out.close();
			connection.connect();
			
			System.out.println("Response received: \""+connection.getResponseMessage() + "\", With code: "+connection.getResponseCode());
			System.out.println("========================================================");
			switch(connection.getResponseCode()) {
				case 200: System.out.println("Success"); return true;
				case 201: System.out.println("The field has been created - try listing all fields on the main menu with [3]."); return true;
				case 409: System.out.println("This field already exists with given specifications. Please try again."); return false;
				default: return false;
			}
		} catch(Exception e) {
			e.printStackTrace();
			return false;
		}
	}	
	
	/* boolean createField()
	 * Returns true if completed successfully. Sends a POST request to create a new field in the host/v2/fields path.
	 * Has duplicate code from authenticate() and getRequest() because it has a different request method and requires a JSON body.
	 */
	public boolean createField() {
		if(!isAuthenticated) {
			System.out.println("!#!->No OAuth Token, cannot connect.<-!#!");
			return false;
		}
		try {
			//Warning for not closing input - can be ignored for this demo project.
			Scanner input = new Scanner(System.in);
			URL url = new URL(HOST+"/v2/fields");
			System.out.println("Communicating with -> " + HOST + "/v2/fields");
			connection = (HttpURLConnection) url.openConnection();
			System.out.println("Connection established");
			
			connection.setRequestMethod("POST");
			connection.setDoOutput(true); 
			connection.setDoInput(true); 
			connection.setRequestProperty("Authorization","Bearer "+oAuthToken);
			connection.setRequestProperty("Content-Type", "application/json");
			
			JSONObject json = new JSONObject();
			JSONObject centerPoint = new JSONObject();
			
			System.out.println("What should the ID be for this field?: ");
			String fieldId = input.nextLine();
			System.out.println("What should the Name be for this field?: ");
			String fieldName = input.nextLine();
			System.out.println("What should the Farm ID be for this field?: ");
			String farmId = input.nextLine();
			System.out.println("What should the acreage be for this field?: ");
			String acres = input.nextLine();
			System.out.println("What should the Latitude be for this field?: ");
			String lat = input.nextLine();
			System.out.println("What should the Longitude be for this field?: ");
			String lon = input.nextLine();
			try {
				json.put("id", fieldId);
				json.put("name",fieldName);
				json.put("farmId",farmId);
				json.put("acres", acres);
				centerPoint.put("latitude", lat);
				centerPoint.put("longitude", lon);
				json.put("centerPoint", centerPoint);
			} catch(RuntimeException e) {
				e.printStackTrace(); //In case variables are passed incorrectly or null
			}
			System.out.println("JSON of request: \n"+json.toString(4));
			OutputStream out = connection.getOutputStream();
			BufferedWriter writer = new BufferedWriter(new OutputStreamWriter(out,"UTF-8"));

			writer.write(json.toString());
			writer.flush();
			writer.close();
			out.close();
			connection.connect();
			
			System.out.println("Response received: \""+connection.getResponseMessage() + "\", With code: "+connection.getResponseCode());
			System.out.println("========================================================");
			switch(connection.getResponseCode()) {
				case 200: System.out.println("Success"); return true;
				case 201: System.out.println("The field has been created - try listing all fields on the main menu with [3]."); return true;
				case 400: System.out.println("Bad request, make sure you did not use invalid special characters or leave a field blank."); return false;
				case 409: System.out.println("This field already exists with given specifications. Please try again."); return false;
				default: return false;
			}
		} catch(Exception e) {
			System.out.println(e.getStackTrace());
		}
		return true;
	}
	
	/* JSONObject getResponse()
	 * Returns a JSONObject from the inputstream of the get request sent.
	 * setRequest() sets the rest URL and this method initiates the connection if the current instance has been authenticated and a request has been set.
	 * Much of this code is used multiple times, this method exists to reduce code bloat.
	 */
	public JSONObject getResponse() {
		JSONObject response = null;
		if(!isAuthenticated) {
			System.out.println("!#!->No OAuth Token, cannot connect.<-!#!");
			return null;
		}
		if(request == "" || request == null) {
			System.out.println("No valid request, cannot connect.");
			return null;
		}
		try {
			URL url = new URL(HOST+request);
			System.out.println("Communicating with -> " + HOST + request);
			connection = (HttpURLConnection) url.openConnection();
			System.out.println("Connection established");
			
			connection.setRequestMethod("GET");
			connection.setDoOutput(true);
			connection.setDoInput(true);
			connection.setRequestProperty("Authorization","Bearer "+oAuthToken);
			connection.connect();
			Reader in = new BufferedReader(new InputStreamReader(connection.getInputStream(),"UTF-8"));
			response = formatInputStream(in);
			System.out.println("Response received: \""+connection.getResponseMessage() + "\", With code: "+connection.getResponseCode());
						
		} catch(Exception e) {
			System.out.println(e.getStackTrace());
		}
		return response;
	}

	/* JSONObject formatInputStream(Reader in)
	 * Converts a read bufferedinputstream (inputstreamreader) into a JSON Object for the getResponse() method.
	 * Exists to increase readability and reduce bloat.
	 */
	private JSONObject formatInputStream(Reader in) {
		StringBuilder sb = new StringBuilder();
		try {
			for(int c; (c=in.read()) >= 0;) {
				sb.append((char)c);
			}
		} catch (IOException e) {
			e.printStackTrace();
		}
		JSONObject json = new JSONObject(sb.toString());
		return json;		
	}
	
	/* setSecret(String key)
	 * Sets the secret to a new secret. Unused. 
	 */
	public void setSecret(String key) {
		this.secret = key;
	}
	
	/* setKey(String key)
	 * Sets the key to a new key. Unused. 
	 */
	public void setKey(String key) {
		this.key = key;
	}

}
