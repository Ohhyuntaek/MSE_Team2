package com.example.demo;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.ConfigurableApplicationContext;

@SpringBootApplication
public class FarmWarsApplication {

	public static void main(String[] args) {
		// SpringApplication.run(FarmWarsApplication.class, args);
	
		ConfigurableApplicationContext ctx = SpringApplication.run(FarmWarsApplication.class, args);
		
		IPlayerRepository repo = ctx.getBean(IPlayerRepository.class);
		
		Player p = new Player("koo05249", "Oh my", "koo05249@@");
		repo.add(p);
	
	}

}
