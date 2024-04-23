package com.example.demo;

import java.util.List;

public interface IPlayerRepository {
	Player add(Player player);
	int update(Player player);
	int delete(Player player);
	Player findOne(long id);
	List<Player> findAll();
}
