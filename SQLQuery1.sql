/* display all of the tables */
/*
select * from Show;
select * from [User];
select * from UserShow;
*/

/* all of the shows that user 1 has watched using inner join */
select s.Name from Show s inner join UserShow us on s.ShowId = us.ShowID where us.UserID = 1