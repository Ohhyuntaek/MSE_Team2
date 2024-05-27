package com.example.demo;

import java.util.List;

public interface IGamehistoryManager {
	
	Gamehistory createNew(Player p);
	Gamehistory findHistory(Player p);
	Gamehistory updateHistory(GameResultInfo gri);
	int deleteHistory(Player p);
	List<WinningRate> findTopTotalHistory();
	List<WinningRate> findTopEasyHistory();
	List<WinningRate> findTopHardHistory();
}
