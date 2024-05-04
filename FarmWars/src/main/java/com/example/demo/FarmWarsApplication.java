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
		
		System.out.println("Player private code: "+p.getPrivateCode());
		
		
		Player p2 = new Player("tester", "testing", "asdf");
		Player p3 = new Player("tester2", "testing2", "asdwasd");
		
		repo.add(p2);
		repo.add(p3);
		
		System.out.println("All players: ");
		for(Player ps : repo.findAll()) {
			System.out.println(ps);
		}
		
		System.out.println("Find player koo05249");
		System.out.println(repo.findOnebyID("koo05249"));
		
		System.out.println("Find player p2");
		System.out.println(repo.findOne(p2.getPrivateCode()));
		
		p2.setNickname("testing testing");
		repo.update(p2);
		System.out.println("changed nickname of p2");
		System.out.println(repo.findOne(p2.getPrivateCode()));
		
		repo.delete(p3);
		System.out.println("All players after deleting: ");
		for (Player ps : repo.findAll()) {
			System.out.println(ps);
		}
		
	}

}
