using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _8._IncreaseMinionAge
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
                string selectionCommandString = @"UPDATE Minions
                                                 SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                                                 WHERE Id = @Id";
                SqlCommand command = new SqlCommand(selectionCommandString, connection);

                int[] ids = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse).ToArray();

                foreach (var id in ids)
                {
                    SqlParameter parameter = new SqlParameter("@Id", id);

                    command.Parameters.Add(parameter);
                    command.ExecuteNonQuery();
                    command.Parameters.Remove(parameter);
                }

                command = new SqlCommand("SELECT Name, Age FROM Minions", connection);
                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        minionsNames.Add(reader[0].ToString() + " " + reader[1].ToString());
                    }
                }

                Console.WriteLine(string.Join(Environment.NewLine, minionsNames));
            }
        }
    }
}
