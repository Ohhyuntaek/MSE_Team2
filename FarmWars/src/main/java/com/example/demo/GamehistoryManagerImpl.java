package com.example.demo;

import java.util.ArrayList;
import java.util.List;

import org.springframework.stereotype.Service;
import jakarta.annotation.PostConstruct;
import org.springframework.beans.factory.annotation.Autowired;

@Service
public class GamehistoryManagerImpl implements IGamehistoryManager {
	
	@Autowired
	private IGamehistoryRepository gamehistoryRepository;

	@Override
	public Gamehistory createNew(Player p) {
		Gamehistory gh = new Gamehistory(p.getPrivateCode());
		return gamehistoryRepository.add(gh);
	}

	@Override
	public Gamehistory findHistory(Player p) {
		return gamehistoryRepository.find(p.getPrivateCode());
	}

	@Override
	public Gamehistory updateHistory(GameResultInfo gri) {
		return gamehistoryRepository.update(gri);
	}

	@Override
	public int deleteHistory(Player p) {
		return gamehistoryRepository.delete(p.getPrivateCode());
	}
	
	@Override
	public List<WinningRate> findTopTotalHistory() {
		return gamehistoryRepository.findTopTotalHistory();
	}
	
	@Override
	public List<WinningRate> findTopEasyHistory() {
		return gamehistoryRepository.findTopEasyHistory();
	}
	
	@Override
	public List<WinningRate> findTopHardHistory() {
		return gamehistoryRepository.findTopHardHistory();
	}
	
}
