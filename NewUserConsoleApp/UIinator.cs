using System;
using System.Data;

namespace NewUserConsoleApp
{
    internal class UIinator
    {
        public static string AskName()
        {
            Console.WriteLine("What is your name?");
            return Console.ReadLine();
        }

        internal static string AskQuestion(string question)
        {
            Console.WriteLine(question);
            return Console.ReadLine();
        }

        internal static void DisplayUserShowsNRatings(string name)
        {
            if(SqlDoer.UserHasWatchedShows(name))
            {
                DataTable userShowsNRatings = SqlDoer.GetUserShowsNRatings(name);
                for (int i = 0; i < userShowsNRatings.Rows.Count; i++)
                {
                    userShowsNRatings.Rows[i]["Your Rating"] += $"/10";
                }
                PrintTable("Shows You Have Watched", userShowsNRatings);
            } else
            {
                Console.WriteLine("You have not watched any shows yet.");
            }
            

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
                    AskShowInfo(name);
                    break;
                case 2:
                    DisplayUserShowsNRatings(name);
                    break;
                case 3:
                    DisplayKnownShowsAndAverageRatings();
                    break;
                case 4:
                    ChangeShowRating(name);
                    break;
                case 5:
                    RemoveShow(name);
                    break;
                case 6:
                    SayGoodbye();
                    break;
            }
        }

        private static void RemoveShow(string username)
        {
            Console.WriteLine("Enter the name of the show would you like to remove:");
            string showName = Console.ReadLine();

            if (SqlDoer.WatchedShow(username, showName))
            {
                //remove row from usershow
                SqlDoer.RemoveFromUserShow(username, showName);
                //check if show is still present in UserShow
                if (!SqlDoer.ShowIsWatched(showName))
                {
                    SqlDoer.RemoveFromShow(showName);
                }
                Console.WriteLine($"{showName} was removed from your watched shows.");
            }
            else
            {
                Console.WriteLine($"{showName} is not in your list of watched shows.");
            }

        }

        private static void ChangeShowRating(string name)
        {
            Console.WriteLine("Enter the name of the show would you like to change your rating of:");
            string showName = Console.ReadLine();
            if (SqlDoer.WatchedShow(name, showName))
            {
                int rating = GetValidInt(10, "What would you rate the show out of 10?");
                SqlDoer.UpdateRating(name, showName, rating);
                Console.WriteLine($"Rating of {showName} updated to {rating}/10");
            } else
            {
                Console.WriteLine($"{showName} is not in your list of watched shows.");
            }
        }

        private static void DisplayKnownShowsAndAverageRatings()
        {
            if (SqlDoer.TableHasRows("Show"))
            {
                DataTable showsAverageRatings = SqlDoer.GetKnownShowsAverageRatings();
                
                for (int i = 0; i < showsAverageRatings.Rows.Count; i++)
                {
                    double roundedRating = double.Parse(showsAverageRatings.Rows[i]["Average Rating"].ToString());
                    showsAverageRatings.Rows[i]["Average Rating"] = $"{roundedRating}/10";
                }
                
                PrintTable("Shows Known by This Application", showsAverageRatings);
            }
            else
            {
                Console.WriteLine("There are no shows known to this app yet.");
            }

        }
        private static void AskShowInfo(string username)
        {
            Console.WriteLine("What is the name of the show?");
            string showName = Console.ReadLine();
            int rating = GetValidInt(10, "What would you rate the show out of 10?");
            if(!SqlDoer.ValueIsInColumn(showName, "Show", "Name"))
                SqlDoer.AddToShow(showName);
            SqlDoer.AddToUserShow(username, showName, rating);
            Console.WriteLine($"{showName} added to your watched shows.");
        }

        internal static int GetValidInt(int max, string question)
        {
            int validInt;
            string answer;
            bool success;
            do
            {
                answer = UIinator.AskQuestion(question);
                success = int.TryParse(answer, out validInt);
                if (validInt < 1 || validInt > max)
                    success = false;
                if (!success)
                    Console.WriteLine("Please enter just a valid number");
            } while (!success);
            return validInt;
        }

        internal static void SayGoodbye()
        {
            Console.WriteLine("Thanks for using this app!\r\nGoodbye!");
            Console.ReadLine();
        }

        internal static string GetActionQuestion()
        {
            return "What would you like to do?\r\nEnter '1' to add a show\r\n'2' display your watched shows\r\n'3' display all known shows\r\n'4' to change your rating of a show\r\n'5' to remove a show\r\n'6' to quit the application";
        }
    }
}
