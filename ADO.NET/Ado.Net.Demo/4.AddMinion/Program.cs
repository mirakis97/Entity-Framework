using Microsoft.Data.SqlClient;
using System;

namespace _4.AddMinion
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] minionData = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);

            string minionName = minionData[1];
            int minionAge = int.Parse(minionData[2]);
            string minionCity = minionData[3];

            string vilianName = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries)[1];

            string connectionString = "Server=.; Database=MinionsDB; Trusted_Connection=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (connection)
            {
                string creationCommandString = $"SELECT COUNT(*) FROM Towns WHERE Name = @townName";
                SqlCommand command = new SqlCommand(creationCommandString, connection);
                command.Parameters.Add(new SqlParameter("@townName", minionCity));

                int count = (int)command.ExecuteScalar();

                if (count == 0)
                {
                    command = new SqlCommand("INSERT INTO Towns (Name) VALUES (@townName)", connection);
                    command.Parameters.Add(new SqlParameter("@townName", minionCity));

                    command.ExecuteNonQuery();

                    Console.WriteLine($"Town {minionCity} was added to the database.");
                }

                command = new SqlCommand($"SELECT COUNT(*) FROM Villains WHERE Name = @Name", connection);
                command.Parameters.Add(new SqlParameter("@Name", vilianName));

                count = (int)command.ExecuteScalar();

                if (count == 0)
                {
                    command = new SqlCommand("INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)", connection);
                    command.Parameters.Add(new SqlParameter("@villainName", vilianName));

                    command.ExecuteNonQuery();

                    Console.WriteLine($"Villain {vilianName} was added to the database.");
                }

                command = new SqlCommand("SELECT Id FROM Towns WHERE Name = @townName", connection);
                command.Parameters.Add(new SqlParameter("@townName", minionCity));
                int townId = (int)command.ExecuteScalar();

                command = new SqlCommand("SELECT Id FROM Villains WHERE Name = @Name", connection);
                command.Parameters.Add(new SqlParameter("@Name", vilianName));
                int vilianId = (int)command.ExecuteScalar();

                command = new SqlCommand($"SELECT COUNT(*) FROM Minions WHERE Name = @Name", connection);
                command.Parameters.Add(new SqlParameter("@Name", minionName));

                count = (int)command.ExecuteScalar();

                if (count == 0)
                {
                    command = new SqlCommand($"INSERT INTO Minions (Name, Age, TownId) VALUES (@nam, @age, @townId)", connection);

                    command.Parameters.Add(new SqlParameter("@nam", minionName));
                    command.Parameters.Add(new SqlParameter("@age", minionAge));
                    command.Parameters.Add(new SqlParameter("@townId", townId));

                    command.ExecuteNonQuery();

                }

                command = new SqlCommand("SELECT Id FROM Minions WHERE Name = @Name", connection);
                command.Parameters.Add(new SqlParameter("@Name", minionName));
                int minionId = (int)command.ExecuteScalar();


                command = new SqlCommand($"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@villainId, @minionId)", connection);
                command.Parameters.Add(new SqlParameter("@villainId", vilianId));
                command.Parameters.Add(new SqlParameter("@minionId", minionId));

                command.ExecuteNonQuery();

                Console.WriteLine($"Successfully added {minionName} to be minion of {vilianName}.");

            }
        }
    }
}
