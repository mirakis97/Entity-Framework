namespace VaporStore.DataProcessor
{
	using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Export;

    public static class Serializer
	{
		public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
		{

			var result = context.Genres
				.ToArray()
				.Where(g => genreNames.Contains(g.Name))
				.Select(genre => new GenreDto
				{
					Id = genre.Id,
					Genre = genre.Name,
					Games = genre.Games
						.Where(g => g.Purchases.Any())
						.Select(game => new GameDto
						{
							Id = game.Id,
							Title = game.Name,
							Developer = game.Developer.Name,
							Tags = string.Join(", ", game.GameTags.Select(g => g.Tag.Name)),
							Players = game.Purchases.Count
						})
						.OrderByDescending(game => game.Players)
						.ThenBy(game => game.Id)
						.ToArray(),
					TotalPlayers = genre.Games.Sum(g => g.Purchases.Count)
				})
				.OrderByDescending(g => g.TotalPlayers)
				.ThenBy(g => g.Id)
				.ToArray();

			var json = JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
			return json;
		}

		public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
		{

            PurchaseType purchaseTypeEnum = Enum.Parse<PurchaseType>(storeType);

            ExportUserDto[] usersDtos = context.Users
                .ToArray()
                .Where(u => u.Cards.Any(c => c.Purchases.Any()))
                .Select(u => new ExportUserDto()
                {
                    Username = u.Username,
                    Purchases = context.Purchases
                        .ToArray()
                        .Where(p => p.Card.User.Username == u.Username && p.Type == purchaseTypeEnum)
                        .OrderBy(p => p.Date)
                        .Select(p => new ExportPurchaseDto()
                        {
                            CardNumber = p.Card.Number,
                            Cvc = p.Card.Cvc,
                            Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                            Game = new ExportGameDto()
                            {
                                Name = p.Game.Name,
                                GenreName = p.Game.Genre.Name,
                                Price = p.Game.Price
                            }
                        })
                        .ToArray(),
                    TotalSpent = context
                        .Purchases
                        .ToArray()
                        .Where(p => p.Card.User.Username == u.Username && p.Type == purchaseTypeEnum)
                        .Sum(p => p.Game.Price)

                })
                .Where(u => u.Purchases.Length > 0)
                .OrderByDescending(u => u.TotalSpent)
                .ThenBy(u => u.Username)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportUserDto[]), new XmlRootAttribute("Users"));

            using (StringWriter writer = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, usersDtos, namespaces);
            }

            return sb.ToString().Trim();
        }
	}
}