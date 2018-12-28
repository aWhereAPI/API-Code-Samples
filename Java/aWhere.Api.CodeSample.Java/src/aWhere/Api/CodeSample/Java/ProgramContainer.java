package aWhere.Api.CodeSample.Java;

import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.io.UnsupportedEncodingException;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;
import java.net.http.*;
import java.nio.charset.StandardCharsets;
import java.util.Base64;
import java.util.Map;

public class ProgramContainer {

	public static final String HOST = "https://api.awhere.com";
	
	URL url;
	
	private Map<String, String> parameters;
	private HttpURLConnection connection;
	private String request;
	private String oAuthToken;
	private String secret;
	private String key;
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
	
	public void createOAuthToken() {
		
	}
	
	/*
	 * 1: Connection started
	 * 0: Connection failed
	 */
	public boolean startConnection() {
		String auth_att = key + ":" + secret;
		BufferedReader reader;
		Base64.Encoder encoder = Base64.getEncoder();
		String encoded = encoder.encodeToString(auth_att.getBytes(StandardCharsets.UTF_8));
		try {
			url = new URL(HOST+"/oauth/token");
			System.out.println("Communicating with -> " + HOST + "/oauth/token");
			connection = (HttpURLConnection) url.openConnection();
			System.out.println("Connection established");
			connection.setDoOutput(true);
			connection.setDoInput(true);
			connection.setRequestProperty("Authorization","Basic "+encoded);
			connection.setRequestProperty("Content-Type", "application/x-www-form-urlencoded");
			connection.setRequestMethod("POST");
			OutputStream out = connection.getOutputStream();
			System.out.println("Adding body\n{\n\tgrant_type : client_credentials\n}");
			out.write("grant_type=client_credentials".getBytes("UTF8"));
			System.out.println(connection.getResponseMessage());
			System.out.println(connection.getResponseCode());
			System.out.println(connection.getContent());
			out.close();
		} catch(Exception e) {
			System.out.println("Error creating connection to host. Incorrect URL or HTTP Protocol.");
			e.printStackTrace();
			return false;
		}
		
		return true;
	}
	
	public String getRequest() {
		return request;
	}
	
	public void setRequest(String r) {
		request = r;
	}
	
	public boolean sendRequest() throws UnsupportedEncodingException, IOException {
		if(parameters.isEmpty() || connection == null)
			return false;
		/*DataOutputStream out = new DataOutputStream(connection.getOutputStream());
		out.writeBytes(ParameterStringBuilder.getParamsString(parameters));
		out.flush();
		out.close();*/
		return true;
	}
	
	public void setSecret(String key) {
		this.secret = key;
	}
	
	public void setKey(String key) {
		this.key = key;
	}
	
	
}
