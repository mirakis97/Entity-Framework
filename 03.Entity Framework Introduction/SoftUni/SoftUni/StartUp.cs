using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var context = new SoftUniContext();
            using (context)
            {
                //Console.WriteLine(GetEmployeesFullInformation(context));
                //Console.WriteLine(GetEmployeesWithSalaryOver50000(context));
                //Console.WriteLine(GetEmployeesFromResearchAndDevelopment(context));
                //Console.WriteLine(AddNewAddressToEmployee(context));
                //Console.WriteLine(GetEmployeesInPeriod(context));
                //Console.WriteLine(GetAddressesByTown(context));
                //Console.WriteLine(GetEmployee147(context));
                //Console.WriteLine(GetDepartmentsWithMoreThan5Employees(context));
                //Console.WriteLine(GetLatestProjects(context));
                //Console.WriteLine(IncreaseSalaries(context));
                //Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(context));
                //Console.WriteLine(DeleteProjectById(context));
                Console.WriteLine(RemoveTown(context));
            }
        }
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var db = context.Employees.OrderBy(x => x.EmployeeId).ToList();
            StringBuilder sb = new StringBuilder();
            
            foreach (var employee in db)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:f2}");
            }

            return sb.ToString();
        }
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var db = context.Employees.Where(x => x.Salary > 50000).OrderBy(x => x.FirstName).ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var employee in db)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:F2}");
            }

            return sb.ToString();
        }
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var db = context.Employees.Select( e =>  new 
            { e.FirstName,
              e.LastName,
              e.Department,
              e.Salary
            }).Where(x => x.Department.Name == "Research and Development").OrderBy(x => x.Salary).ThenByDescending(x => x.FirstName).ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var employee in db)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.Department.Name} - ${employee.Salary:F2}");
            }

            return sb.ToString();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var addres = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            context.Addresses.Add(addres);

            var employee = context.Employees.Where(x => x.LastName == "Nakov").First();
            employee.Address = addres;
            context.SaveChanges();

            var db = context.Employees.OrderByDescending(e => e.AddressId).Take(10).ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var item in db)
            {
                sb.AppendLine($"{item.Address.AddressText}");
            }

            return sb.ToString();
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employee = context.Employees.Where(x => x.EmployeesProjects.Any(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003)).Take(10).Select(e => new 
            { 
                e.FirstName,
                e.LastName,
                e.Manager,
                Projects = e.EmployeesProjects.Select(ep => new 
                {
                    ProjectName = ep.Project.Name,
                    StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt",
                                CultureInfo.InvariantCulture),
                    
                    EndDate = ep.Project.EndDate.HasValue
                    ? ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt",
                                CultureInfo.InvariantCulture)
                    : "not finished"
                }).ToList()
                
            }).ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var item in employee)
            {
                sb.AppendLine($"{item.FirstName} {item.LastName} - Manager: {item.Manager.FirstName} {item.Manager.LastName}");
                foreach (var item2 in item.Projects)
                {
                    sb.AppendLine($"--{item2.ProjectName} - {item2.StartDate} - {item2.EndDate}");
                }
            }

            return sb.ToString();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addreses = context.Addresses.OrderByDescending(x => x.Employees.Count).ThenBy(x => x.Town.Name).ThenBy(x => x.AddressText).Take(10).Select(a => new
            {
                TownName = a.Town.Name,
                EmployeesCount = a.Employees.Count,
                AddressText = a.AddressText
            }).ToList();
            StringBuilder sb = new StringBuilder();

            foreach (var addres in addreses)
            {
                sb.AppendLine($"{addres.AddressText}, {addres.TownName} - {addres.EmployeesCount} employees");
            }

            return sb.ToString();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var employee = context.Employees.Select(e => new
            {
                EmployeeId = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                JobTitle = e.JobTitle,
                Projects = e.EmployeesProjects.Select(ep => new
                {
                    ProjectName = ep.Project.Name,
                }).OrderBy(x => x.ProjectName).ToList()
            }).Where(x => x.EmployeeId == 147).ToList();
            StringBuilder sb = new StringBuilder();

            foreach (var em in employee)
            {
                sb.AppendLine($"{em.FirstName} {em.LastName} - {em.JobTitle}");
                foreach (var p in em.Projects)
                {
                    sb.AppendLine($"{p.ProjectName}");
                }
            }

            return sb.ToString();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments.Where(x => x.Employees.Count > 5).OrderBy(x => x.Employees.Count).ThenBy(x => x.Name).Select(d => new 
            { 
                DepartmentName = d.Name,
                ManagerFirstName = d.Manager.FirstName , 
                ManagerLastName = d.Manager.LastName,
                Employees = d.Employees.Select(e => new 
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle
                }).ToList()
            }).ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var dep in departments)
            {
                sb.AppendLine($"{dep.DepartmentName} - {dep.ManagerFirstName} {dep.ManagerLastName}");
                foreach (var dep2 in dep.Employees)
                {
                    sb.AppendLine($"{dep2.FirstName} {dep2.LastName} - {dep2.JobTitle}");
                }
            }

            return sb.ToString();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects.OrderByDescending(p=> p.StartDate).Take(10).OrderBy(x => x.Name).Select(p => new 
            { 
                Name = p.Name,
                Description = p.Description,
                StartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)              
            }).ToList();
            StringBuilder sb = new StringBuilder();

            foreach (var project in projects)
            {
                sb.AppendLine($"{project.Name}");
                sb.AppendLine($"{project.Description}");
                sb.AppendLine($"{project.StartDate}");
            }
            return sb.ToString();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            var employees = context.Employees.Where(x => x.Department.Name == "Engineering" || x.Department.Name == "Tool Design" || x.Department.Name == "Marketing" || x.Department.Name == "Information Services").OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();

            foreach (var employee in employees)
            {
                employee.Salary *= 1.12m;
            }

            context.SaveChanges();

            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:F2})");
            }

            return sb.ToString();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees.Where(x => x.FirstName.StartsWith("Sa")).OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:F2})");
            }

            return sb.ToString();
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var projectToDelete = context.Projects
          .FirstOrDefault(p => p.ProjectId == 2);

            var employeeProject = context.EmployeesProjects
                .Where(p => p.EmployeeId == 2)
                .ToList();


            foreach (var project in employeeProject)
            {
                context.EmployeesProjects.Remove(project);
            }

            context.Projects.Remove(projectToDelete);

            context.SaveChanges();

            var result = new StringBuilder();

            var projects = context.Projects
                .Select(p => p.Name)
                .Take(10)
                .ToList();

            foreach (var project in projects)
            {
                result.AppendLine($"{project}");
            }

            return result.ToString().Trim();
        }

        public static string RemoveTown(SoftUniContext context)
        {
            var townToDelete = context.Towns
                 .FirstOrDefault(p => p.Name == "Seattle");

            var addresesToDelete = context.Addresses
                 .Where(p => p.TownId == townToDelete.TownId);

            var employeesAddresses = context.Employees.Where(x => addresesToDelete.Any(a => a.AddressId == x.AddressId ));

            var countOfAddressRemoved = addresesToDelete.Count();
            foreach (var employee in employeesAddresses)
            {
                employee.AddressId = null;
            }


            foreach (var addresess in addresesToDelete)
            {
                context.Addresses.Remove(addresess);
            }

            context.Towns.Remove(townToDelete);
            
            context.SaveChanges();

            return $"{countOfAddressRemoved} addresses in Seattle were deleted";
        }
    }
}
