package com.example.demo;

public class GameResultInfo {
	private long privateCode;
	private String gameMode;
	private String result;
	
	public GameResultInfo(long privateCode, String gameMode, String result) {
		super();
		this.privateCode = privateCode;
		this.gameMode = gameMode;
		this.result = result;
	}
	
	public long getPrivateCode() {
		return privateCode;
	}
	
	public String getGameMode() {
		return gameMode;
	}
	
	public String getResult() {
		return result;
	}
}
