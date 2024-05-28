package com.example.demo;

import java.util.List;

import org.apache.tomcat.util.json.JSONParser;
import org.apache.tomcat.util.json.ParseException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.*;

@RestController
public class GamehistoryController {
	
	@Autowired
	private IGamehistoryManager manager;
	
	@PostMapping(value="/signup_history", consumes=MediaType.APPLICATION_JSON_VALUE)
	public Gamehistory addSignupHistory(@RequestBody Player p) {
		return manager.createNew(p);
	}
	
	@PostMapping(value="/login_history", consumes=MediaType.APPLICATION_JSON_VALUE, produces=MediaType.APPLICATION_JSON_VALUE)
	public Gamehistory loginFindHistory (@RequestBody Player p) {
		return manager.findHistory(p);
	}
	
	@PostMapping(value="/update_history", consumes=MediaType.APPLICATION_JSON_VALUE, produces=MediaType.APPLICATION_JSON_VALUE)
	public Gamehistory updatePlayer(@RequestBody GameResultInfo gri) {
		return manager.updateHistory(gri);
	}
	
	@PostMapping(value="/delete_history", consumes=MediaType.APPLICATION_JSON_VALUE, produces=MediaType.APPLICATION_JSON_VALUE)
	public int deletePlayer(@RequestBody Player p) {
		return manager.deleteHistory(p);
	}
	
	@PostMapping(value="/ranking_history_total", consumes=MediaType.APPLICATION_JSON_VALUE, produces=MediaType.APPLICATION_JSON_VALUE)
	public List<WinningRate> findTopHistoryTotal() {
		return manager.findTopTotalHistory();
	}
	
	@PostMapping(value="/ranking_history_easy", consumes=MediaType.APPLICATION_JSON_VALUE, produces=MediaType.APPLICATION_JSON_VALUE)
	public List<WinningRate> findTopHistoryEasy() {
		return manager.findTopEasyHistory();
	}
	
	@PostMapping(value="/ranking_history_hard", consumes=MediaType.APPLICATION_JSON_VALUE, produces=MediaType.APPLICATION_JSON_VALUE)
	public List<WinningRate> findTopHistoryHard() {
		return manager.findTopHardHistory();
	}	
	
	
}
