package com.example.demo;

import java.util.ArrayList;

import org.springframework.stereotype.Service;

import jakarta.annotation.PostConstruct;

@Service
public class PlayerManagerImpl implements IPlayerManager {
	
	private ArrayList<Player> players = new ArrayList<Player>();
	private ArrayList<String> playerIDs = new ArrayList<String>();
	private ArrayList<String> playerNicknames = new ArrayList<String>();
	
	@PostConstruct
	private void init() {
		players.add(new Player("koo05249", "기간제라니", "koo05249@@"));
	}
	
	@Override
	public boolean addPlayer(Player p) {
		if (p != null && players.indexOf(p)==-1) {
			int indexID = playerIDs.indexOf(p.getID());
			int indexNickname = playerNicknames.indexOf(p.getNickname());
			if (indexID != -1 && indexNickname != -1) {
				players.add(p);
				playerIDs.add(p.getID());
				playerNicknames.add(p.getNickname());
				return true;
			}
			return false;
		}
		return false;
	}
	
	@Override
	public Player loginPlayer(String ID, String Password) {
		int indexID = playerIDs.indexOf(ID);
		if (ID != null && indexID!=-1) {
			if (players.get(indexID).getPassword().equals(Password)) {
				return players.get(indexID);
			}
			else return new Player("fail", "fail", "fail");
		}
		else return new Player("fail", "fail", "fail");
	}
	
}
