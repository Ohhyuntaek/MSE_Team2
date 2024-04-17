package com.example.demo;

public class Player {
	private String ID;
	private String Nickname;
	private String Password;
	
	private int totalGameNumber;
	private int winGameNumber;
	
	private int easymodeTotalGameNumber;
	private int easymodeWinGameNumber;
	
	private int hardmodeTotalGameNumber;
	private int hardmodeWinGameNumber;
	
	public Player() {}
	
	public Player(String ID, String Nickname, String Password) {
		super();
		this.ID = ID;
		this.Nickname = Nickname;
		this.Password = Password;
		
		this.totalGameNumber = 0;
		this.winGameNumber = 0;
		this.easymodeTotalGameNumber = 0;
		this.easymodeWinGameNumber = 0;
		this.hardmodeTotalGameNumber = 0;
		this.hardmodeWinGameNumber = 0;
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
	public int getTotalGameNumber() {
		return totalGameNumber;
	}
	public void setTotalGameNumber(int totalGameNumber) {
		this.totalGameNumber = totalGameNumber;
	}
	public int getWinGameNumber() {
		return winGameNumber;
	}
	public void setWinGameNumber(int winGameNumber) {
		this.winGameNumber = winGameNumber;
	}
	
	public int getEasymodeTotalGameNumber() {
		return easymodeTotalGameNumber;
	}
	public void setEasymodeTotalGameNumber(int easymodeTotalGameNumber) {
		this.easymodeTotalGameNumber = easymodeTotalGameNumber;
	}
	public int getEasymodeWinGameNumber() {
		return easymodeWinGameNumber;
	}
	public void setEasymodeWinGameNumber(int easymodeWinGameNumber) {
		this.easymodeWinGameNumber = easymodeWinGameNumber;
	}
	public int getHardmodeTotalGameNumber() {
		return hardmodeTotalGameNumber;
	}
	public void setHardmodeTotalGameNumber(int hardmodeTotalGameNumber) {
		this.hardmodeTotalGameNumber = hardmodeTotalGameNumber;
	}
	public int getHardmodeWinGameNumber() {
		return hardmodeWinGameNumber;
	}
	public void setHardmodeWinGameNumber(int hardmodeWinGameNumber) {
		this.hardmodeWinGameNumber = hardmodeWinGameNumber;
	}
	
}
