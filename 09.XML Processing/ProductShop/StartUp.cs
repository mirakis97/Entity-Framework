using AutoMapper;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        private static string resultPath = "../../../Datasets/Results/";
        public static void Main(string[] args)
        {
            var db = new ProductShopContext();
            //db.Database.EnsureDeleted();
            //db.Database.EnsureCreated();
            InitializeMapper();

            //string result = File.ReadAllText(resultPath + "categories-products.xml");;

            //Console.WriteLine(ImportCategoryProducts(db,result));
            //TODO Problem 06
            var result = GetUsersWithProducts(db);
            File.WriteAllText(resultPath + "GetSoldProducts.xml", result);

           // string result = GetSoldProducts(db);
            //if (!Directory.Exists(resultPath))
            //{
               // Directory.CreateDirectory(resultPath);
            //}
            //File.WriteAllText($"{resultPath}/GetProductsInRange.xml", result);
        }
        //Problem01
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlSerializer xml = new XmlSerializer(typeof(ImportUsersDto[]), new XmlRootAttribute("Users"));
            var usersDTO = (ImportUsersDto[])xml.Deserialize(new StringReader(inputXml));

            var users = Mapper.Map<User[]>(usersDTO);
            context.Users.AddRange(users);

            context.SaveChanges();
            return $"Successfully imported {users.Length}";
        }
        //Problem02
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer xml = new XmlSerializer(typeof(ImportProductDTO[]), new XmlRootAttribute("Products"));
            var productDto = (ImportProductDTO[])xml.Deserialize(new StringReader(inputXml));

            var products = new List<Product>();

            foreach (var item in productDto)
            {
                Product product = new Product()
                {
                    BuyerId = item.BuyerId,
                    Name = item.Name,
                    SellerId = item.SellerId,
                    Price = item.Price
                };
                products.Add(product);
            }
            context.Products.AddRange(products);

            context.SaveChanges();
            return $"Successfully imported {products.Count}";
        }
        //Problem03
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlSerializer xml = new XmlSerializer(typeof(ImportCategoryDTO[]), new XmlRootAttribute("Categories"));
            var categoryDTOs = (ImportCategoryDTO[])xml.Deserialize(new StringReader(inputXml));

            var categories = new List<Category>();

            foreach (var item in categoryDTOs)
            {
                Category category = new Category()
                {
                    Name = item.Name
                };
                if (!categories.Any(c => c.Name == category.Name))
                {
                    categories.Add(category);
                }
            }
            context.Categories.AddRange(categories);

            context.SaveChanges();
            return $"Successfully imported {categories.Count}";
        }
        //Problem04
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer xml = new XmlSerializer(typeof(ImportCategoryProductDTO[]), new XmlRootAttribute("CategoryProducts"));
            var categoryProductsDTOs = (ImportCategoryProductDTO[])xml.Deserialize(new StringReader(inputXml));

            var categoryProducts = new List<CategoryProduct>();

            foreach (var item in categoryProductsDTOs)
            {
                if (context.Categories.Any(c => c.Id == item.CategoryId) &&
                      context.Products.Any(p => p.Id == item.ProductId))
                {
                    CategoryProduct categoryProduct = new CategoryProduct()
                    {
                        CategoryId = item.CategoryId,
                        ProductId = item.ProductId
                    };
                    categoryProducts.Add(categoryProduct);
                }
            }
            context.CategoryProducts.AddRange(categoryProducts);

            context.SaveChanges();
            return $"Successfully imported {categoryProducts.Count}";
        }
        //Problem05
        public static string GetProductsInRange(ProductShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);


            var products = context
                .Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Take(10)
                .Select(p => new ExportProductsInRangeDTO()
                {
                    Name = p.Name,
                    BuyerName = p.Buyer.FirstName + " " + p.Buyer.LastName,
                    Price = p.Price

                })
                .ToArray();

            XmlSerializer xmlSerializer =
          new XmlSerializer(typeof(ExportProductsInRangeDTO[]), new XmlRootAttribute("Products"));


            xmlSerializer.Serialize(new StringWriter(sb), products, namespaces);

            return sb.ToString().Trim();
        }
        //Problem05
        public static string GetSoldProducts(ProductShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);


            var user = context.Users.OrderBy(u => u.LastName)
              .ThenBy(u => u.FirstName)
              .Where(u => u.ProductsSold.Any(ps => ps.Buyer != null))
              .Take(5)
              .Select(u => new ExportUserSoldProductsDTO
              {
                  FirstName = u.FirstName,
                  LastName = u.LastName,
                  SoldProducts = u.ProductsSold.Where(p => p.Buyer != null)
                 .Select(ps => new ExportSoldProductsDTO
                 {
                     Name = ps.Name,
                     Price = ps.Price
                 }).ToArray()
              })
              .ToArray();

            XmlSerializer xmlSerializer =
          new XmlSerializer(typeof(ExportUserSoldProductsDTO[]), new XmlRootAttribute("Users"));


            xmlSerializer.Serialize(new StringWriter(sb), user, namespaces);

            return sb.ToString().Trim();
        }
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            var categories = context.Categories
             .Select(x => new ExportCategoryByProductsDTO
             {
                 Name = x.Name,
                 ProductsCount = x.CategoryProducts.Count,
                 AveragePrice = x.CategoryProducts.Average(np => np.Product.Price),
                 TotalRevenue = x.CategoryProducts.Sum(tr => tr.Product.Price)
             }).OrderByDescending(x => x.ProductsCount)
             .ThenBy(x => x.TotalRevenue)
             .ToArray();

            XmlSerializer xmlSerializer =
             new XmlSerializer(typeof(ExportCategoryByProductsDTO[]), new XmlRootAttribute("Categories"));


            xmlSerializer.Serialize(new StringWriter(sb), categories, namespaces);

            return sb.ToString().Trim();
        }
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            var users = new UserRootDTO()
            {
                Count = context.Users.Count(u => u.ProductsSold.Any(p => p.Buyer != null)),
                Users = context.Users
                    .ToArray()
                    .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                    .OrderByDescending(u => u.ProductsSold.Count)
                    .Take(10)
                    .Select(u => new UserExportDTO()
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Age = u.Age,
                        SoldProducts = new SoldProductsDTO()
                        {
                            Count = u.ProductsSold.Count(ps => ps.Buyer != null),
                            Products = u.ProductsSold
                                .ToArray()
                                .Where(ps => ps.Buyer != null)
                                .Select(ps => new ExportProductSoldDTO()
                                {
                                    Name = ps.Name,
                                    Price = ps.Price
                                })
                                .OrderByDescending(p => p.Price)
                                .ToArray()
                        }
                    })

                    .ToArray()
            };

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserRootDTO), new XmlRootAttribute("Users"));

            xmlSerializer.Serialize(new StringWriter(sb), users, namespaces);

            return sb.ToString().Trim();
        }
        private static void InitializeMapper()
        {
            Mapper.Initialize(cfg => { cfg.AddProfile<ProductShopProfile>(); });
        }
    }
}