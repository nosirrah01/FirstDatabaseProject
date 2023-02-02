using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using NewUserConsoleApp.FirstDatabaseDataSetTableAdapters;
using System.Collections;

namespace NewUserConsoleApp
{
    internal class Program
    {
        

        static void Main(string[] args)
        {
            Console.WriteLine("This line will appear in the branch!");

            Console.WriteLine("What would you like to do?");
            Console.WriteLine("Enter 1 for add user");
            Console.WriteLine("Enter 2 to display tables");
            

            // setting this temporarily. will add ability for user to enter their name later
            string name = "Ryan";

            DataTable selectedUserIdTable = SqlDoer.CreateDatatableFromQuery($"select [UserId] from [User] where [Username] = '{name}'");
            //selectedUserIdTable.ToString().Trim();
            

            UiClass.DisplayTable("Ryan's ID!!!",$"select [UserId] from [User] where [Username] = '{name}'");

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
