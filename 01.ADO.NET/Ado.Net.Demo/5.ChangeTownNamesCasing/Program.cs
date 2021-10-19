using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace _5.ChangeTownNamesCasing
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
                string country = Console.ReadLine();
                string selectionCommandString = "UPDATE Towns SET Name = UPPER(Name) WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";
                SqlCommand command = new SqlCommand(selectionCommandString, connection);

                command.Parameters.Add(new SqlParameter("@countryName" , country));

                int affectedRows = command.ExecuteNonQuery();

                if (affectedRows > 0)
                {
                    Console.WriteLine($"{affectedRows} town names were affected.");

                    command = new SqlCommand($"SELECT t.Name FROM Towns as t JOIN Countries AS c ON c.Id = t.CountryCode WHERE c.Name = @countryName",connection);

                    command.Parameters.Add(new SqlParameter("@countryName",country));

                    List<string> towns = new List<string>();


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            towns.Add(reader[0].ToString());
                        }
                    }

                    Console.WriteLine($"[{string.Join(", ", towns)}]");
                }

                else
                {
                    Console.WriteLine("No town names were affected.");
                }
            }
        }
    }
}
