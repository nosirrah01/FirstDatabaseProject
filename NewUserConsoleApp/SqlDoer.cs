using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace NewUserConsoleApp
{
    internal class SqlDoer
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
    }
}
