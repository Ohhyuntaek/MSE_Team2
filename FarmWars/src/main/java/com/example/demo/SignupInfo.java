package com.example.demo;

public class SignupInfo {
	private String ID;
	private String Nickname;
	private String Password;
	
	
	public SignupInfo(String ID, String Nickname, String Password) {
		super();
		this.ID = ID;
		this.Nickname = Nickname;
		this.Password = Password;
	}
	
	public String getID() {
		return ID;
	}
	
	public String getNickname() {
		return Nickname;
	}
	
	public String getPassword() {
		return Password;
	}
	
}
