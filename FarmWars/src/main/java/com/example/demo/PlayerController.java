package com.example.demo;

import java.util.List;

import org.apache.tomcat.util.json.JSONParser;
import org.apache.tomcat.util.json.ParseException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.*;

@RestController
public class PlayerController {
	
	@Autowired
	private IPlayerManager manager;
	
	@PostMapping(value="/signup", consumes=MediaType.APPLICATION_JSON_VALUE)
	public Player addPlayer(@RequestBody SignupInfo signinfo) {
		return manager.addPlayer(signinfo);
	}
	
	@PostMapping(value="/login", consumes=MediaType.APPLICATION_JSON_VALUE, produces=MediaType.APPLICATION_JSON_VALUE)
	public Player loginPlayer(@RequestBody LoginInfo loginfo) {
		return manager.loginPlayer(loginfo);
	}
	
	@PostMapping(value="/update", consumes=MediaType.APPLICATION_JSON_VALUE, produces=MediaType.APPLICATION_JSON_VALUE)
	public Player updatePlayer(@RequestBody Player p) {
		return manager.updatePlayer(p);
	}
	
	@PostMapping(value="/delete", consumes=MediaType.APPLICATION_JSON_VALUE, produces=MediaType.APPLICATION_JSON_VALUE)
	public int deletePlayer(@RequestBody Player p) {
		return manager.deletePlayer(p);
	}
	
	@PostMapping(value="/find_one", consumes=MediaType.APPLICATION_JSON_VALUE, produces=MediaType.APPLICATION_JSON_VALUE)
	public Player findOnePlayer(@RequestBody long privateCode) {
		return manager.findOnePlayer(privateCode);
	}
	
	@PostMapping(value="/find_all", consumes=MediaType.APPLICATION_JSON_VALUE, produces=MediaType.APPLICATION_JSON_VALUE)
	public List<Player> findAllPlayer() {
		return manager.findAllPlayer();
	}
	
	
	
	
}
