package com.example.demo;

public interface IPlayerManager {
	
	boolean addPlayer(Player p);
	Player loginPlayer(String ID, String Password);
}
