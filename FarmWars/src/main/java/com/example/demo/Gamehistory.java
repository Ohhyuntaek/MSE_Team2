package com.example.demo;

import java.io.Serializable;

public class Gamehistory implements Serializable{
	
	private long privateCode;
	
	private int easyGame;
	private int easyWin;
	
	private int hardGame;
	private int hardWin;
	
	public Gamehistory() {	
	}
	
	public Gamehistory(long privateCode) {
		super();
		this.privateCode = privateCode;
		
		this.easyGame = 0;
		this.easyWin = 0;
		
		this.hardGame = 0;
		this.hardWin = 0;
	}
	
	public Gamehistory(long privateCode, int easygame, int easywin, int hardgame, int hardwin) {
		super();
		this.privateCode = privateCode;
		
		this.easyGame = easygame;
		this.easyWin = easywin;
		
		this.hardGame = hardgame;
		this.hardWin = hardwin;
	}
	
	
	
	public long getPrivateCode() {
		return privateCode;
	}

	public void setPrivateCode(long privateCode) {
		this.privateCode = privateCode;
	}

	public int getEasyGame() {
		return easyGame;
	}

	public void setEasyGame(int easyGame) {
		this.easyGame = easyGame;
	}

	public int getEasyWin() {
		return easyWin;
	}

	public void setEasyWin(int easyWin) {
		this.easyWin = easyWin;
	}

	public int getHardGame() {
		return hardGame;
	}

	public void setHardGame(int hardGame) {
		this.hardGame = hardGame;
	}

	public int getHardWin() {
		return hardWin;
	}

	public void setHardWin(int hardWin) {
		this.hardWin = hardWin;
	}
	
	@Override
	public String toString() {
		return "Gamehistory [PrivateCode=" + privateCode + ", EasyGame=" + easyGame + ", EasyWin=" + easyWin + ", HardGame=" + hardGame + ", HardWin=" + hardWin
				+ "]";
	}
}
