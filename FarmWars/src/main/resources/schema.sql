create table Player (
	PrivateCode identity,
	ID varchar(20) unique not null,
	Nickname varchar(20) not null,
	Password varchar(20) not null
);