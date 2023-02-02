using System;
using System.Data;

namespace NewUserConsoleApp
{
    internal class Displayer
    {
        public static void DisplayTable(string tableName)
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
        public static void DisplayTable(string tableName, string query)
        {
            DataTable dataTable = CreateDatatableFromQuery(query);

            // get the highest length of an item in the table
            int formattedItemLength = getFormattedItemLength(dataTable);
            string formatedStringStructure = @"{0," + formattedItemLength + "}|";
            //Console.WriteLine(formattedItemLength);

            PrintTable(tableName, dataTable, formattedItemLength, formatedStringStructure);

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
    }
}