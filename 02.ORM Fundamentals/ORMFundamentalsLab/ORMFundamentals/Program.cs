using ORMFundamentals.Models;
using System;
using System.Linq;

namespace ORMFundamentals
{
    class Program
    {
        static void Main(string[] args)
        {

            var db = new SoftUniContext();

            var employees = db.Employees.Where(x => x.JobTitle == "Design Engineer").OrderBy(x => x.JobTitle).ToList();

            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.FirstName} {employee.LastName} => {employee.Salary}");
            }

        }

        public string CreatingEmployee()
        {
            var database = new SoftUniContext();

            database.Employees.Add(new Employee
            {
                FirstName = "Miroslav",
                LastName = "Vasilev",
                Salary = 12000,
                JobTitle = "Junior C# Developer",
                HireDate = new DateTime(2015, 12, 31, 5, 10, 20, DateTimeKind.Utc),
                DepartmentId = 1

            });

            database.SaveChanges();
            return "";
        }
    }
}
