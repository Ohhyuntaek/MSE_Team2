create table Player (
	privateCode identity,
	playerId varchar(20) unique not null,
	playerNickname varchar(20) not null,
	playerPassword varchar(20) not null
);