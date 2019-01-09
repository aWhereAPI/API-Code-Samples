package com.aWhere.java;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.InputStream;
import java.util.Properties;
import java.util.Scanner;

public class Main {
	
	//Do not add to code here, use apiconfig.properties
	//Do not commit properties file to git
	private static String apiKey; //= "";
	private static String apiSecret;// = "";
	
	public final static Scanner input = new Scanner(System.in);
	
	public static void main(String[] args) {
		int menuChoice = 1;
		loadKeyAndSecret();
		//Check to see if the user has configured the key and secret appropriately
		if(!checkForKeyAndSecret()) {
			System.out.println("!#!->API Key and Secret not configured. Aborting.<-!#!");
			System.exit(0);
		}
		System.out.println("--------------------------------------------------");
		System.out.println("=======Welcome to the aWhere Java API Demo=======");
		System.out.print("First, we will get an OAuth token in order to do\nsome test queries with our api. ");
		System.out.println("The REST Api works\nfluidly with default Java packages and resources.");
		System.out.println("We will start with a POST request to aWhere first.");
		System.out.println("==================================================");
		pressToContinue();
		System.out.println("Creating Java aWhere API client");
		ProgramContainer pgc = new ProgramContainer(apiSecret, apiKey);
		pgc.authenticate();
		System.out.println("OAuth Token has been created. On the following screen you\nwill be allowed to choose what to query with aWhere servers and\noutput it to the screen in raw results or formatted results.");
		pressToContinue();
		do{
			menuChoice = mainMenu();
			if(menuChoice == 1) {
				if(!pgc.getDailyObservations())
					System.out.println("Unexpected Error - Please make sure all input was correct");
			}else if(menuChoice == 2) {
				if(!pgc.getForecast())
					System.out.println("Unexpected Error - Please make sure all input was correct");
			}else if(menuChoice == 3) {
				if(!pgc.getFields())
					System.out.println("Unexpected Error - Please make sure all input was correct");
			}else if(menuChoice == 4) {
				if(!pgc.createField())
					System.out.println("Unexpected Error - Please make sure all input was correct");
			}
		}while(menuChoice != 0);
		System.out.println("==================================================");
		System.out.println("                    Goodbye!");
		System.out.println("==================================================");
	}
	
	public static void pressToContinue() {
		System.out.println("    ---    [Press Enter to Continue]    ---");
		try {
			input.nextLine();
			System.out.flush();
			System.out.println("==================================================");
		} catch (Exception e) {
			System.out.println("Unexpected I/O error");
			e.printStackTrace();
		}
	}

	public static int mainMenu() {
		System.out.println("Enter a number from the following list and press Enter:");
		System.out.println("[1] Daily Observations");
		System.out.println("[2] Forecast");
		System.out.println("[3] All Field Report");
		System.out.println("[4] Create a new Field");
		System.out.println("[0] Exit Program");
		String in = input.nextLine();
		System.out.flush();
		in = (in.isBlank()) ? "999" : in;
		int out = Integer.parseInt(in);
		if(out < 0 || out > 4) {
			System.out.println("[ Incorrect Input ]");
			return 999;
		}
		return out;
	}
	
	public static void loadKeyAndSecret() {
		File f = new File("apiconfig.properties");
		Properties p = new Properties();
		InputStream is = null;
		try {
			is = new FileInputStream(f);
		}catch(FileNotFoundException e) {
			e.printStackTrace();
			System.out.println("!#!->Cannot find the requested file<-!#!");
		}
		try {
			p.load(is);
		}catch(Exception e) {
			e.printStackTrace();
			System.out.println("!#!->Properties failed to load<-!#!");
		}
		apiSecret=p.getProperty("SECRET");
		apiKey = p.getProperty("KEY");
	}
	
	public static boolean checkForKeyAndSecret() {
		if(apiKey == null || apiSecret == null || apiKey.isBlank() || apiSecret.isBlank())
			return false;
		return true;
	}
	
}
