using Microsoft.Data.SqlClient;
using System;

namespace _6.RemoveVillain
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
                int villianId = int.Parse(Console.ReadLine());
                string selectionCommandString = "SELECT COUNT(*) FROM Villains WHERE Id = @villainId";
                SqlCommand command = new SqlCommand(selectionCommandString, connection);
                command.Parameters.Add(new SqlParameter("@villainId", villianId));

                int count = (int)command.ExecuteScalar();

                if (count == 0)
                {
                    Console.WriteLine($"No such villain was found.");
                }
                else
                {
                    command = new SqlCommand("SELECT Name FROM Villains WHERE Id = @villainId", connection);
                    command.Parameters.Add(new SqlParameter("@villainId",villianId));

                    string name = command.ExecuteScalar().ToString();

                    command = new SqlCommand(@"DELETE FROM MinionsVillains 
                                                WHERE VillainId = @villainId", connection);
                    command.Parameters.Add(new SqlParameter("@villainId", villianId));

                    int affectedRows = command.ExecuteNonQuery();

                    command = new SqlCommand(@"DELETE FROM Villains
                                                 WHERE Id = @villainId", connection);

                    command.Parameters.Add(new SqlParameter("@villainId", villianId));

                    command.ExecuteNonQuery();

                    Console.WriteLine($"{name} was deleted.");
                    Console.WriteLine($"{affectedRows} minions was released.");
                }

            }
        }
    }
}
