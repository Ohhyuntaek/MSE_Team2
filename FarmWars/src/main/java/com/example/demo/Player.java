package com.example.demo;

import java.io.Serializable;

public class Player implements Serializable{
	
	private long privateCode;

	private String playerId;
	private String playerNickname;
	private String playerPassword;
	
	public Player() {	
	}
	
	public Player(String ID, String Nickname, String Password) {
		super();
		this.playerId = ID;
		this.playerNickname = Nickname;
		this.playerPassword = Password;
	}
	
	public Player(long PrivateCode, String ID, String Nickname, String Password) {
		super();
		this.privateCode = PrivateCode;
		this.playerId = ID;
		this.playerNickname = Nickname;
		this.playerPassword = Password;
	}
	
	public void setPrivateCode(long privatecode) {
		privateCode = privatecode;
	}
	
	public long getPrivateCode() {
		return privateCode;
	}
	
	public String getID() {
		return playerId;
	}

	public void setID(String iD) {
		playerId = iD;
	}
	
	public String getNickname() {
		return playerNickname;
	}
	
	public void setNickname(String nickname) {
		playerNickname = nickname;
	}
	
	public String getPassword() {
		return playerPassword;
	}
	
	public void setPassword(String password) {
		playerPassword = password;
	}
	
	@Override
	public String toString() {
		return "Player [PrivateCode=" + privateCode + ", ID=" + playerId + ", Nickname=" + playerNickname + ", Password=" + playerPassword
				+ "]";
	}
}
