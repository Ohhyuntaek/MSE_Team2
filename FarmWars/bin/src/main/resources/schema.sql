create table Player (
	privateCode identity,
	playerId varchar(20) unique not null,
	playerNickname varchar(20) not null,
	playerPassword varchar(20) not null
);

create table Gamehistory (
	privateCode identity,
	easyGame integer not null,
	easyWin integer not null,
	hardGame integer not null,
	hardWin integer not null
);