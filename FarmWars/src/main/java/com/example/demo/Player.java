package com.example.demo;

public class Player {
	private long PrivateCode;

	private String ID;
	private String Nickname;
	private String Password;
	
	
	public Player(String ID, String Nickname, String Password) {
		super();
		this.ID = ID;
		this.Nickname = Nickname;
		this.Password = Password;
	}
	

	public void setPrivateCode(long privateCode) {
		PrivateCode = privateCode;
	}
	
	public String getID() {
		return ID;
	}
	public void setID(String iD) {
		ID = iD;
	}
	public String getNickname() {
		return Nickname;
	}
	public void setNickname(String nickname) {
		Nickname = nickname;
	}
	public String getPassword() {
		return Password;
	}
	public void setPassword(String password) {
		Password = password;
	}
	
	public String printThis() {
		return "ID: "+this.ID+", Nickname: "+this.Nickname+", Password: "+this.Password;
	}
}
