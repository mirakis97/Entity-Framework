namespace BookShop
{
    using Data;
    using Initializer;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);
            //01.
            //string problem1 = Console.ReadLine();
            //Console.WriteLine(GetBooksByAgeRestriction(db,problem1));
            //02.
            // Console.WriteLine(GetGoldenBooks(db));
            //03.
            //Console.WriteLine(GetBooksByPrice(db));
            //04.
            //int year = int.Parse(Console.ReadLine());
            //Console.WriteLine(GetBooksNotReleasedIn(db, year));
            //05.
            //string problem5 = Console.ReadLine();
            //Console.WriteLine(GetBooksByCategory(db, problem5));
            //06.
            //string problem6 = Console.ReadLine();
            //Console.WriteLine(GetBooksReleasedBefore(db, problem6));
            //07.
            //string problem7 = Console.ReadLine();
            //Console.WriteLine(GetAuthorNamesEndingIn(db, problem7));
            //08.
            //string problem8 = Console.ReadLine();
            //Console.WriteLine(GetBookTitlesContaining(db, problem8));
            //09.
            //string problem9 = Console.ReadLine();
            //Console.WriteLine(GetBooksByAuthor(db, problem9));
            //10.
            //int length = int.Parse(Console.ReadLine());
            //Console.WriteLine(CountBooks(db, length));
            //11.
            //Console.WriteLine(CountCopiesByAuthor(db));
            //12.
            //Console.WriteLine(GetTotalProfitByCategory(db));
            //13.
            //Console.WriteLine(GetMostRecentBooks(db));
            //14.
            //Console.WriteLine(IncreasePrices(db));
            //15.
            //Console.WriteLine(RemoveBooks(db));
        }
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var books = context.Books
                .AsEnumerable()
                .Where(x => x.AgeRestriction.ToString().ToLower() == command.ToLower())
                .Select(x => new { x.Title }).OrderBy(x => x.Title);

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .AsEnumerable()
                .Where(x => x.EditionType.ToString() == "Gold" && x.Copies < 5000)
                .Select(x => new
                {
                    x.BookId,
                    x.Title
                }).OrderBy(x => x.BookId);
            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
             .Where(x => x.Price > 40)
             .Select(x => new
             {
                 x.BookId,
                 x.Title,
                 x.Price
             }).OrderByDescending(x => x.Price);
            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
            .Where(x => x.ReleaseDate.Value.Year != year)
            .Select(x => new
            {
                x.BookId,
                x.Title,
                x.Price
            }).OrderBy(x => x.BookId);
            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            List<string> categories = input
                .Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLower())
                .ToList();
            List<string> bookTitles = new List<string>();

            foreach (var category in categories)
            {
                List<string> currCategoryBooks = context.Books
                    .Where(x => x.BookCategories.Any(x => x.Category.Name.ToLower() == category))
                    .Select(x => x.Title).ToList();

                bookTitles.AddRange(currCategoryBooks);
            }


            return string.Join(Environment.NewLine, bookTitles.OrderBy(x => x));
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime dateTime = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var books = context.Books
               .Where(x => x.ReleaseDate < dateTime)
               .Select(x => new
               {
                   x.BookId,
                   x.EditionType,
                   x.Title,
                   x.Price,
                   x.ReleaseDate
               }).OrderByDescending(x => x.ReleaseDate).ToList();
            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors.AsEnumerable().Where(x => x.FirstName.EndsWith(input)).Select(x => new
            {
                FUllName = x.FirstName + " " + x.LastName

            }).OrderBy(x => x.FUllName).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.FUllName}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books.Where(x => x.Title.ToLower().Contains(input.ToLower())).Select(x => new
            {
                x.Title

            }).OrderBy(x => x.Title).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books.Where(x => x.Author.LastName.ToLower().StartsWith(input.ToLower())).Select(x => new
            {
                x.BookId,
                x.Title,
                FullName = x.Author.FirstName + " " + x.Author.LastName

            }).OrderBy(x => x.BookId).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} ({book.FullName})");
            }

            return sb.ToString().TrimEnd();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books.Where(x => x.Title.Length > lengthCheck).Count();
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var count = context.Authors.Select(x => new
            {
                FullName = x.FirstName + " " + x.LastName,
                Count = x.Books.Sum(x => x.Copies)
            }).OrderByDescending(x => x.Count).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in count)
            {
                sb.AppendLine($"{book.FullName} - {book.Count}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var count = context.Categories.Select(x => new
            {
                Name = x.Name,
                TotalProfit = x.CategoryBooks.Select(x => new
                { 
                    BookProfit = x.Book.Copies * x.Book.Price
                }).Sum(x => x.BookProfit)
            }).OrderByDescending(x => x.TotalProfit).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var catagory in count)
            {
                sb.AppendLine($"{catagory.Name} ${catagory.TotalProfit}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var books = context.Categories.Select(x => new 
            {
                x.Name,
                Books = x.CategoryBooks.OrderByDescending(x => x.Book.ReleaseDate).Take(3).Select(x => new 
                {
                   x.Book.Title,
                   Year = x.Book.ReleaseDate
                })
            }).OrderBy(x => x.Name).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"--{book.Name}");
                foreach (var item in book.Books)
                {
                    sb.AppendLine($"{item.Title} ({item.Year.Value.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var priceToBooksToIncrease = context.Books.Where(x => x.ReleaseDate.Value.Year < 2010);

            foreach (var item in priceToBooksToIncrease)
            {
                item.Price += 5;
            }
            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var bookToRemove = context.Books.Where(x => x.Copies < 4200).ToList();

            var count = bookToRemove.Count();
            context.RemoveRange(bookToRemove);

            context.SaveChanges();
            return count;
        }
    }
}
