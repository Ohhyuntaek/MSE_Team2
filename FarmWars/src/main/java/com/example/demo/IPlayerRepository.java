package com.example.demo;

import java.util.List;

public interface IPlayerRepository {
	Player add(Player player);
	Player update(Player player);
	int delete(Player player);
	Player findOne(long privateCode);
	Player findOnebyID(String iD);
	List<Player> findAll();
}
