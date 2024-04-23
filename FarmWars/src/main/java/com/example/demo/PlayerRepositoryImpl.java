package com.example.demo;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcOperations;
import org.springframework.jdbc.core.PreparedStatementCreator;
import org.springframework.stereotype.Repository;
import org.springframework.jdbc.support.*;

@Repository
public class PlayerRepositoryImpl implements IPlayerRepository {

	@Autowired
	private JdbcOperations jdbc;
	
	private static final String SQL_INSERT = "insert into player(ID, Nickname, Password) values (?, ?, ?)";
	
	
	@Override
	public Player add(Player player) {
		
		
		KeyHolder keyHolder = new GeneratedKeyHolder();
		
		int rows = jdbc.update(new PreparedStatementCreator() {
			
			@Override
			public PreparedStatement createPreparedStatement(Connection con) throws SQLException {
				PreparedStatement ps = con.prepareStatement(SQL_INSERT, new String[] {"PrivateCode"});
				ps.setString(1, player.getID());
				ps.setString(2, player.getNickname());
				ps.setString(3, player.getPassword());		
				return ps;
			}
		}, keyHolder);
		
		
		if (rows==1) {
			System.out.println("Add player succeed");
			player.setPrivateCode(keyHolder.getKey().longValue());
		} else {
			System.out.println("Add player failed");
			return null;
		}
		
		return null;
	}

	@Override
	public int update(Player player) {
		// TODO Auto-generated method stub
		return 0;
	}

	@Override
	public int delete(Player player) {
		// TODO Auto-generated method stub
		return 0;
	}

	@Override
	public Player findOne(long id) {
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	public List<Player> findAll() {
		// TODO Auto-generated method stub
		return null;
	}
	
	
}
