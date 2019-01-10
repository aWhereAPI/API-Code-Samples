package com.aWhere.java;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.InputStream;
import java.util.Properties;
import java.util.Scanner;

/* Welcome to the aWhere code sample Java project. This should give you a basic overview of how easy it is to connect with aWhere's developer APIs with Java.
 * Make sure you are running on Java 11 or higher in order for this project to compile correctly.
 */
public class Main {
	
	/*
	 * Please make sure to configure the Key and Secret in the apiconfig.properties file before trying to compile and run this.
	 */
	
	private static String apiKey;
	private static String apiSecret;
	
	public final static Scanner input = new Scanner(System.in);
	
	/* Main loop method. 
	 * Creates an instance of the ProgramContainer class which contains the specific API calls we want, and authenticates it with the key and secret.
	 */
	public static void main(String[] args) {
		int menuChoice = 1;
		loadKeyAndSecret();
		if(!checkForKeyAndSecret()) {
			System.out.println("!#!->API Key and Secret not configured. Aborting.<-!#!");
			pressToContinue();
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
		do {
			menuChoice = mainMenu();
			switch(menuChoice) {
				case 1: if(!pgc.createAutoField()) System.out.println("Unexpected Error - Auto generated field failed to create"); break;
				case 2: if(!pgc.createField()) System.out.println("Unexpected Error - Please make sure all input was correct"); break;
				case 3: if(!pgc.getFields()) System.out.println("Unexpected Error - Please make sure all input was correct"); break;
				case 4: if(!pgc.getForecast()) System.out.println("Unexpected Error - Please make sure all input was correct"); break;
				case 5: if(!pgc.getDailyObservations()) System.out.println("Unexpected Error - Please make sure all input was correct"); break;
				default: break;
			}
		} while(menuChoice != 0);
		System.out.println("==================================================");
		System.out.println("                     Goodbye");
		System.out.println("==================================================");
	}
	
	/* pressToContinue()
	 * Pauses and waits for user to press Enter to continue program. This is meant just to reduce bloat, make main more readable. 
	 */
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

	/* int mainMenu()
	 * Prints the main menu for the main loop of the program. Sends the input option to the main function.
	 */
	public static int mainMenu() {
		System.out.println("Enter a number from the following list and press Enter:");
		System.out.println("[1] Auto Generate a new Field");
		System.out.println("[2] Create a new user input Field");
		System.out.println("[3] All Field Report");
		System.out.println("[4] Forecast Demo");
		System.out.println("[5] Daily Observations Demo");
		System.out.println("[0] Exit Program");
		String in = input.nextLine();
		System.out.flush();
		in = (in.isEmpty()) ? "999" : in;
		int out = Integer.parseInt(in);
		if(out < 0 || out > 5) {
			System.out.println("[ Incorrect Input ]");
			return 999;
		}
		return out;
	}
	
	/* loadKeyAndSecret()
	 * Opens the apiconfig.properties file and reads the key and secret if available.
	 */
	public static void loadKeyAndSecret() {
		File f = new File("apiconfig.properties");
		Properties p = new Properties();
		InputStream is = null;
		try {
			is = new FileInputStream(f);
		} catch(FileNotFoundException e) {
			e.printStackTrace();
			System.out.println("!#!->Cannot find the requested file<-!#!");
		} try {
			p.load(is);
		} catch(Exception e) {
			e.printStackTrace();
			System.out.println("!#!->Properties failed to load<-!#!");
		}
		apiSecret = p.getProperty("SECRET");
		apiKey = p.getProperty("KEY");
	}
	
	/* boolean checkForKeyAndSecret()
	 * Returns false if apiKey and apiSecret were not successfully retreived from config file.
	 */
	public static boolean checkForKeyAndSecret() {
		if(apiKey == null || apiSecret == null || apiKey.isEmpty() || apiSecret.isEmpty())
			return false;
		return true;
	}
	
}
