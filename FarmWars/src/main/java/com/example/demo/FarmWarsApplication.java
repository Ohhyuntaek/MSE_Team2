package com.example.demo;

import java.util.List;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.ConfigurableApplicationContext;

@SpringBootApplication
public class FarmWarsApplication {

	public static void main(String[] args) {
		// SpringApplication.run(FarmWarsApplication.class, args);
	
		ConfigurableApplicationContext ctx = SpringApplication.run(FarmWarsApplication.class, args);
		
		// player related ones test
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
		
		/*
		 * System.out.println(repo.delete(p3));
		 * System.out.println("All players after deleting: "); for (Player ps :
		 * repo.findAll()) { System.out.println(ps); }
		 */
		
		System.out.println("\n\nGame history test!!!");
		
		// gamehistory related test
		
		IGamehistoryRepository ghrepo = ctx.getBean(IGamehistoryRepository.class);
		
		Gamehistory gh = new Gamehistory(p.getPrivateCode());
		ghrepo.add(gh);
		
		System.out.println(p);
		System.out.println(gh);
		
		Gamehistory gh2 = new Gamehistory(p2.getPrivateCode(), 5, 5, 3, 3);
		ghrepo.add(gh2);
		
		System.out.println(p2);
		System.out.println(gh2);
		
		Gamehistory gh3 = new Gamehistory(p3.getPrivateCode(), 5, 5, 3, 0);
		ghrepo.add(gh3);
		
		System.out.println(p3);
		System.out.println(gh3);
		
		System.out.println("\nFind koo05249's gamehistory: ");
		System.out.println(ghrepo.find(p.getPrivateCode()));
		
		System.out.println("\nAll gamehistories: ");
		for(WinningRate ghs : ghrepo.findTopTotalHistory()) {
			System.out.println(ghs);
		}
		
//		System.out.println("\nDelete gh3 player's gamehistory");
//		System.out.println(ghrepo.delete(gh3.getPrivateCode()));
//		System.out.println("\nAll gamehistories after deleting: ");
		for(WinningRate ghs : ghrepo.findTopTotalHistory()) {
			System.out.println(ghs);
		}
		
		System.out.println("\nUpdate koo05249's history");
		System.out.println("\nWin Easy");
		GameResultInfo gri = new GameResultInfo(p.getPrivateCode(), "Easy", "Win");
		ghrepo.update(gri);
		System.out.println(ghrepo.find(p.getPrivateCode()));
		System.out.println("\nLose Easy");
		GameResultInfo gri2 = new GameResultInfo(p.getPrivateCode(), "Easy", "Lose");
		ghrepo.update(gri2);
		System.out.println(ghrepo.find(p.getPrivateCode()));
		System.out.println("\nWin Hard");
		GameResultInfo gri3 = new GameResultInfo(p.getPrivateCode(), "Hard", "Win");
		ghrepo.update(gri3);
		System.out.println(ghrepo.find(p.getPrivateCode()));
		System.out.println("\nLose Hard");
		GameResultInfo gri4 = new GameResultInfo(p.getPrivateCode(), "Hard", "Lose");
		ghrepo.update(gri4);
		System.out.println(ghrepo.find(p.getPrivateCode()));
		
		System.out.println("\nTest ranking");
		System.out.println("Total ranking");
		for(WinningRate ghs : ghrepo.findTopTotalHistory()) {
			System.out.println(ghs);
		}
		System.out.println("Easy ranking");
		for(WinningRate ghs : ghrepo.findTopEasyHistory()) {
			System.out.println(ghs);
		}
		System.out.println("Hard ranking");
		for(WinningRate ghs : ghrepo.findTopHardHistory()) {
			System.out.println(ghs);
		}
	}

}
