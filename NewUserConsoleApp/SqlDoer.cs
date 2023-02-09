using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace NewUserConsoleApp
{
    public class SqlDoer
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["NewUserConsoleApp.Properties.Settings.FirstDatabaseConnectionString"].ConnectionString;
        static SqlConnection sqlConnection = new SqlConnection(connectionString);

        public static DataTable CreateDatatableFromQuery(string query)
        {
            // SqlDataAdapter can be imagined like an Interface to make Tables usable by C#-Objects
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
            // trying moving dataTable outside of the using statement.
            DataTable dataTable = new DataTable();
            using (sqlDataAdapter)
            {
                sqlDataAdapter.Fill(dataTable);
            }
            return dataTable;
        }

        internal static void AddNameToUser(string name)
        {
            Object[] newRow = new Object[] { 1, name };
            AddRowToTable(newRow, "User");
        }

        private static void AddRowToTable(object[] newRow, string tableName)
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter($"select * from [{tableName}];", sqlConnection);
            DataTable dataTable = new DataTable();
            using (sqlDataAdapter)
            {
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(sqlDataAdapter);
                sqlDataAdapter.Fill(dataTable);

                dataTable.Rows.Add(newRow);

                sqlDataAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                sqlDataAdapter.Update(dataTable);
            }
        }

        internal static DataTable GetUserShowsNRatings(string name)
        {
            return CreateDatatableFromQuery($"select s.Name as 'Show Name', CAST(us.Rating as varchar(50)) as 'Your Rating' from Show s inner join UserShow us on s.ShowId = us.ShowID where us.UserID = {CreateDatatableFromQuery($"select [UserId] from [User] where [Username] = '{name}'").Rows[0][0]}");
        }

        internal static bool ValueIsInColumn(string value, string tableName, string columnName)
        {
            bool isIn = false;
            if(TableHasRows($"{tableName}"))
            {
                DataTable columnValues = CreateDatatableFromQuery($"select [{columnName}] from [{tableName}]");
                foreach (DataRow dataRow in columnValues.Rows)
                {
                    foreach (var item in dataRow.ItemArray)
                    {
                        if (item.ToString().Equals(value))
                        {
                            isIn = true;
                        }
                        
                    }
                }
            }
            return isIn;
        }

        private static bool ValueIsInColumn(string value, DataTable columnValues)
        {
            bool isIn = false;
            foreach (DataRow dataRow in columnValues.Rows)
            {
                foreach (var item in dataRow.ItemArray)
                {
                    if (item.ToString().Equals(value))
                    {
                        isIn = true;
                    }

                }
            }
            return isIn;
        }

        internal static bool TableHasRows(string tableName)
        {
            int count = int.Parse(CreateDatatableFromQuery($"SELECT COUNT(*) as 'UserCount' from [{tableName}]").Rows[0][0].ToString());
            bool hasRows = (count != 0);
            return hasRows;
        }

        internal static bool UserHasWatchedShows(string name)
        {
            DataTable selectedUserIdTable = CreateDatatableFromQuery($"select [UserId] from [User] where [Username] = '{name}'");
            DataTable userIdsThatHaveShows = CreateDatatableFromQuery($"select [UserID] from [UserShow];");
            bool hasWatched = false;
            foreach (DataRow dataRow in userIdsThatHaveShows.Rows)
            {
                foreach (var item in dataRow.ItemArray)
                {
                    if (item.ToString().Equals(selectedUserIdTable.Rows[0][0].ToString()))
                    {
                        hasWatched = true;
                    }

                }
            }
            return hasWatched;
        }

        internal static void AddToShow(string showName)
        {
            Object[] newRow = new Object[] { 1, showName };
            AddRowToTable(newRow, "Show");
        }

        internal static void AddToUserShow(string userName, string showName, int rating)
        {
            int userID = int.Parse(CreateDatatableFromQuery($"select [UserId] from [User] where [Username] = '{userName}'").Rows[0][0].ToString());
            int showID = int.Parse(CreateDatatableFromQuery($"select [ShowId] from [Show] where [Name] = '{showName}'").Rows[0][0].ToString());
            Object[] newRow = new Object[] { 1, userID, showID, rating};
            AddRowToTable(newRow, "UserShow");
        }

        internal static DataTable GetKnownShowsAverageRatings()
        {
            return CreateDatatableFromQuery("SELECT s.[Name] as 'Show Name', CAST(ROUND(AVG(CAST(us.[Rating] as decimal(4,2))), 1, 1) as varchar(50)) as 'Average Rating' FROM Show s INNER JOIN [UserShow] us on s.[ShowId] = us.[ShowID] GROUP BY s.[Name];");
        }

        internal static bool WatchedShow(string userName, string showName)
        {
            bool watched = false;
            int count = int.Parse(CreateDatatableFromQuery($"SELECT COUNT(*) as 'ShowCount' from [UserShow] where [UserID] = {CreateDatatableFromQuery($"select [UserId] from [User] where [Username] = '{userName}'").Rows[0][0]}").Rows[0][0].ToString());
            if (count != 0)
            {
                if (ValueIsInColumn(showName, GetWatchedShowsDatatable(userName)))
                    watched = true;
            }
            return watched;
        }

        private static DataTable GetWatchedShowsDatatable(string userName)
        {
            return CreateDatatableFromQuery($"select s.Name as 'Show Name' from Show s inner join UserShow us on s.ShowId = us.ShowID where us.UserID = {CreateDatatableFromQuery($"select [UserId] from [User] where [Username] = '{userName}'").Rows[0][0]};");
        }

        internal static void UpdateRating(string userName, string showName, int rating)
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter($"select * from [UserShow];", sqlConnection);
            DataTable dataTable = new DataTable();
            using (sqlDataAdapter)
            {
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(sqlDataAdapter);
                sqlDataAdapter.Fill(dataTable);
                int showID = int.Parse(CreateDatatableFromQuery($"select [ShowId] from [Show] where [Name] = '{showName}';").Rows[0][0].ToString());
                int userID = int.Parse(CreateDatatableFromQuery($"select [UserId] from [User] where [Username] = '{userName}';").Rows[0][0].ToString());
                DataTable userShow = CreateDatatableFromQuery("select * from [UserShow]");
                int rowIndex = 0;
                for (int i = 0; i < userShow.Rows.Count; i++)
                {
                    if (userShow.Rows[i][2].Equals(showID) && userShow.Rows[i][1].Equals(userID))
                        rowIndex = i;
                }
                dataTable.Rows[rowIndex]["Rating"] = rating;
                sqlDataAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                sqlDataAdapter.Update(dataTable);
            }
        }

        internal static void RemoveFromUserShow(string username, string showName)
        {
            int userID = int.Parse(CreateDatatableFromQuery($"select [UserId] from [User] where [Username] = '{username}'").Rows[0][0].ToString());
            int showID = int.Parse(CreateDatatableFromQuery($"select [ShowId] from [Show] where [Name] = '{showName}'").Rows[0][0].ToString());
            int rating = int.Parse(CreateDatatableFromQuery($"select [Rating] from [UserShow] where UserID = {userID} and ShowID = {showID};").Rows[0][0].ToString());
            DataTable userShow = CreateDatatableFromQuery("select * from [UserShow]");
            int rowIndex = 0;
            for (int i = 0; i < userShow.Rows.Count; i++)
            {
                if (userShow.Rows[i][2].Equals(showID) && userShow.Rows[i][1].Equals(userID))
                    rowIndex = i;
            }
            Object[] rowToRemove = new Object[] { 1, userID, showID, rating };
            RemoveRowFromTable(rowIndex, "UserShow");
        }

        private static void RemoveRowFromTable(int rowIndex, string tableName)
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter($"select * from [{tableName}];", sqlConnection);
            DataTable dataTable = new DataTable();
            using (sqlDataAdapter)
            {
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(sqlDataAdapter);
                sqlDataAdapter.Fill(dataTable);
                dataTable.Rows[rowIndex].Delete();
                sqlDataAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                sqlDataAdapter.Update(dataTable);
            }
        }

        internal static bool ShowIsWatched(string showName)
        {
            string showIDString = CreateDatatableFromQuery($"select [ShowId] from [Show] where [Name] = '{showName}'").Rows[0][0].ToString();
            bool isWatched = false;
            if (TableHasRows("UserShow"))
            {
                if (ValueIsInColumn(showIDString, "UserShow", "ShowID"))
                    isWatched = true;
            }
            return isWatched;
        }

        internal static void RemoveFromShow(string showName)
        {
            DataTable showTable = CreateDatatableFromQuery("select * from [Show]");
            int rowIndex = 0;
            for (int i = 0; i < showTable.Rows.Count; i++)
            {
                if (showTable.Rows[i][1].ToString().Equals(showName))
                    rowIndex = i;
            }
            RemoveRowFromTable(rowIndex, "Show");
        }
    }
}
