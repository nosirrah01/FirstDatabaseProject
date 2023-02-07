using System;
using System.Data;

namespace NewUserConsoleApp
{
    internal class Program
    {
        

        static void Main(string[] args)
        {
            // Note: I am in the process of transitioning from UiClass to UIinator.
            // This method below references an old version of Main that is to be deleted
            // once functionality of new main is complete and it is no longer needed for reference.
            //InitialMessingAround();

            string name = UIinator.AskName();
            if (!SqlDoer.ValueIsInColumn(name, "User", "Username"))
            {
                SqlDoer.AddNameToUser(name);
            }
            UIinator.DisplayUserShowsNRatings(name);
            int actionInt;
            do
            {
                actionInt = UIinator.GetValidInt(6, UIinator.GetActionQuestion());
                UIinator.DoAction(actionInt, name);
            } while (actionInt != 6);
        }

        //TODO: Delete this method (Save for last.)
        //TODO: Delete UiClass once it is no longer needed.
        private static void InitialMessingAround()
        {
            Console.WriteLine("This line will appear in the branch!");

            Console.WriteLine("What would you like to do?");
            Console.WriteLine("Enter 1 for add user");
            Console.WriteLine("Enter 2 to display tables");


            // setting this temporarily. will add ability for user to enter their name later
            string name = "Ryan";

            DataTable selectedUserIdTable = SqlDoer.CreateDatatableFromQuery($"select [UserId] from [User] where [Username] = '{name}'");
            //selectedUserIdTable.ToString().Trim();


            UiClass.DisplayTable("Ryan's ID!!!", $"select [UserId] from [User] where [Username] = '{name}'");

            Console.WriteLine(selectedUserIdTable.Rows[0][0]);

            UiClass.DisplayTable($"{name}'s shows", $"select s.Name from Show s inner join UserShow us on s.ShowId = us.ShowID where us.UserID = {selectedUserIdTable.Rows[0][0]}");

            /*
            DisplayTable("User");
            DisplayTable("Show");
            DisplayTable("UserShow");
            */
            //DisplayTable("User 1's shows", @"select s.Name from Show s inner join UserShow us on s.ShowId = us.ShowID where us.UserID = 1");
            name = "Bob";
            selectedUserIdTable = SqlDoer.CreateDatatableFromQuery($"select [UserId] from [User] where [Username] = '{name}'");
            UiClass.DisplayTable($"{name}'s shows", $"select s.Name from Show s inner join UserShow us on s.ShowId = us.ShowID where us.UserID = {selectedUserIdTable.Rows[0][0]}");

            name = "David";
            selectedUserIdTable = SqlDoer.CreateDatatableFromQuery($"select [UserId] from [User] where [Username] = '{name}'");
            UiClass.DisplayTable($"{name}'s shows", $"select s.Name from Show s inner join UserShow us on s.ShowId = us.ShowID where us.UserID = {selectedUserIdTable.Rows[0][0]}");

            name = "Matt";
            selectedUserIdTable = SqlDoer.CreateDatatableFromQuery($"select [UserId] from [User] where [Username] = '{name}'");
            UiClass.DisplayTable($"{name}'s shows", $"select s.Name from Show s inner join UserShow us on s.ShowId = us.ShowID where us.UserID = {selectedUserIdTable.Rows[0][0]}");

            SqlDoer.UpdateDatabaseWithQuery("select * from [User];");
            UiClass.DisplayTable("User");

            Console.Read();
        }





    }
}
