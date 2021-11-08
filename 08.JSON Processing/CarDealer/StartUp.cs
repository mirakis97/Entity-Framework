using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        private static string RESULT_DIRECTORY_PATH = @"C:\Miroslav Apps\Programing\SoftUni\C# Web Developer\DB\Entity Framework\JSON Processing\CarDealer\Datasets\Results";

        public static void Main(string[] args)
        {
            var db = new CarDealerContext();
            //db.Database.EnsureDeleted();
            //db.Database.EnsureCreated();
            //Problem01
            //string jsonString1 = File.ReadAllText(@"C:\Miroslav Apps\Programing\SoftUni\C# Web Developer\DB\Entity Framework\JSON Processing\CarDealer\Datasets\suppliers.json");
            //Console.WriteLine(ImportSuppliers(db, jsonString1));
            //Problem02
            //string jsonString2 = File.ReadAllText(@"C:\Miroslav Apps\Programing\SoftUni\C# Web Developer\DB\Entity Framework\JSON Processing\CarDealer\Datasets\parts.json");
            //Console.WriteLine(ImportParts(db, jsonString2));
            //Problem03
            //string jsonString3 = File.ReadAllText(@"C:\Miroslav Apps\Programing\SoftUni\C# Web Developer\DB\Entity Framework\JSON Processing\CarDealer\Datasets\cars.json");
            //Console.WriteLine(ImportCars(db, jsonString3));
            //Problem03
            //string jsonString4 = File.ReadAllText(@"C:\Miroslav Apps\Programing\SoftUni\C# Web Developer\DB\Entity Framework\JSON Processing\CarDealer\Datasets\customers.json");
            //Console.WriteLine(ImportCustomers(db, jsonString4));
            //Problem04
            //string jsonString5 = File.ReadAllText(@"C:\Miroslav Apps\Programing\SoftUni\C# Web Developer\DB\Entity Framework\JSON Processing\CarDealer\Datasets\sales.json");
            //Console.WriteLine(ImportSales(db, jsonString5));

            string result = GetSalesWithAppliedDiscount(db);
            if (!Directory.Exists(RESULT_DIRECTORY_PATH))
            {
                Directory.CreateDirectory(RESULT_DIRECTORY_PATH);
            }
            File.WriteAllText($"{RESULT_DIRECTORY_PATH}/GetSalesWithAppliedDiscount.json", result);

        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            List<Supplier> suppliers = JsonConvert.DeserializeObject<List<Supplier>>(inputJson);
            context.Suppliers.AddRange(suppliers);
            int count = suppliers.Count();
            context.SaveChanges();
            return $"Successfully imported {count}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            List<Part> parts = JsonConvert.DeserializeObject<List<Part>>(inputJson);
            var suppliers = context.Suppliers.Select(s => s.Id);
            parts = parts.Where(p => suppliers.Any(s => s == p.SupplierId)).ToList();
            context.Parts.AddRange(parts);
            int count = parts.Count();
            context.SaveChanges();
            return $"Successfully imported {count}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var cars = JsonConvert.DeserializeObject<List<ImportCarDto>>(inputJson);
            List<Car> listOfcars = new List<Car>();
            foreach (var carJson in cars)
            {
                Car car = new Car()
                {
                    Make = carJson.Make,
                    Model = carJson.Model,
                    TravelledDistance = carJson.TravelledDistance
                };
                foreach (var partId in carJson.PartsId.Distinct())
                {
                    car.PartCars.Add(new PartCar()
                    {
                        Car = car,
                        PartId = partId
                    });
                }
                listOfcars.Add(car);
            }
            context.Cars.AddRange(listOfcars);
            int count = listOfcars.Count();
            context.SaveChanges();
            return $"Successfully imported {count}.";
        }
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(inputJson);
            context.Customers.AddRange(customers);
            int count = customers.Count();
            context.SaveChanges();
            return $"Successfully imported {count}.";
        }
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            List<Sale> sales = JsonConvert.DeserializeObject<List<Sale>>(inputJson);
            context.Sales.AddRange(sales);
            int count = sales.Count();
            context.SaveChanges();
            return $"Successfully imported {count}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customures = context.Customers
                .Select(x => new
                {
                    Name = x.Name,
                    BirthDate = x.BirthDate,
                    IsYoungDriver = x.IsYoungDriver
                }).OrderBy(x => x.BirthDate).ThenBy(x => x.IsYoungDriver).ToList();

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = "dd/MM/yyyy";
            jsonSettings.Formatting = Formatting.Indented;
            string customersJson = JsonConvert.SerializeObject(customures, jsonSettings);

            return customersJson;
        }
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var toyotaCars = context.Cars
                .Select(x => new
                {
                    Id = x.Id,
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                }).Where(x => x.Make == "Toyota")
                .OrderBy(x => x.Model).ThenByDescending(x => x.TravelledDistance).ToList();

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.Formatting = Formatting.Indented;
            string cars = JsonConvert.SerializeObject(toyotaCars, jsonSettings);

            return cars;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(x => x.IsImporter == false)
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    PartsCount = x.Parts.Count
                }).ToList();

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.Formatting = Formatting.Indented;
            string supplie = JsonConvert.SerializeObject(suppliers, jsonSettings);

            return supplie;
        }
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carParts = context.Cars
                .Select(x => new
                {
                    car = new
                    {
                        Make = x.Make,
                        Model = x.Model,
                        TravelledDistance = x.TravelledDistance
                    },

                    parts = x.PartCars.Select(c => new
                    {
                        Name = c.Part.Name,
                        Price = $"{c.Part.Price:F2}"
                    }).ToList()
                }).ToList();

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.Formatting = Formatting.Indented;
            string carPart = JsonConvert.SerializeObject(carParts, jsonSettings);

            return carPart;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(x => x.Sales.Count >= 1)
                .Select(x => new
                {
                    fullName = x.Name,
                    boughtCars = x.Sales.Count,
                    spentMoney = x.Sales.Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price))
                }).OrderByDescending(x => x.spentMoney).ThenByDescending(x => x.boughtCars).ToList();

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.Formatting = Formatting.Indented;
            string customer = JsonConvert.SerializeObject(customers, jsonSettings);

            return customer;
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(x => new
                {
                    car = new
                    {
                        Make = x.Car.Make,
                        Model = x.Car.Model,
                        TravelledDistance = x.Car.TravelledDistance
                    },
                    customerName = x.Customer.Name,
                    Discount = x.Discount.ToString("F2"),
                    price = x.Car.PartCars.Sum(c => c.Part.Price).ToString("F2"),
                    priceWithDiscount = (((100 - x.Discount) / 100) * x.Car.PartCars.Sum(s => s.Part.Price)).ToString("f2")
                }).Take(10).ToList();

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.Formatting = Formatting.Indented;
            string sale = JsonConvert.SerializeObject(sales, jsonSettings);

            return sale;
        }
    }
}