using Microsoft.Data.SqlClient;
using System;

namespace _3._MinionNames
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
                string id = Console.ReadLine();
                string selectionCommandString = "SELECT Name FROM Villains WHERE Id = @Id";

                SqlCommand command = new SqlCommand(selectionCommandString, connection);

                command.Parameters.Add(new SqlParameter("@Id", id));

                string vilianName = (string)command.ExecuteScalar();

                if (vilianName != null)
                {
                    Console.WriteLine($"Villain: {vilianName}");
                }
                else
                {
                    Console.WriteLine($"No villain with ID {id} exists in the database.");
                    return;
                }

                command = new SqlCommand(@"SELECT ROW_NUMBER() OVER(ORDER BY m.Name) as RowNum,
                                         m.Name,
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = @Id
                                ORDER BY m.Name", connection);
                command.Parameters.Add(new SqlParameter("@Id",id));
                SqlDataReader reader = command.ExecuteReader();

                int counter = 0;
                using (reader)
                {
                    while (reader.Read())
                    {
                        counter++;

                        Console.WriteLine($"{counter}. {reader["Name"]} {reader["Age"]}");
                    }
                }

                if (counter == 0)
                {
                    Console.WriteLine($"(no minions)");
                }
            }
        }
    }
}
