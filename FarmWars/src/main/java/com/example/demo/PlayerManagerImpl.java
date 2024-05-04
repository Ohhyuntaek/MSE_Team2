package com.example.demo;

import java.util.ArrayList;
import java.util.List;

import org.springframework.stereotype.Service;
import jakarta.annotation.PostConstruct;
import org.springframework.beans.factory.annotation.Autowired;

@Service
public class PlayerManagerImpl implements IPlayerManager {
	
	private ArrayList<Player> players = new ArrayList<Player>();
	private ArrayList<String> playerIDs = new ArrayList<String>();
	
//	@PostConstruct
//	private void init() {
//		Player errorP = new Player("fail", "fail", "fail");
//		players.add(errorP);
//		playerIDs.add(errorP.getID());
//	}
//
	
//	@Override
//	public Player addPlayer(Player p) {
//		System.out.println(p);
//		if (p != null && players.indexOf(p)==-1) {
//			int indexID = playerIDs.indexOf(p.getID());
//			if (indexID == -1) {
//				players.add(p);
//				playerIDs.add(p.getID());
//				System.out.println(playerIDs);
//				return p;
//			}
//			return null;
//		}
//		return null;
//	}
//	
//	@Override
//	public Player loginPlayer(LoginInfo loginfo) {
//		int indexID = playerIDs.indexOf(loginfo.getID());
//		if (loginfo != null && indexID!=-1) {
//			if (players.get(indexID).getPassword().equals(loginfo.getPassword())) {
//				return players.get(indexID);
//			}
//			else return new Player("fail", "fail", "fail");
//		}
//		else return new Player("fail", "fail", "fail");
//	}
	
	@Autowired
	private IPlayerRepository playerRepository;
	
	@Override
	public Player addPlayer(SignupInfo signinfo) {
		Player p = new Player(signinfo.getID(), signinfo.getNickname(), signinfo.getPassword());
		return playerRepository.add(p);
	}
	
	@Override
	public int updatePlayer(Player p) {
		return playerRepository.update(p);
	}
	
	@Override
	public int deletePlayer(Player p) {
		return playerRepository.delete(p);
	}
	
	@Override
	public Player findOnePlayer(long privateCode) {
		return playerRepository.findOne(privateCode);
	}
	
	@Override
	public List<Player> findAllPlayer() {
		return playerRepository.findAll();
	}
	
	@Override
	public Player loginPlayer(LoginInfo loginfo) {
		Player insert_p = playerRepository.findOnebyID(loginfo.getID());
		if (insert_p.getPassword().equals(loginfo.getPassword())) {
			return insert_p;
		}
		return new Player("fail", "fail", "fail");
	}
}
