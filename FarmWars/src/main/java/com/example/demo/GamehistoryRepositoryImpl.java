package com.example.demo;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Comparator;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcOperations;
import org.springframework.jdbc.core.PreparedStatementCreator;
import org.springframework.jdbc.core.RowMapper;
import org.springframework.stereotype.Repository;

import org.springframework.jdbc.support.*;

@Repository
public class GamehistoryRepositoryImpl implements IGamehistoryRepository {

	@Autowired
	private JdbcOperations jdbc;
	
	@Autowired
	private IPlayerRepository playerRepo;
	
	private static final String SQL_INSERT = "insert into gamehistory(privateCode, easyGame, easyWin, hardGame, hardWin) values (?,?,?,?,?)";
	private static final String SQL_FIND = "select * from gamehistory where privateCode=?";
	private static final String SQL_UPDATE = "update gamehistory set easyGame=?, easyWin=?, hardGame=?, hardWin=? where privateCode=?";
	private static final String SQL_DELETE = "delete gamehistory where privateCode=?";
	private static final String SQL_FIND_ALL = "select * from gamehistory order by privateCode";
	
	@Override
	public Gamehistory add(Gamehistory gh) {
		
		int rows = jdbc.update(new PreparedStatementCreator() {
			
			@Override
			public PreparedStatement createPreparedStatement(Connection con) throws SQLException {
				
				PreparedStatement ps = con.prepareStatement(SQL_INSERT);
				ps.setLong(1, gh.getPrivateCode());
				ps.setInt(2, gh.getEasyGame());
				ps.setInt(3, gh.getEasyWin());
				ps.setInt(4, gh.getHardGame());
				ps.setInt(5, gh.getHardWin());
				return ps;
			}
		});
		
		
		if (rows==1) {
			System.out.println("Add gamehistory succeed");
		} else {
			System.out.println("Add gamehistory failed");
			return null;
		}
		
		return gh;
		
	}
	
	@Override
	public Gamehistory find(long privateCode) {
		return jdbc.queryForObject(SQL_FIND, new GamehistoryRowMapper(), privateCode);
	}
	
	@Override
	public Gamehistory update(GameResultInfo gri) {
		
		Gamehistory gh = find(gri.getPrivateCode());
		
		if (gri.getGameMode().equals("Easy")) {
			if (gri.getResult().equals("Win")) {
				int updateResult = jdbc.update(SQL_UPDATE, gh.getEasyGame()+1, gh.getEasyWin()+1, gh.getHardGame(), gh.getHardWin(), gri.getPrivateCode());
				if (updateResult == 1) {
					return gh;
				}
			}
			else {
				int updateResult = jdbc.update(SQL_UPDATE, gh.getEasyGame()+1, gh.getEasyWin(), gh.getHardGame(), gh.getHardWin(), gri.getPrivateCode());
				if (updateResult == 1) {
					return gh;
				}
			}
		}
		else if (gri.getGameMode().equals("Hard")) {
			if (gri.getResult().equals("Win")) {
				int updateResult = jdbc.update(SQL_UPDATE, gh.getEasyGame(), gh.getEasyWin(), gh.getHardGame()+1, gh.getHardWin()+1, gri.getPrivateCode());
				if (updateResult == 1) {
					return gh;
				}
			}
			else {
				int updateResult = jdbc.update(SQL_UPDATE, gh.getEasyGame(), gh.getEasyWin(), gh.getHardGame()+1, gh.getHardWin(), gri.getPrivateCode());
				if (updateResult == 1) {
					return gh;
				}
			}
		}
		
		return null;
	}
	
	@Override
	public int delete(long privateCode) {
		return jdbc.update(SQL_DELETE, privateCode);
	}
	
	@Override
	public List<WinningRate> findTopTotalHistory() {
		List <Gamehistory> allhistory = jdbc.query(SQL_FIND_ALL, new GamehistoryRowMapper());
		
		Map<String, Double> winningrateMap = new HashMap<>();
		
		for(Gamehistory gh : allhistory) {
			if (!((gh.getEasyGame()==0)&(gh.getHardGame()==0))) {
				winningrateMap.put(playerRepo.findOne(gh.getPrivateCode()).getNickname(), (double) ((gh.getEasyWin()+gh.getHardWin())*100/(gh.getEasyGame()+gh.getHardGame())));
			}
			else {
				winningrateMap.put(playerRepo.findOne(gh.getPrivateCode()).getNickname(), (double) 0);
			}
		}
		
		List<String> privateCodeSet = new ArrayList<>(winningrateMap.keySet());
		
		privateCodeSet.sort(new Comparator<String>() {
            @Override
            public int compare(String o1, String o2) {
                return winningrateMap.get(o1).compareTo(winningrateMap.get(o2));
            }
        });
		
		privateCodeSet.sort((o1, o2) -> winningrateMap.get(o2).compareTo(winningrateMap.get(o1)));
		
		for (String key : privateCodeSet) {
			System.out.print("Key : "+key);
			System.out.println(", Winning Rate : "+winningrateMap.get(key));
		}
		
		List<WinningRate> sortedWinningRateList = new ArrayList<WinningRate>() {};
		for (String key : privateCodeSet) {
			WinningRate wr = new WinningRate(key, winningrateMap.get(key));
			System.out.println(key+"\t"+winningrateMap.get(key)+"~~~~~");
			sortedWinningRateList.add(wr);
		}
		
		return sortedWinningRateList;
	}
	
	@Override
	public List<WinningRate> findTopEasyHistory() {
		List <Gamehistory> allhistory = jdbc.query(SQL_FIND_ALL, new GamehistoryRowMapper());
		
		Map<String, Double> winningrateMap = new HashMap<>();
		
		for(Gamehistory gh : allhistory) {
			if (!(gh.getEasyGame()==0)) {
				winningrateMap.put(playerRepo.findOne(gh.getPrivateCode()).getNickname(), (double) (gh.getEasyWin()*100/gh.getEasyGame()));
			}
			else {
				winningrateMap.put(playerRepo.findOne(gh.getPrivateCode()).getNickname(), (double) 0);
			}
		}
		
		List<String> privateCodeSet = new ArrayList<>(winningrateMap.keySet());
		
		privateCodeSet.sort(new Comparator<String>() {
            @Override
            public int compare(String o1, String o2) {
                return winningrateMap.get(o1).compareTo(winningrateMap.get(o2));
            }
        });
		
		privateCodeSet.sort((o1, o2) -> winningrateMap.get(o2).compareTo(winningrateMap.get(o1)));
		
		for (String key : privateCodeSet) {
			System.out.print("Key : "+key);
			System.out.println(", Winning Rate : "+winningrateMap.get(key));
		}
		
		List<WinningRate> sortedWinningRateList = new ArrayList<WinningRate>() {};
		for (String key : privateCodeSet) {
			WinningRate wr = new WinningRate(key, winningrateMap.get(key));
			sortedWinningRateList.add(wr);
		}
		
		return sortedWinningRateList;
	}
	
	@Override
	public List<WinningRate> findTopHardHistory() {
		List <Gamehistory> allhistory = jdbc.query(SQL_FIND_ALL, new GamehistoryRowMapper());
		
		Map<String, Double> winningrateMap = new HashMap<>();
		
		for(Gamehistory gh : allhistory) {
			if (!(gh.getHardGame()==0)) {
			winningrateMap.put(playerRepo.findOne(gh.getPrivateCode()).getNickname(), (double) (gh.getHardWin()*100/gh.getHardGame()));
			}
			else {
				winningrateMap.put(playerRepo.findOne(gh.getPrivateCode()).getNickname(), (double) 0);
			}
		}
		
		List<String> privateCodeSet = new ArrayList<>(winningrateMap.keySet());
		
		privateCodeSet.sort(new Comparator<String>() {
            @Override
            public int compare(String o1, String o2) {
                return winningrateMap.get(o1).compareTo(winningrateMap.get(o2));
            }
        });
		
		privateCodeSet.sort((o1, o2) -> winningrateMap.get(o2).compareTo(winningrateMap.get(o1)));
		
		for (String key : privateCodeSet) {
			System.out.print("Key : "+key);
			System.out.println(", Winning Rate : "+winningrateMap.get(key));
		}
		
		List<WinningRate> sortedWinningRateList = new ArrayList<WinningRate>() {};
		for (String key : privateCodeSet) {
			WinningRate wr = new WinningRate(key, winningrateMap.get(key));
			sortedWinningRateList.add(wr);
		}
		
		return sortedWinningRateList;
	}
	
	private static class GamehistoryRowMapper implements RowMapper<Gamehistory>{

		@Override
		public Gamehistory mapRow(ResultSet rs, int rowNum) throws SQLException {
			System.out.println("Mapping row: "+rowNum);
			
			return new Gamehistory(
					rs.getLong("privateCode"),
					rs.getInt("EasyGame"),
					rs.getInt("EasyWin"),
					rs.getInt("HardGame"),
					rs.getInt("HardWin")
					);
		}
		
	}
	
		
}
