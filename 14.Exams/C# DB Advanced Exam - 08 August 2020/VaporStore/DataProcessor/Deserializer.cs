namespace VaporStore.DataProcessor
{
	using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
	{
		public static string ImportGames(VaporStoreDbContext context, string jsonString)
		{
			var sb = new StringBuilder();

			var deserializedGames = JsonConvert.DeserializeObject<ImportGamesDvelopers[]>(jsonString);

			var developers = new List<Developer>();
			var genres = new List<Genre>();
			var tags = new List<Tag>();
			var games = new List<Game>();

			foreach (var gameDto in deserializedGames)
			{
				if (!IsValid(gameDto) || !gameDto.Tags.All(IsValid))
				{
					sb.AppendLine("Invalid Data");
					continue;
				}

				var developer = developers.SingleOrDefault(d => d.Name == gameDto.Developer);
				if (developer == null)
				{
					developer = new Developer()
					{
						Name = gameDto.Developer
					};
					developers.Add(developer);
				}


				var genre = genres.SingleOrDefault(g => g.Name == gameDto.Genre);
				if (genre == null)
				{
					genre = new Genre()
					{
						Name = gameDto.Genre
					};
					genres.Add(genre);
				}

				var gameTags = new List<Tag>();
				foreach (var tagName in gameDto.Tags)
				{
					var tag = tags.SingleOrDefault(t => t.Name == tagName);
					if (tag == null)
					{
						tag = new Tag() { Name = tagName };
						tags.Add(tag);
					}

					gameTags.Add(tag);
				}
				var IsRelaseDateValid = DateTime.TryParseExact(gameDto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime relaseDate);
				var game = new Game()
				{
					Name =	gameDto.Name,
					Price = gameDto.Price,
					ReleaseDate = relaseDate,
					Developer = developer,
					Genre = genre,
					GameTags = gameTags.Select(t => new GameTag { Tag = t }).ToArray()
				};

				games.Add(game);

				sb.AppendLine($"Added {gameDto.Name} ({gameDto.Genre}) with {gameDto.Tags.Count} tags");
			}

			context.Games.AddRange(games);

			context.SaveChanges();

			var result = sb.ToString();

			return result;
		}

		public static string ImportUsers(VaporStoreDbContext context, string jsonString)
		{
			StringBuilder sb = new StringBuilder();
			List<User> users = new List<User>();
			var importUsers = JsonConvert.DeserializeObject<IEnumerable<ImportUsers>>(jsonString);

			foreach (var importUser in importUsers)
			{
				if (!IsValid(importUser) || !importUser.Cards.All(IsValid))
				{
					sb.AppendLine("Invalid Data");
					continue;
				}
				
				User user = new User()
				{
					FullName = importUser.FullName,
					Username = importUser.Username,
					Email = importUser.Email,
					Age = importUser.Age,
					Cards = importUser.Cards.Select(x => new Card
                    {
						Number = x.Number,
						Cvc = x.CVC,
						Type = Enum.Parse<CardType>(x.Type)
                    }).ToArray()
				};

				users.Add(user);
				sb.AppendLine($"Imported {importUser.Username} with {importUser.Cards.Count} cards");
			}

			context.Users.AddRange(users);
			context.SaveChanges();

			return sb.ToString().TrimEnd();
		}

		public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
		{

			StringBuilder sb = new StringBuilder();
			List<Purchase> purchases = new List<Purchase>();
			var purchesesImport = XmlConverter.Deserializer<ImportPurchases>(xmlString, "Purchases");

			foreach (var importPurches in purchesesImport)
			{
				if (!IsValid(importPurches))
				{
					sb.AppendLine("Invalid Data");
					continue;
				}
				var game = context.Games.Single(g => g.Name == importPurches.Title);
				var card = context.Cards.Include(c => c.User).Single(c => c.Number == importPurches.Card);
				var date = DateTime.ParseExact(importPurches.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

				Purchase purchase = new Purchase()
				{
					Game = game,
					Type = Enum.Parse <PurchaseType>(importPurches.Type),
					ProductKey = importPurches.Key,
					Card = card,
					Date = date
				};

				purchases.Add(purchase);
				sb.AppendLine($"Imported {purchase.Game.Name} for {purchase.Card.User.Username}");
			}

			context.Purchases.AddRange(purchases);
			context.SaveChanges();

			return sb.ToString().TrimEnd();
		}

		private static bool IsValid(object dto)
		{
			var validationContext = new ValidationContext(dto);
			var validationResult = new List<ValidationResult>();

			return Validator.TryValidateObject(dto, validationContext, validationResult, true);
		}
	}
}