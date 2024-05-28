package com.example.demo;

import java.util.List;

public interface IGamehistoryRepository {
	Gamehistory add(Gamehistory gh);
	Gamehistory find(long privateCode);
	Gamehistory update(GameResultInfo gri);
	int delete(long privateCode);
	List<WinningRate> findTopTotalHistory();
	List<WinningRate> findTopEasyHistory();
	List<WinningRate> findTopHardHistory();
}
