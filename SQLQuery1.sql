/* display all of the tables */
/*
select * from Show;
select * from [User];
select * from UserShow;
*/

/* all of the shows that user 1 has watched using inner join */
/*
select s.Name from Show s inner join UserShow us on s.ShowId = us.ShowID where us.UserID = 1


select [UserId] from [User] where [Username] = 'Ryan'

insert into [User] (Username) values ('Jacob');
select * from [User];
*/

-- select [UserID] from [UserShow];

SELECT [ShowID], [Rating]
FROM [UserShow]
ORDER BY [ShowID]
--where [ShowID] = 3;


select s.Name as 'Show Name', us.Rating as 'Your Rating' from Show s inner join UserShow us on s.ShowId = us.ShowID;

--where us.UserID = 1
--select s.[Name] as 'Show Name', (AVG(us.Rating)) as 'Average Rating' from Show s inner join UserShow us on s.ShowId = us.ShowID where us.ShowID = 2; 

SELECT us.[ShowID], AVG(us.[Rating]) as 'Average Rating'
FROM [UserShow] us
GROUP BY [ShowID];

SELECT us.[ShowID], ROUND(AVG(CAST(us.[Rating] as decimal(4,2))), 1, 1) as 'Average Rating'
FROM [UserShow] us
GROUP BY [ShowID];

SELECT s.[Name] as 'Show Name', CAST(ROUND(AVG(CAST(us.[Rating] as decimal(4,2))), 1, 1) as varchar(50)) as 'Average Rating'
FROM Show s
INNER JOIN [UserShow] us on s.[ShowId] = us.[ShowID]
GROUP BY s.[Name];
