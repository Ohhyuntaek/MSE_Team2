package com.example.demo;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcOperations;
import org.springframework.jdbc.core.PreparedStatementCreator;
import org.springframework.jdbc.core.RowMapper;
import org.springframework.stereotype.Repository;
import org.springframework.jdbc.support.*;

@Repository
public class PlayerRepositoryImpl implements IPlayerRepository {

	@Autowired
	private JdbcOperations jdbc;
	
	private static final String SQL_INSERT = "insert into player(playerId, playerNickname, playerPassword) values (?, ?, ?)";
	private static final String SQL_UPDATE = "update player set playerId=?, playerNickname=?, playerPassword=? where privateCode=?";
	private static final String SQL_DELETE = "delete player where privateCode=?";
	private static final String SQL_FIND_ONE = "select * from player where privateCode=?";
	private static final String SQL_FIND_ONE_BY_ID = "select * from player where playerId=?";
	private static final String SQL_FIND_ALL = "select * from player order by playerId";
	
	@Override
	public Player add(Player player) {
		
		
		KeyHolder keyHolder = new GeneratedKeyHolder();
		
		int rows = jdbc.update(new PreparedStatementCreator() {
			
			@Override
			public PreparedStatement createPreparedStatement(Connection con) throws SQLException {
				
				PreparedStatement ps = con.prepareStatement(SQL_INSERT, new String[] {"privateCode"});
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
		
		return player;
	}

	@Override
	public int update(Player player) {

		return jdbc.update(SQL_UPDATE, player.getID(), player.getNickname(), player.getPassword(), player.getPrivateCode());
		
	}

	@Override
	public int delete(Player player) {
		
		return jdbc.update(SQL_DELETE, player.getPrivateCode());
		
	}

	@Override
	public Player findOne(long privatecode) {
		
		return jdbc.queryForObject(SQL_FIND_ONE, new PlayerRowMapper(), privatecode);
	
	}
	
	@Override
	public Player findOnebyID(String iD) {
		return jdbc.queryForObject(SQL_FIND_ONE_BY_ID, new PlayerRowMapper(), iD);
	}

	@Override
	public List<Player> findAll() {
		
		return jdbc.query(SQL_FIND_ALL, new PlayerRowMapper());
		
	}
	
	private static class PlayerRowMapper implements RowMapper<Player>{

		@Override
		public Player mapRow(ResultSet rs, int rowNum) throws SQLException {
			System.out.println("Mapping row: "+rowNum);
			
			return new Player(
					rs.getLong("privateCode"),
					rs.getString("playerID"),
					rs.getString("playerNickname"),
					rs.getString("playerPassword")
					);
		}
		
	}
	
	
}
