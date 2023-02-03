using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NewUserConsoleApp
{
    internal class UIinator
    {
        public static string AskName()
        {
            Console.WriteLine("What is your name?");
            return Console.ReadLine();
        }

        internal static string AskAction()
        {
            Console.WriteLine("What would you like to do?\r\nEnter '1' to add a show\r\n'2' display your watched shows\r\n'3' display all known shows\r\n'4' to change your rating of a show\r\n'5' to remove a show\r\n'6' to quit the application");
            return Console.ReadLine();
        }

        internal static void DisplayUserShowsNRatings(string name)
        {
            DataTable userShowsNRatings = SqlDoer.GetUserShowsNRatings(name);
            PrintTable("Shows You Have Watched", userShowsNRatings);

        }

        private static void PrintTable(string tableName, DataTable dataTable)
        {
            int formattedItemLength = getFormattedItemLength(dataTable);
            string formatedStringStructure = @"{0," + formattedItemLength + "}|";
            Console.WriteLine(tableName);
            // print top line
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                Console.Write("+");
                for (int j = 0; j < formattedItemLength; j++)
                {
                    Console.Write("–");
                }
            }
            Console.WriteLine("+");
            // print column names
            Console.Write("|");
            foreach (DataColumn dataColumn in dataTable.Columns)
            {
                Console.Write(String.Format(formatedStringStructure, dataColumn.ColumnName));
            }
            Console.WriteLine();

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                Console.Write("+");
                for (int j = 0; j < formattedItemLength; j++)
                {
                    Console.Write("–");
                }
            }
            Console.WriteLine("+");
            // print items
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Console.Write("|");
                foreach (var item in dataRow.ItemArray)
                {
                    Console.Write(String.Format(formatedStringStructure, item));
                }
                Console.WriteLine();

                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    Console.Write("+");
                    for (int j = 0; j < formattedItemLength; j++)
                    {
                        Console.Write("–");
                    }
                }
                Console.WriteLine("+");

            }
        }

        private static int getFormattedItemLength(DataTable dataTable)
        {
            int formattedItemLength = 0;
            foreach (DataColumn dataColumn in dataTable.Columns)
            {
                if (formattedItemLength < dataColumn.ColumnName.Length)
                {
                    formattedItemLength = dataColumn.ColumnName.Length;
                }
            }
            foreach (DataRow dataRow in dataTable.Rows)
            {
                foreach (var item in dataRow.ItemArray)
                {
                    if (formattedItemLength < item.ToString().Length)
                        formattedItemLength = item.ToString().Length;
                }
            }

            return formattedItemLength;
        }

        internal static void DoAction(int actionInt, string name)
        {
            switch (actionInt)
            {
                case 1:
                    //TODO: add a show
                    break;
                case 2:
                    //TODO: check if user has watched any shows before attempting to display shows.
                    //Could do this by having SqlDoer check if corelating UserID is present on UserShow.
                    DisplayUserShowsNRatings(name);
                    break;
                case 3:
                    //TODO: display all known shows
                    break;
                case 4:
                    //TODO: change your rating of a show
                    break;
                case 5:
                    //TODO: remove a show
                    break;
                case 6:
                    SayGoodbye();
                    break;
            }
        }

        internal static int GetValidActionInt()
        {
            int actionInt;
            string action;
            bool success;
            do
            {
                action = UIinator.AskAction();
                success = int.TryParse(action, out actionInt);
                if (actionInt < 1 || actionInt > 6)
                    success = false;
                if (!success)
                    Console.WriteLine("Please enter just a valid number");
            } while (!success);
            return actionInt;
        }

        internal static void SayGoodbye()
        {
            Console.WriteLine("Thanks for using this app!\r\nGoodbye!");
            Console.ReadLine();
        }
    }
}
