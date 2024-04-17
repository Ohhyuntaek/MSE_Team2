package com.example.demo;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

@RestController
public class PlayerController {
	
	@Autowired
	private IPlayerManager manager;
	
	@PostMapping(value="/signup", consumes="application/json")
	public boolean addPlayer(@RequestBody Player p) {
		return manager.addPlayer(p);
	}
	
	@GetMapping(value="/login", produces="application/json")
	public Player loginPlayer(@RequestBody String ID, @RequestBody String Password) {
		return manager.loginPlayer(ID, Password);
	}
}
