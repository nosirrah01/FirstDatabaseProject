using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections;


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

    }
}
