using Microsoft.Data.SqlClient;
using System;

namespace _2.VillainNames
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
                string selectionCommandString = "SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount FROM Villains AS v JOIN MinionsVillains AS mv ON v.Id = mv.VillainId  GROUP BY v.Id, v.Name  HAVING COUNT(mv.VillainId) > 3  ORDER BY COUNT(mv.VillainId)";
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
