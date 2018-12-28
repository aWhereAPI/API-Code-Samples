package aWhere.Api.CodeSample.Java;

import java.io.IOException;
import java.net.http.*;

public class Main {

	public final static String API_KEY = "";
	public final static String API_SECRET = "";
	
	
	public static void main(String[] args) {
		System.out.println("Creating Java aWhere API client");
		ProgramContainer pgc = new ProgramContainer(API_SECRET, API_KEY);
		pgc.startConnection();
		pgc.setRequest("/v2/fields/field1");
		try {
			pgc.sendRequest();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

}
