namespace TeisterMask.DataProcessor
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {

            var projects = context.Projects
                .ToArray()
                .Where(p => p.Tasks.Count > 0)
                .Select(x => new ExportProjetsDto
                {
                    HasEndDate = x.DueDate.HasValue ? "Yes" : "No",
                    ProjectName = x.Name,
                    TasksCount = x.Tasks.Count,
                    Tasks = x.Tasks.Select(x => new ExportTasksDto
                    {
                        Name = x.Name,
                        Label = x.LabelType.ToString(),
                    })
                    .OrderBy(x => x.Name)
                    .ToArray()
                })
                .OrderByDescending(x => x.TasksCount).ThenBy(x => x.ProjectName).ToArray();

            var xml = XmlConverter.Serialize(projects, "Projects");

            return xml;
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var employee = context.Employees
                .ToArray()
                .Where(x => x.EmployeesTasks.Any(x => x.Task.OpenDate >= date))
                 .Select(x => new
                 {
                     Username = x.Username,
                     Tasks = x.EmployeesTasks
                     .ToArray()
                     .Where(x => x.Task.OpenDate >= date)
                     .OrderByDescending(t => t.Task.DueDate)
                     .ThenBy(n => n.Task.Name)
                     .Select(t => new
                     {
                         TaskName = t.Task.Name,
                         OpenDate = t.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                         DueDate = t.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                         LabelType = t.Task.LabelType.ToString(),
                         ExecutionType = t.Task.ExecutionType.ToString()
                     })
                     .ToArray()
                 })
                 .OrderByDescending(x => x.Tasks.Length)
                 .ThenBy(x => x.Username)
                 .Take(10)
                 .ToArray();

            string employees = JsonConvert.SerializeObject(employee, Formatting.Indented);

            return employees;
        }
    }
}