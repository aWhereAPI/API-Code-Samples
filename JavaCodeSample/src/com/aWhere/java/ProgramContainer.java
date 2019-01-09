package com.aWhere.java;

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
	
	URL url;
	
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
	
	/* Connects to https://api.awhere.com/oauth/token and receives an oauth token with the secret and key.
	 * 1: Authentication passed
	 * 0: Exception was caught, authentication failed
	 */	
	public boolean authenticate() {
		String auth_att = key + ":" + secret;
		Base64.Encoder encoder = Base64.getEncoder();
		String encoded = encoder.encodeToString(auth_att.getBytes(StandardCharsets.UTF_8));
		try {
			url = new URL(HOST+"/oauth/token"); //The URL to post for an oauth token
			System.out.println("Communicating with -> " + HOST + "/oauth/token");
			connection = (HttpURLConnection) url.openConnection();
			System.out.println("Connection established");
			
			connection.setRequestMethod("POST");
			connection.setDoOutput(true); //can write to stream
			connection.setDoInput(true); //can receive reply stream
			connection.setRequestProperty("Authorization","Basic "+encoded); //encode our secret and key
			connection.setRequestProperty("Content-Type", "application/x-www-form-urlencoded"); //set content type
			connection.connect();
			
			OutputStream out = connection.getOutputStream();
			OutputStreamWriter outw = new OutputStreamWriter(out,"UTF-8");
			outw.write("grant_type=client_credentials");
			outw.flush();
			outw.close();
			out.close();
			
			System.out.println("Response received: \""+connection.getResponseMessage() + "\", With code: "+connection.getResponseCode());
			
			//Build a bufferedreader to interpret the bufferedinputstream
			Reader in = new BufferedReader(new InputStreamReader(connection.getInputStream(), "UTF-8"));
			JSONObject json = formatInputStream(in);
			oAuthToken = (String) json.get("access_token");

			System.out.println("-> Oauth Token Request received.\n-> OAuth Token: "+oAuthToken);
			System.out.println("========================================================");
			return isAuthenticated = true;
			//Catch all exceptions, not a good practice but the scope of possible exceptions is very limited.
			} catch(Exception e) {
			System.out.println("Error creating connection to host. Incorrect URL or HTTP Protocol.");
			e.printStackTrace();
			return isAuthenticated = false;
		}
	}
	
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
	
	public boolean getDailyObservations() {
		setRequest("/v2/weather/fields/field1/observations");
		JSONObject json = getResponse();
		if(json == null)
			return false;
		JSONArray observations = json.getJSONArray("observations");
		Iterator<Object> i = observations.iterator();
		DecimalFormat temp = new DecimalFormat("#.000");
		System.out.println("========================================================");
		while(i.hasNext()) {
			JSONObject j = (JSONObject) i.next();
			System.out.println("Date: "+j.get("date"));
			System.out.println("--------------------");
			System.out.println("Field: ID "+j.getJSONObject("location").get("fieldId"));
			System.out.println("            Latitude: "+j.getJSONObject("location").get("latitude"));
			System.out.println("            Longitude: "+j.getJSONObject("location").get("longitude"));
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
	
	public boolean getForecast() {
		setRequest("/v2/weather/fields/field1/forecasts");
		JSONObject json = getResponse();
		if(json == null)
			return false;
		Scanner input = new Scanner(System.in);
		JSONArray forecasts = json.getJSONArray("forecasts");
		int in = 1;
		int num = 0;
		System.out.println("How many hours per day to forecast? (Starting from 8AM - Default 8AM - 5PM)");
		num = input.nextInt();
		int index = 0;
		while(in != 0 && index <= 8) {
			System.out.println("--------------------");
			JSONObject forecast = forecasts.getJSONObject(index);
			JSONArray data = forecast.getJSONArray("forecast");
			System.out.println("Forecast Date:		" + forecast.getString("date"));
			System.out.println("Forecast Field: 	" + forecast.getJSONObject("location").getString("fieldId"));
			for(int i = 8; i <= 8+num; i++) {
				if(i == data.length())
					break;
				System.out.println("	--(Hour: 	"+data.getJSONObject(i).getString("startTime").substring(11, 16)+")--");
				System.out.println("		Conditions:		"+data.getJSONObject(i).getString("conditionsText"));
				System.out.println("		Wind Speed:		"+data.getJSONObject(i).getJSONObject("wind").get("average")+" m/s");
				System.out.println("		Wind Speed:		"+data.getJSONObject(i).getJSONObject("relativeHumidity").get("average"));
				System.out.println("		Precip Chance:	"+data.getJSONObject(i).getJSONObject("precipitation").get("chance"));
				System.out.println("		Temperature:	"+data.getJSONObject(i).getJSONObject("temperatures").getFloat("value")+" C");
			}
			if(index  >= 8)
				break;
			System.out.println("--- Print Next Day Forecast? (1 Yes | 0 No ) ---");
			in = input.nextInt();
			index++;
		}
		//Iterator<Object> i = forecast.iterator();
		/*while(i.hasNext()) {
			JSONObject current = (JSONObject) i.next();
			System.out.println("--------------------");
			System.out.println("From "+current.get("startTime") + "	To " + current.get("endTime"));
			System.out.println("Current Temperature: "+current.getJSONObject("temperatures").get("value"));
			System.out.println("		Max: "+current.getJSONObject("temperatures").get("max"));
			System.out.println("		Min: "+current.getJSONObject("temperatures").get("min"));
			System.out.println("--------------------");
		}*/
		return true;
	}

	public boolean createField() {
		if(!isAuthenticated) {
			System.out.println("!#!->No OAuth Token, cannot connect.<-!#!");
			return false;
		}
		try {
			Scanner input = new Scanner(System.in);
			url = new URL(HOST+"/v2/fields"); //The URL to post for an oauth token
			System.out.println("Communicating with -> " + HOST + "/v2/fields");
			connection = (HttpURLConnection) url.openConnection();
			System.out.println("Connection established");
			
			connection.setRequestMethod("POST");
			connection.setDoOutput(true); //can write to stream
			connection.setDoInput(true); //can receive reply stream
			connection.setRequestProperty("Authorization","Bearer "+oAuthToken); //encode our secret and key
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
			}catch(RuntimeException e){
				e.printStackTrace();
			}
			System.out.println("Created JSON: \n"+json.toString(4));
			OutputStream out = connection.getOutputStream();
			BufferedWriter writer = new BufferedWriter(new OutputStreamWriter(out,"UTF-8"));
			//OutputStreamWriter outw = new OutputStreamWriter(out, "UTF-8");
			writer.write(json.toString());
			writer.flush();
			writer.close();
			out.close();
			connection.connect();
			
			System.out.println("Response received: \""+connection.getResponseMessage() + "\", With code: "+connection.getResponseCode());
			System.out.println("========================================================");
			if(connection.getResponseCode() != 200)
				return false;
			
		}catch(Exception e) {
			System.out.println(e.getStackTrace());
		}
		return true;
	}
	
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
			url = new URL(HOST+request); //The URL to post for an oauth token
			System.out.println("Communicating with -> " + HOST + request);
			connection = (HttpURLConnection) url.openConnection();
			System.out.println("Connection established");
			
			connection.setRequestMethod("GET");
			connection.setDoOutput(true); //can write to stream
			connection.setDoInput(true); //can receive reply stream
			connection.setRequestProperty("Authorization","Bearer "+oAuthToken); //encode our secret and key
			connection.connect();
			Reader in = new BufferedReader(new InputStreamReader(connection.getInputStream(),"UTF-8"));
			response = formatInputStream(in);
			System.out.println("Response received: \""+connection.getResponseMessage() + "\", With code: "+connection.getResponseCode());
			
			//Build a bufferedreader to interpret the bufferedinputstream
			
		}catch(Exception e) {
			System.out.println(e.getStackTrace());
		}
		return response;
	}

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
	
	public void setSecret(String key) {
		this.secret = key;
	}
	
	public void setKey(String key) {
		this.key = key;
	}	
	
}
