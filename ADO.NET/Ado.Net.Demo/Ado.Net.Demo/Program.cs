using Microsoft.Data.SqlClient;
using System;

namespace Ado.Net.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=.; Database=SoftUni; Trusted_Connection=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (connection)
            {
                string selectionCommandString = "SELECT * FROM Employees";
                SqlCommand command = new SqlCommand(selectionCommandString, connection);
                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write($"{reader[i]} ");
                        }
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}

