package com.example.demo;

import java.util.ArrayList;

import org.springframework.stereotype.Service;

import jakarta.annotation.PostConstruct;

@Service
public class PlayerManagerImpl implements IPlayerManager {
	
	private ArrayList<Player> players = new ArrayList<Player>();
	private ArrayList<String> playerIDs = new ArrayList<String>();
	
	@PostConstruct
	private void init() {
		Player errorP = new Player("fail", "fail", "fail");
		players.add(errorP);
		playerIDs.add(errorP.getID());
	}
	
	@Override
	public String addPlayer(Player p) {
		if (p != null && players.indexOf(p)==-1) {
			int indexID = playerIDs.indexOf(p.getID());
			if (indexID == -1) {
				players.add(p);
				playerIDs.add(p.getID());
				System.out.println(playerIDs);
				return "Success Sign up";
			}
			return "Fail - Existing ID";
		}
		return "Fail - Existing User or data is null";
	}
	
	@Override
	public Player loginPlayer(LoginInfo loginfo) {
		int indexID = playerIDs.indexOf(loginfo.getID());
		if (loginfo != null && indexID!=-1) {
			if (players.get(indexID).getPassword().equals(loginfo.getPassword())) {
				return players.get(indexID);
			}
			else return new Player("fail", "fail", "fail");
		}
		else return new Player("fail", "fail", "fail");
	}
	
}
