package com.example.demo;

import java.util.ArrayList;
import java.util.List;

import org.springframework.stereotype.Service;
import jakarta.annotation.PostConstruct;
import org.springframework.beans.factory.annotation.Autowired;

@Service
public class PlayerManagerImpl implements IPlayerManager {
	
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
		return null;
	}
}
