using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _7._PrintAllMinionNames
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=.; Database=MinionsDB; Trusted_Connection=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (connection)
            {
                List<string> minionsNames = new List<string>();
                string selectionCommandString = "SELECT Name FROM Minions";
                SqlCommand command = new SqlCommand(selectionCommandString, connection);
                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        minionsNames.Add(reader[0].ToString());
                    }
                }

                int counter = 0;

                while (minionsNames.Any())
                {
                    if (counter % 2 == 0)
                    {
                        Console.WriteLine(minionsNames[0]);

                        minionsNames.RemoveAt(0);
                    }
                    else
                    {
                        Console.WriteLine(minionsNames[minionsNames.Count - 1]);
                        minionsNames.RemoveAt(minionsNames.Count - 1);
                    }

                    counter++;
                }
            }
        }
    }
}
