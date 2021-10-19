using Microsoft.Data.SqlClient;
using System;

namespace _9._IncreaseAgeStoredProcedure
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
                string selectionCommandString = @"CREATE PROC usp_GetOlder @id INT
                                                    AS
                                                    UPDATE Minions
                                                       SET Age += 1
                                                     WHERE Id = @id";
                SqlCommand command = new SqlCommand(selectionCommandString, connection);
                command.ExecuteNonQuery();

                int id = int.Parse(Console.ReadLine());

    

                command = new SqlCommand($"EXEC usp_GetOlder @id = {id}", connection);
                command.ExecuteNonQuery();
                command = new SqlCommand($"SELECT Name, Age FROM Minions WHERE Id = {id}", connection);

                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader[0]} - {reader[1]} years old");
                    }
                }
            }
        }
    }
}
