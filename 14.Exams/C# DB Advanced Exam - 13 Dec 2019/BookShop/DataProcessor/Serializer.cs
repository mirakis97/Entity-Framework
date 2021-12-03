namespace BookShop.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportMostCraziestAuthors(BookShopContext context)
        {
            var result = context.Authors
                .ToArray()
                .Select(x => new
                {
                    AuthorName = x.FirstName + " " + x.LastName,
                    Books = x.AuthorsBooks.OrderByDescending(b => b.Book.Price).Select(b => new
                    {
                        BookName = b.Book.Name,
                        BookPrice = b.Book.Price.ToString("f2")
                    })
                    
                }).OrderByDescending(x => x.Books.Count()).ThenBy(x => x.AuthorName);


			var json = JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
			return json;
		}

        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {
            var projects = context.Books
            .ToArray()
            .Where(p => p.PublishedOn < date && p.Genre == Genre.Science)
            .OrderByDescending(x => x.Pages).ThenByDescending(x => x.PublishedOn)
            .Take(10)
            .Select(x => new ExportOldestBook
            {
                Name = x.Name,
                Date = x.PublishedOn.ToString("d", CultureInfo.InvariantCulture),
                Pages = x.Pages
            })
            .ToArray();

            var xml = XmlConverter.Serialize(projects, "Books");

            return xml;
        }
    }
}