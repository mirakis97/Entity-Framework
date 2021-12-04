namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Globalization;
    using System.Linq;
    using Theatre.Data;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var tickets = context.Theatres
                .ToArray()
                .Where(x => x.NumberOfHalls >= numbersOfHalls && x.Tickets.Count >= 20)
                 .Select(x => new
                 {
                     Name = x.Name,
                     Halls = x.NumberOfHalls,
                     TotalIncome = x.Tickets.Where(r => r.RowNumber >= 1 && r.RowNumber <= 5).Sum(t => t.Price),
                     Tickets = x.Tickets.Where(r => r.RowNumber >= 1 && r.RowNumber <= 5).Select( x => new
                     {
                         Price = x.Price,
                         RowNumber = x.RowNumber
                     })
                     .OrderByDescending(x => x.Price)
                     .ToArray()
                 })
                 .OrderByDescending(x => x.Halls)
                 .ThenBy(x => x.Name)
                 .ToArray();

            string theatres = JsonConvert.SerializeObject(tickets, Formatting.Indented);

            return theatres;
        }

        public static string ExportPlays(TheatreContext context, double rating)
        {
           
            var playesDto = context.Plays
           .ToArray()
           .Where(x => x.Rating <= rating)
           .Select(x => new ExportPlayesDTO
           {
               Title = x.Title,
               Duration = x.Duration.ToString("c", CultureInfo.InvariantCulture),
               Rating = x.Rating.ToString(),
               Genre = x.Genre.ToString(),
               Actors = x.Casts.Where(ch => ch.IsMainCharacter == true).Select(x => new ExportActorsDto 
               {
                    FullName = x.FullName,
                    MainCharacter = $"Plays main character in '{x.Play.Title}'."
               })
               .OrderByDescending(x => x.FullName)
               .ToArray()

           })
           .OrderBy(x => x.Title)
           .ThenByDescending(x => x.Genre)
           .ToArray();
            foreach (var data in playesDto)
            {
                if (data.Rating == "0")
                {
                    data.Rating = "Premier";
                }
            }
            var playes = XmlConverter.Serialize(playesDto, "Plays");

            return playes;
        }
    }
}
