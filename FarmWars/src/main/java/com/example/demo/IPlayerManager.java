package com.example.demo;

import java.util.List;

public interface IPlayerManager {
	
	Player addPlayer(SignupInfo signinfo);
	Player loginPlayer(LoginInfo loginfo);
	int updatePlayer(Player p);
	int deletePlayer(Player p);
	Player findOnePlayer(long privateCode);
	List<Player> findAllPlayer();
}
