namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            List<Department> departments = new List<Department>();
            var departmetsCells = JsonConvert.DeserializeObject<IEnumerable<DepartmetsCellsDTO>>(jsonString);

            foreach(var departmetCell in departmetsCells)
            {
                if (!IsValid(departmetCell)|| !departmetCell.Cells.All(IsValid) || departmetCell.Cells.Count == 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                Department department = new Department()
                {
                    Name = departmetCell.Name,
                    Cells = departmetCell.Cells.Select(x => new Cell
                    {
                        CellNumber = x.CellNumber,
                        HasWindow = x.HasWindow
                    })
                    .ToList()
                };

                departments.Add(department);
                sb.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
            }

            context.Departments.AddRange(departments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            List<Prisoner> prisoners = new List<Prisoner>();
            var prisonersMails = JsonConvert.DeserializeObject<IEnumerable<ImportPrisonersMails>>(jsonString);

            foreach (var prisonersMail in prisonersMails)
            {
                if (!IsValid(prisonersMail) || !prisonersMail.Mails.All(IsValid))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var IsRelaseDateValid = DateTime.TryParseExact(prisonersMail.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,DateTimeStyles.None,out DateTime relaseDate);
                var incarcerationDate = DateTime.ParseExact(prisonersMail.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                Prisoner prisoner = new Prisoner()
                {
                    FullName = prisonersMail.FullName,
                    Nickname = prisonersMail.Nickname,
                    Age = prisonersMail.Age,
                    IncarcerationDate = incarcerationDate,
                    ReleaseDate = IsRelaseDateValid ? (DateTime?)relaseDate : null,
                    Bail = prisonersMail.Bail,
                    CellId = prisonersMail.CellId,
                    Mails = prisonersMail.Mails.Select(x => new Mail
                    {
                        Description = x.Description,
                        Sender = x.Sender,
                        Address = x.Address
                    })
                    .ToList()
                };

                prisoners.Add(prisoner);
                sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            List<Officer> officers = new List<Officer>();
            var officersPrisoners = XmlConverter.Deserializer<ImportOfficersPrisonersDTO>(xmlString,"Officers");

            foreach (var officerPrisoner in officersPrisoners)
            {
                if (!IsValid(officerPrisoner) || !officerPrisoner.Prisoners.All(IsValid))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                Position position;

                var isPositionValid = Enum.TryParse(officerPrisoner.Position, out position);

                Weapon weapon;

                var isWeaponValid = Enum.TryParse(officerPrisoner.Weapon, out weapon);
                Officer officer = new Officer()
                {
                    FullName = officerPrisoner.Name,
                    Salary = officerPrisoner.Money,
                    Position = Enum.Parse<Position>(officerPrisoner.Position),
                    Weapon = Enum.Parse<Weapon>(officerPrisoner.Weapon),
                    DepartmentId = officerPrisoner.DepartmentId,
                    OfficerPrisoners = officerPrisoner.Prisoners.Select(x => new OfficerPrisoner
                    {
                        PrisonerId = x.Id
                    }).ToArray()
                };

                officers.Add(officer);
                sb.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");
            }

            context.Officers.AddRange(officers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}