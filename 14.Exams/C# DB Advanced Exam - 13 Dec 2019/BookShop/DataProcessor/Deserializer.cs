namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using BookShop.Data.Models;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            List<Book> bookToAdd = new List<Book>();
            var booksToImport = XmlConverter.Deserializer<ImportAuthorsDTO>(xmlString, "Books");

            foreach (var books in booksToImport)
            {
                if (!IsValid(books))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var date = DateTime.ParseExact(books.PublishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                ;
                Book book = new Book()
                {
                    Name = books.Name,
                    Genre = (Genre)books.Genre,
                    Price = books.Price,
                    Pages = books.Pages,
                    PublishedOn = date,
                };

                bookToAdd.Add(book);
                sb.AppendLine(string.Format(SuccessfullyImportedBook,books.Name,books.Price));
            }

            context.Books.AddRange(bookToAdd);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            List<Author> authorsToAdd = new List<Author>();
            var importAuthors = JsonConvert.DeserializeObject<IEnumerable<ImportBooks>>(jsonString);

            foreach (var importAuthor in importAuthors)
            {
                if (!IsValid(importAuthor))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (authorsToAdd.Any(x => x.Email == importAuthor.Email))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Author author = new Author()
                {
                    FirstName = importAuthor.FirstName,
                    LastName = importAuthor.LastName,
                    Phone = importAuthor.Phone,
                    Email = importAuthor.Email,
                };

                foreach (var books in importAuthor.Books)
                {
                    if (!books.Id.HasValue)
                    {
                        continue;
                    }

                    Book  book = context.Books.FirstOrDefault(x => x.Id == books.Id);
                    if (book == null)
                    {
                        continue;
                    }

                    author.AuthorsBooks.Add(new AuthorBook
                    {
                        Book = book,
                        Author = author
                    });
                }

                if (!author.AuthorsBooks.Any())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                authorsToAdd.Add(author);
                sb.AppendLine(string.Format(SuccessfullyImportedAuthor, author.FirstName + " " + author.LastName, author.AuthorsBooks.Count));
            }

            context.Authors.AddRange(authorsToAdd);
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