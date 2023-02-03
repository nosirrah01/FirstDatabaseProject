using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections;
using System.Xml.Linq;


namespace NewUserConsoleApp
{
    public class SqlDoer
    {
        //static SqlConnection sqlConnection;

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
                //DataTable dataTable = new DataTable();

                sqlDataAdapter.Fill(dataTable);

            }

            return dataTable;
        }

        public static void UpdateDatabaseWithQuery(string query)
        {
            /*
            sqlConnection.Open();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.InsertCommand = new SqlCommand();
            */
            // SqlDataAdapter can be imagined like an Interface to make Tables usable by C#-Objects
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
            // trying moving dataTable outside of the using statement.
            DataTable dataTable = new DataTable();
            using (sqlDataAdapter)
            {
                //DataTable dataTable = new DataTable();
                //sqlDataAdapter.SelectCommand = new SqlCommand(query, sqlConnection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(sqlDataAdapter);
                sqlDataAdapter.Fill(dataTable);

                dataTable.Rows.Add(new Object[] { 1, "Smith" });

                sqlDataAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                sqlDataAdapter.Update(dataTable);
            }

        }

        internal static void AddNameToUser(string name)
        {
            //TODO: add name to User
            throw new NotImplementedException();
        }

        internal static DataTable GetUserShowsNRatings(string name)
        {
            string userIdTableQuery = $"select [UserId] from [User] where [Username] = '{name}'";
            DataTable selectedUserIdTable = CreateDatatableFromQuery(userIdTableQuery);
            //TODO: fix this query
            string userShowsNRatingsQuery = $"select s.Name from Show s inner join UserShow us on s.ShowId = us.ShowID where us.UserID = {selectedUserIdTable.Rows[0][0]}";
            return CreateDatatableFromQuery(userShowsNRatingsQuery);
        }

        internal static bool NameIsInUser(string name)
        {
            //TODO: Check if Name is in the table User
            return true;
            
        }
    }
}
