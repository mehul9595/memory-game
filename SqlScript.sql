CREATE DATABASE IF NOT EXISTS ColorGame;

Use ColorGame;

CREATE TABLE IF NOT EXISTS UserScore (
                                               ID VARCHAR(100), 
                                               NAME VARCHAR(100), 
                                               EMAIL VARCHAR(100),
                                               SCORE INT);

--To provide permissions, below query sets appropriate "User" i.e 'localhost' & "Password" i.e 'root123' for our database.
grant all privileges on colorgame.* to root@localhost identified by 'root123';

select * from userscore