namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            List<Play> playsToAdd = new List<Play>();
            var playsDtos = XmlConverter.Deserializer<ImportPlaysDto>(xmlString, "Plays");

            foreach (var playes in playsDtos)
            {
                if (!IsValid(playes))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var durationOfPlay = TimeSpan.ParseExact(playes.Duration, "c", CultureInfo.InvariantCulture);
                if (durationOfPlay.Hours < 01)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Genre genre;

                var isPositionValid = Enum.TryParse(playes.Genre, out genre);
                if (isPositionValid == false)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Play play = new Play()
                {
                    Title = playes.Title,
                    Duration = durationOfPlay,
                    Rating = playes.Rating,
                    Genre = genre,
                    Description = playes.Description,
                    Screenwriter = playes.Screenwriter
                };

                playsToAdd.Add(play);
                sb.AppendLine(string.Format(SuccessfulImportPlay, playes.Title, playes.Genre, playes.Rating));
            }

            context.Plays.AddRange(playsToAdd);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            List<Cast> castsToAdd = new List<Cast>();
            var castDtos = XmlConverter.Deserializer<ImportCastDto>(xmlString, "Casts");

            foreach (var casts in castDtos)
            {
                if (!IsValid(casts))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
            
                Cast cast = new Cast()
                {
                    FullName = casts.FullName,
                    IsMainCharacter = casts.IsMainCharacter,
                    PhoneNumber = casts.PhoneNumber,
                    PlayId = casts.PlayId,
                };
                string mainChar = " ";
                if (cast.IsMainCharacter == true)
                {
                    mainChar = "main";
                }
                else
                {
                    mainChar = "lesser";
                }

                castsToAdd.Add(cast);
                sb.AppendLine(string.Format(SuccessfulImportActor, casts.FullName, mainChar));
            }

            context.Casts.AddRange(castsToAdd);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            List<Theatre> theatresToAdd = new List<Theatre>();
            var theatresDtos = JsonConvert.DeserializeObject<IEnumerable<ImportTheatresDto>>(jsonString);

            foreach (var theatres in theatresDtos)
            {
                if (!IsValid(theatres))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Theatre theatre = new Theatre()
                {
                    Name = theatres.Name,
                    NumberOfHalls = (sbyte)theatres.NumberOfHalls,
                    Director = theatres.Director,
                };
                foreach (var ticket in theatres.Tickets)
                {
                    if (!IsValid(ticket))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Ticket ticket1 = new Ticket()
                    {
                        Price = ticket.Price,
                        RowNumber = (sbyte)ticket.RowNumber,
                        PlayId = ticket.PlayId
                    };
                    theatre.Tickets.Add(ticket1);
                }

                theatresToAdd.Add(theatre);
                sb.AppendLine(string.Format(SuccessfulImportTheatre, theatre.Name, theatre.Tickets.Count));
            }

            context.Theatres.AddRange(theatresToAdd);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
