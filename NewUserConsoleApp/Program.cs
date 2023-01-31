using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using NewUserConsoleApp.FirstDatabaseDataSetTableAdapters;

namespace NewUserConsoleApp
{
    internal class Program
    {
        static SqlConnection sqlConnection;

        static void Main(string[] args)
        {
            Console.WriteLine("This line will appear in the branch!");

            Console.WriteLine("What would you like to do?");
            Console.WriteLine("Enter 1 for add user");
            Console.WriteLine("Enter 2 to display tables");
            string connectionString = ConfigurationManager.ConnectionStrings["NewUserConsoleApp.Properties.Settings.FirstDatabaseConnectionString"].ConnectionString;

            sqlConnection = new SqlConnection(connectionString);

            DisplayTable("User");
            DisplayTable("Show");
            DisplayTable("UserShow");
            DisplayTable("User 1's shows", @"select s.Name from Show s inner join UserShow us on s.ShowId = us.ShowID where us.UserID = 1");
            Console.Read();
        }
        private static void DisplayTable(string tableName)
        {
            //string query = isByTableName ? $"select * from [{tableName}]" : tableName;
            string query = $"select * from [{tableName}]";
            DataTable dataTable = CreateDatatableFromQuery(query);

            // get the highest length of an item in the table
            int formattedItemLength = getFormattedItemLength(dataTable);
            string formatedStringStructure = @"{0," + formattedItemLength + "}|";
            //Console.WriteLine(formattedItemLength);

            PrintTable(tableName, dataTable, formattedItemLength, formatedStringStructure);

        }

        private static DataTable CreateDatatableFromQuery(string query)
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

        private static void DisplayTable(string tableName, string query)
        {
            DataTable dataTable = CreateDatatableFromQuery(query);

            // get the highest length of an item in the table
            int formattedItemLength = getFormattedItemLength(dataTable);
            string formatedStringStructure = @"{0," + formattedItemLength + "}|";
            //Console.WriteLine(formattedItemLength);

            PrintTable(tableName, dataTable, formattedItemLength, formatedStringStructure);

        }

        private static void PrintTable(string tableName, DataTable dataTable, int formattedItemLength, string formatedStringStructure)
        {
            // print the table
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
    }
}
