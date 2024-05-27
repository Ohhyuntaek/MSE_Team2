package com.example.demo;

public class WinningRate {
	private String nickName;
	private double winningRate;
	
	public WinningRate(String nn, double wr) {
		super();
		this.nickName = nn;
		this.winningRate = wr;
	}

	public String getNickname() {
		return nickName;
	}

	public void setNickname(String nn) {
		this.nickName = nn;
	}

	public double getWinningRate() {
		return winningRate;
	}

	public void setWinningRate(double winningRate) {
		this.winningRate = winningRate;
	}
}
