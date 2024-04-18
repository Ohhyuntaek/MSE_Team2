package com.example.demo;

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
	public String addPlayer(@RequestBody Player p) {
		return manager.addPlayer(p);
	}
	
	@PostMapping(value="/login", consumes=MediaType.APPLICATION_JSON_VALUE, produces=MediaType.APPLICATION_JSON_VALUE)
	public Player loginPlayer(@RequestBody LoginInfo loginfo) {
		return manager.loginPlayer(loginfo);
	}
}
