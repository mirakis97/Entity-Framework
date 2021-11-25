namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
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
            var authors = context.Authors
         .Select(x => new
         {
             FullName = x.FirstName + " " + x.LastName,
             AuthorBook = x.AuthorsBooks.OrderByDescending(p => p.Book.Price)
             .Select(b => new
             {
                 bookName = b.Book.Name,
                 price = b.Book.Price.ToString("F2")
             })
             .ToList()
         }).ToList()
         .OrderByDescending(x => x.AuthorBook.Count())
         .ThenBy(x => x.FullName);

            string books = JsonConvert.SerializeObject(authors, Formatting.Indented);

            return books;
        }

        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {
            List<ExportBookDto> exportBooks = context.Books
                .Where(x => x.PublishedOn < date && x.Genre == Genre.Science)
                .ToList()
                .OrderByDescending(x => x.Pages)
                .ThenByDescending(x => x.PublishedOn)
                .Take(10)
                .Select(x => new ExportBookDto
                {
                    Date = x.PublishedOn.ToString("d",CultureInfo.InvariantCulture),
                    Name = x.Name,
                    Pages = x.Pages

                }).ToList();



            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);



            XmlSerializer xmlSerializer =
            new XmlSerializer(typeof(List<ExportBookDto>), new XmlRootAttribute("Books"));


            xmlSerializer.Serialize(new StringWriter(sb), exportBooks, namespaces);

            return sb.ToString().Trim();
        }
    }
}