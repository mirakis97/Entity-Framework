using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        private static string RESULT_DIRECTORY_PATH = @"C:\Miroslav Apps\Programing\SoftUni\C# Web Developer\DB\Entity Framework\JSON Processing\ProductShop\Datasets\Results";
        public static void Main(string[] args)
        {

            var db = new ProductShopContext();
            //db.Database.EnsureDeleted();
            //db.Database.EnsureCreated();
            //Problem01
            //string jsonString1 = File.ReadAllText(@"C:\Miroslav Apps\Programing\SoftUni\C# Web Developer\DB\Entity Framework\JSON Processing\ProductShop\Datasets\users.json");
            //Console.WriteLine(ImportUsers(db, jsonString1));
            //Problem02
            //string jsonString2 = File.ReadAllText(@"C:\Miroslav Apps\Programing\SoftUni\C# Web Developer\DB\Entity Framework\JSON Processing\ProductShop\Datasets\products.json");
            //Console.WriteLine(ImportProducts(db, jsonString2));
            //Problem03
            //string jsonString3 = File.ReadAllText(@"C:\Miroslav Apps\Programing\SoftUni\C# Web Developer\DB\Entity Framework\JSON Processing\ProductShop\Datasets\categories.json");
            //Console.WriteLine(ImportCategories(db, jsonString3));
            //Problem04
            //string jsonString = File.ReadAllText(@"C:\Miroslav Apps\Programing\SoftUni\C# Web Developer\DB\Entity Framework\JSON Processing\ProductShop\Datasets\categories-products.json");
            //Console.WriteLine(ImportCategoryProducts(db, jsonString));

            //Problem05
            string result = GetUsersWithProducts(db);
            if (!Directory.Exists(RESULT_DIRECTORY_PATH))
            {
                Directory.CreateDirectory(RESULT_DIRECTORY_PATH);
            }
            File.WriteAllText($"{RESULT_DIRECTORY_PATH}/GetUsersWithProducts.json", result);
        }
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            List<User> users = JsonConvert.DeserializeObject<List<User>>(inputJson);
            context.Users.AddRange(users);
            int count = users.Count();
            context.SaveChanges();
            return $"Successfully imported {count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            List<Product> products = JsonConvert.DeserializeObject<List<Product>>(inputJson);
            context.Products.AddRange(products);
            int count = products.Count();
            context.SaveChanges();
            return $"Successfully imported {count}";
        }
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(inputJson).Where(x => x.Name != null).ToList();
            context.Categories.AddRange(categories);
            int count = categories.Count();
            context.SaveChanges();
            return $"Successfully imported {count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            List<CategoryProduct> categoryProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);
            context.CategoryProducts.AddRange(categoryProducts);
            //int count = categoryProducts.Count();
            context.SaveChanges();
            return $"Successfully imported {categoryProducts.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var productInRange = context.Products

                .Select(x => new
                {
                    name = x.Name,
                    price = x.Price,
                    seller = x.Seller.FirstName + " " + x.Seller.LastName
                }).Where(x => x.price >= 500 && x.price <= 1000).OrderBy(x => x.price).ToList();

            string product = JsonConvert.SerializeObject(productInRange, Formatting.Indented);

            return product;
        }
        public static string GetSoldProducts(ProductShopContext context)
        {
            var user = context.Users
                .Where(u => u.ProductsSold.Any(ps => ps.Buyer != null))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold
                   .Select(ps => new
                   {
                       name = ps.Name,
                       price = ps.Price,
                       buyerFirstName = ps.Buyer.FirstName,
                       buyerLastName = ps.Buyer.LastName
                   })
                   .ToArray()
                })
                .OrderBy(u => u.lastName)
                .ThenBy(u => u.firstName)
                .ToArray();


            string users = JsonConvert.SerializeObject(user, Formatting.Indented);

            return users;

        }
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .OrderByDescending(x => x.CategoryProducts.Count)
                .Select(x => new
                {
                    category = x.Name,
                    productsCount = x.CategoryProducts.Count,
                    averagePrice = x.CategoryProducts.Average(np => np.Product.Price).ToString("F2"),
                    totalRevenue = x.CategoryProducts.Sum(tr => tr.Product.Price).ToString("F2")
                }).ToList();

            string category = JsonConvert.SerializeObject(categories, Formatting.Indented);

            return category;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(x => x.ProductsSold.Any(s => s.Buyer != null))
                .Select(x => new
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Age = x.Age,
                    SoldProducts = new
                    {
                        Count = x.ProductsSold.Count,
                        Products = x.ProductsSold.Select(y => new
                        {
                            Name = y.Name,
                            Price = y.Price
                        }).ToArray()
                    }
                }).OrderByDescending(x => x.SoldProducts.Count).ToArray();


            var usersWithProducts = new
            {
                usersCount = users.Length,
                users = users
            };

            var result = JsonConvert.SerializeObject(usersWithProducts, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            return result;

        }
    }
}