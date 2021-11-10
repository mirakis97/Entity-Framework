using AutoMapper;
using ProductShop.Data;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        private static string resultPath = "../../../Datasets/";
        public static void Main(string[] args)
        {
            var db = new ProductShopContext();
            //db.Database.EnsureDeleted();
            //db.Database.EnsureCreated();
            InitializeMapper();

            string result = File.ReadAllText(resultPath + "categories-products.xml");;

            Console.WriteLine(ImportCategoryProducts(db,result));
        }
        //Problem01
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlSerializer xml = new XmlSerializer(typeof(ImportUsersDto[]),new XmlRootAttribute("Users"));
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

        private static void InitializeMapper()
        {
            Mapper.Initialize(cfg => { cfg.AddProfile<ProductShopProfile>(); });
        }
    }
}