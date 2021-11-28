namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Linq;
    using static SoftJail.DataProcessor.ExportDto.ExportPrisonerDto;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners
                .ToList()
                .Where(x => ids.Contains(x.Id))
                 .Select(x => new
                 {
                     Id = x.Id,
                     Name = x.FullName,
                     CellNumber = x.Cell.CellNumber,
                     Officers = x.PrisonerOfficers.Select(o => new
                     {
                         OfficerName = o.Officer.FullName,
                         Department = o.Officer.Department.Name
                     })
                     .ToList()
                     .OrderBy(o => o.OfficerName),
                     TotalOfficerSalary = decimal.Parse(x.PrisonerOfficers.Sum(s => s.Officer.Salary).ToString("f2"))
                 }).ToList()
                 .OrderBy(x => x.Name)
                 .ThenBy(x => x.Id);

            string prisonersCells = JsonConvert.SerializeObject(prisoners, Formatting.Indented);

            return prisonersCells;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            var prisonersName = prisonersNames.Split(",");

            var prsoners = context.Prisoners
                .Where(p => prisonersName.Contains(p.FullName))
                .Select(x => new ExportPrisonerDto
                {
                    Id = x.Id,
                    Name = x.FullName,
                    IncarcerationDate = x.IncarcerationDate.ToString("yyyy-MM-dd"),
                    EncryptedMessages = x.Mails.Select(p => new Message
                    {
                        Description = string.Join("",p.Description.Reverse())
                    }).ToArray()
                })
                .OrderBy(x => x.Name).ThenBy(x=>x.Id).ToList();

            var xml = XmlConverter.Serialize(prsoners, "Prisoners");

            return xml;
        }
    }
}