package com.example.demo;

public class LoginInfo {
	private String ID;
	private String Password;
	
	
	public LoginInfo(String ID, String Password) {
		super();
		this.ID = ID;
		this.Password = Password;
	}
	
	public String getID() {
		return ID;
	}
	public String getPassword() {
		return Password;
	}
	
}
