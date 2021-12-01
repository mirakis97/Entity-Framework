namespace TeisterMask.DataProcessor
{
    using System;
    using System.Linq;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            return "To DO"
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var prisoners = context.Employees
                .Where(x => x.EmployeesTasks.Any(x => x.Task.OpenDate >= date))
                 .Select(x => new
                 {
                     Username = x.Username,
                     Tasks = x.EmployeesTasks
                     .ToArray()
                     .Where(x => x.Task.OpenDate >= date)
                     .Select(t => new
                     {

                     })
                 }).ToList()
                 .OrderBy(x => x.Name)
                 .ThenBy(x => x.Id);

            string prisonersCells = JsonConvert.SerializeObject(prisoners, Formatting.Indented);

            return prisonersCells;
        }
    }
}