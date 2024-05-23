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
		IGamehistoryRepository ghrepo = ctx.getBean(IGamehistoryRepository.class);
		
		// add sample data
		Player p = new Player("koo05249", "Oh my", "koo05249@@");
		Player p2 = new Player("penguin", "penguin_cute", "penguin@@");
		Player p3 = new Player("pineapple", "pine_apple", "pineapple@@");
		Player p4 = new Player("jy_moon", "JIYOUNG", "jy_moon@@");
		Player p5 = new Player("mse_student", "mse_hard_sad", "mse_student@@");
		Player p6 = new Player("evening_0505", "sunset_lover", "evening_0505@@");
		Player p7 = new Player("phone0402", "iphoneisthebest", "phone0402@@");
		Player p8 = new Player("qndke", "tom_cruise", "qndke@@");
		Player p9 = new Player("pdknwewm", "linsey_S2", "pdknwewm@@");
		Player p10 = new Player("wqkdefqg", "wqkdefqg", "wqkdefqg@@");
		
		repo.add(p);
		repo.add(p2);
		repo.add(p3);
		repo.add(p4);
		repo.add(p5);
		repo.add(p6);
		repo.add(p7);
		repo.add(p8);
		repo.add(p9);
		repo.add(p10);
		
		Gamehistory gh = new Gamehistory(p.getPrivateCode());
		Gamehistory gh2 = new Gamehistory(p2.getPrivateCode(),5,3,9,2);
		Gamehistory gh3 = new Gamehistory(p3.getPrivateCode(),6,3,7,2);
		Gamehistory gh4 = new Gamehistory(p4.getPrivateCode(),8,2,5,3);
		Gamehistory gh5 = new Gamehistory(p5.getPrivateCode(),9,7,4,3);
		Gamehistory gh6 = new Gamehistory(p6.getPrivateCode(),9,2,3,1);
		Gamehistory gh7 = new Gamehistory(p7.getPrivateCode(),7,1,9,1);
		Gamehistory gh8 = new Gamehistory(p8.getPrivateCode(),9,6,5,0);
		Gamehistory gh9 = new Gamehistory(p9.getPrivateCode(),6,3,8,4);
		Gamehistory gh10 = new Gamehistory(p10.getPrivateCode(),8,4,7,2);
		
		ghrepo.add(gh);
		ghrepo.add(gh2);
		ghrepo.add(gh3);
		ghrepo.add(gh4);
		ghrepo.add(gh5);
		ghrepo.add(gh6);
		ghrepo.add(gh7);
		ghrepo.add(gh8);
		ghrepo.add(gh9);
		ghrepo.add(gh10);
		
		
		// players in current db
		for(Player ps : repo.findAll()) {
			System.out.println(ps);
		}
		
		// game history in current db
		
		
//		Player p = new Player("koo05249", "Oh my", "koo05249@@");
//		repo.add(p);
//		
//		System.out.println("Player private code: "+p.getPrivateCode());
//		
//		Player p2 = new Player("tester", "testing", "asdf");
//		Player p3 = new Player("tester2", "testing2", "asdwasd");
//		
//		repo.add(p2);
//		repo.add(p3);
//		
//		System.out.println("All players: ");
//		for(Player ps : repo.findAll()) {
//			System.out.println(ps);
//		}
//		
//		System.out.println("Find player koo05249");
//		System.out.println(repo.findOnebyID("koo05249"));
//		
//		System.out.println("Find player p2");
//		System.out.println(repo.findOne(p2.getPrivateCode()));
//		
//		p2.setNickname("testing testing");
//		repo.update(p2);
//		System.out.println("changed nickname of p2");
//		System.out.println(repo.findOne(p2.getPrivateCode()));
//		
//		/*
//		 * System.out.println(repo.delete(p3));
//		 * System.out.println("All players after deleting: "); for (Player ps :
//		 * repo.findAll()) { System.out.println(ps); }
//		 */
//		
//		System.out.println("\n\nGame history test!!!");
//		
//		// gamehistory related test
//		
		
//		
//		Gamehistory gh = new Gamehistory(p.getPrivateCode());
//		ghrepo.add(gh);
//		
//		System.out.println(p);
//		System.out.println(gh);
//		
//		Gamehistory gh2 = new Gamehistory(p2.getPrivateCode(), 5, 5, 3, 3);
//		ghrepo.add(gh2);
//		
//		System.out.println(p2);
//		System.out.println(gh2);
//		
//		Gamehistory gh3 = new Gamehistory(p3.getPrivateCode(), 5, 5, 3, 0);
//		ghrepo.add(gh3);
//		
//		System.out.println(p3);
//		System.out.println(gh3);
//		
//		System.out.println("\nFind koo05249's gamehistory: ");
//		System.out.println(ghrepo.find(p.getPrivateCode()));
//		
//		System.out.println("\nAll gamehistories: ");
//		for(WinningRate ghs : ghrepo.findTopTotalHistory()) {
//			System.out.println(ghs);
//		}
//		
////		System.out.println("\nDelete gh3 player's gamehistory");
////		System.out.println(ghrepo.delete(gh3.getPrivateCode()));
////		System.out.println("\nAll gamehistories after deleting: ");
//		for(WinningRate ghs : ghrepo.findTopTotalHistory()) {
//			System.out.println(ghs);
//		}
//		
//		System.out.println("\nUpdate koo05249's history");
//		System.out.println("\nWin Easy");
//		GameResultInfo gri = new GameResultInfo(p.getPrivateCode(), "Easy", "Win");
//		ghrepo.update(gri);
//		System.out.println(ghrepo.find(p.getPrivateCode()));
//		System.out.println("\nLose Easy");
//		GameResultInfo gri2 = new GameResultInfo(p.getPrivateCode(), "Easy", "Lose");
//		ghrepo.update(gri2);
//		System.out.println(ghrepo.find(p.getPrivateCode()));
//		System.out.println("\nWin Hard");
//		GameResultInfo gri3 = new GameResultInfo(p.getPrivateCode(), "Hard", "Win");
//		ghrepo.update(gri3);
//		System.out.println(ghrepo.find(p.getPrivateCode()));
//		System.out.println("\nLose Hard");
//		GameResultInfo gri4 = new GameResultInfo(p.getPrivateCode(), "Hard", "Lose");
//		ghrepo.update(gri4);
//		System.out.println(ghrepo.find(p.getPrivateCode()));
//		
//		System.out.println("\nTest ranking");
//		System.out.println("Total ranking");
//		for(WinningRate ghs : ghrepo.findTopTotalHistory()) {
//			System.out.println(ghs);
//		}
//		System.out.println("Easy ranking");
//		for(WinningRate ghs : ghrepo.findTopEasyHistory()) {
//			System.out.println(ghs);
//		}
//		System.out.println("Hard ranking");
//		for(WinningRate ghs : ghrepo.findTopHardHistory()) {
//			System.out.println(ghs);
//		}
	}

}
