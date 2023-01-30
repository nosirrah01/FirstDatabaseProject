//using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Configuration;
using System.Data.SqlClient;
//using SqlConnection = System.Data.SqlClient.SqlConnection;

namespace UserConsoleApp
{
    internal class Program
    {
        static string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=FirstDatabase;Integrated Security=True";
        static SqlConnection sqlconn = new SqlConnection(connectionString);
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[@"Data Source=localhost\SQLEXPRESS;Initial Catalog=FirstDatabase;Integrated Security=True"].ConnectionString;

            using (sqlconn)
            { 
                try
                {
                    sqlconn.Open();
                    SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand("");

                }
            }
        }
    }
}
