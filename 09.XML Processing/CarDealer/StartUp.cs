using AutoMapper;
using CarDealer.Data;
using CarDealer.Dto.Export;
using CarDealer.Dto.Import;
using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        private const string DatasetsDirPath = @"../../../Datasets/";
        private const string ResultDirPath = DatasetsDirPath + "Results/";
        public static void Main(string[] args)
        {
            InitializeMapper();
            var db = new CarDealerContext();
            //db.Database.EnsureDeleted();
            //db.Database.EnsureCreated();


            //Problem 01
            //string inputXml = File.ReadAllText(DatasetsDirPath + "suppliers.xml");
            //string result = ImportSuppliers(db, inputXml);
            //Console.WriteLine(result);
            //Problem 02
            //string inputXml2 = File.ReadAllText(DatasetsDirPath + "parts.xml");
            //string result2 = ImportParts(db, inputXml2);
            //Console.WriteLine(result2);
            //Problem 03
            //string inputXml2 = File.ReadAllText(DatasetsDirPath + "cars.xml");
            //string result2 = ImportCars(db, inputXml2);
            //Console.WriteLine(result2);
            //Problem 04
            //string inputXml2 = File.ReadAllText(DatasetsDirPath + "customers.xml");
            //string result2 = ImportCustomers(db, inputXml2);
            //Console.WriteLine(result2);
            //Problem 05
            //string inputXml2 = File.ReadAllText(DatasetsDirPath + "sales.xml");
            //string result2 = ImportSales(db, inputXml2);
            //Console.WriteLine(result2);

            var result = GetSalesWithAppliedDiscount(db);
            File.WriteAllText(ResultDirPath + "GetSalesWithAppliedDiscount.xml", result);
        }
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {

            XmlSerializer xml = new XmlSerializer(typeof(ImportSupplierDTO[]), new XmlRootAttribute("Suppliers"));
            var usersDTO = (ImportSupplierDTO[])xml.Deserialize(new StringReader(inputXml));

            List<Supplier> suppliers = new List<Supplier>();

            foreach (var importSupplierDto in usersDTO)
            {
                Supplier supplier = new Supplier()
                {
                    Name = importSupplierDto.Name,
                    IsImporter = importSupplierDto.IsImporter
                };
                suppliers.Add(supplier);
            }
            context.Suppliers.AddRange(suppliers);

            context.SaveChanges();
            return $"Successfully imported {suppliers.Count}";
        }
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlSerializer xml = new XmlSerializer(typeof(ImportPartDTO[]), new XmlRootAttribute("Parts"));
            var partsDto = (ImportPartDTO[])xml.Deserialize(new StringReader(inputXml));

            List<Part> parts = new List<Part>();

            foreach (var importPartDto in partsDto)
            {
                if (context.Suppliers.Any(c => c.Id == importPartDto.SupplierId))
                {
                    Part part = new Part()
                    {
                        Name = importPartDto.Name,
                        Price = importPartDto.Price,
                        Quantity = importPartDto.Quantity,
                    };
                    parts.Add(part);
                }
            }
            context.Parts.AddRange(parts);

            context.SaveChanges();
            return $"Successfully imported {parts.Count}";
        }
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer xml = new XmlSerializer(typeof(ImportCarDTO[]), new XmlRootAttribute("Cars"));
            var carDTOs = (ImportCarDTO[])xml.Deserialize(new StringReader(inputXml));
            var cars = new List<Car>();
            var partCars = new List<PartCar>();

            foreach (var carDto in carDTOs)
            {
                var car = new Car()
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TravelledDistance
                };

                var parts = carDto
                    .Parts
                    .Where(pc => context.Parts.Any(p => p.Id == pc.Id))
                    .Select(p => p.Id)
                    .Distinct();

                foreach (var part in parts)
                {
                    PartCar partCar = new PartCar()
                    {
                        PartId = part,
                        Car = car
                    };

                    partCars.Add(partCar);
                }

                cars.Add(car);

            }

            context.PartCars.AddRange(partCars);

            context.Cars.AddRange(cars);

            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer =
             new XmlSerializer(typeof(ImportCustomerDTO[]), new XmlRootAttribute("Customers"));

            ImportCustomerDTO[] customersDtos =
                (ImportCustomerDTO[])xmlSerializer.Deserialize(new StringReader(inputXml));

            List<Customer> customers = new List<Customer>();

            foreach (var importCustomer in customersDtos)
            {
                DateTime date;
                bool isValidDate = DateTime.TryParseExact(importCustomer.BirthDate, "yyyy-MM-dd'T'HH:mm:ss",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
                if (isValidDate)
                {
                    Customer customer = new Customer()
                    {
                        Name = importCustomer.Name,
                        BirthDate = date,
                        IsYoungDriver = importCustomer.IsYoungDriver
                    };
                    customers.Add(customer);
                }
            }
            context.Customers.AddRange(customers);

            context.SaveChanges();
            return $"Successfully imported {customers.Count}";
        }
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer =
             new XmlSerializer(typeof(ImportSaleDTO[]), new XmlRootAttribute("Sales"));

            ImportSaleDTO[] saleDTOs =
                (ImportSaleDTO[])xmlSerializer.Deserialize(new StringReader(inputXml));

            List<Sale> sales = new List<Sale>();

            foreach (var saleDTO in saleDTOs)
            {
                if (context.Cars.Any(c => c.Id == saleDTO.CarId))
                {
                    Sale sale = new Sale()
                    {
                        CarId = saleDTO.CarId,
                        CustomerId = saleDTO.CustomerId,
                        Discount = saleDTO.Discount
                    };

                    sales.Add(sale);
                }
            }
            context.Sales.AddRange(sales);

            context.SaveChanges();
            return $"Successfully imported {sales.Count}";
        }
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);


            var products = context
                .Cars
                .Where(p => p.TravelledDistance > 2000000)
                .OrderBy(p => p.Make)
                .ThenBy(p => p.Model)
                .Take(10)
                .Select(p => new ExportCarsWithDistance()
                {
                    Make = p.Make,
                    Model = p.Model,
                    TravelledDistance = p.TravelledDistance

                })
                .ToArray();

            XmlSerializer xmlSerializer =
          new XmlSerializer(typeof(ExportCarsWithDistance[]), new XmlRootAttribute("cars"));


            xmlSerializer.Serialize(new StringWriter(sb), products, namespaces);

            return sb.ToString().Trim();
        }
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var cars = context
                .Cars
                .Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .Select(x => new ExportCarsBMWDTO()
                {
                    Id = x.Id,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCarsBMWDTO[]), new XmlRootAttribute("cars"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            StringBuilder sb = new StringBuilder();
            xmlSerializer.Serialize(new StringWriter(sb), cars, namespaces);

            return sb.ToString().Trim();
        }
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context
               .Suppliers
               .Where(c => !c.IsImporter)
               .Select(x => new ExportLocalSuppliersDTO()
               {
                   Id = x.Id,
                   Name = x.Name,
                   PartsCount = x.Parts.Count
               })
               .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportLocalSuppliersDTO[]), new XmlRootAttribute("suppliers"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            StringBuilder sb = new StringBuilder();
            xmlSerializer.Serialize(new StringWriter(sb), suppliers, namespaces);

            return sb.ToString().Trim();
        }
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var partsCar = context
              .Cars
              .OrderByDescending(c => c.TravelledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
              .Select(x => new ExportCarWithPartsDTO()
              {
                  Make = x.Make,
                  Model = x.Model,
                  Parts = x.PartCars.Select(y => new ExportPartCarsDTO()
                  {
                      Name = y.Part.Name,
                      Price = y.Part.Price
                  }).OrderByDescending(y => y.Price)
                        .ToArray(),
                  TravelledDistance = x.TravelledDistance,
              })
              .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCarWithPartsDTO[]), new XmlRootAttribute("cars"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            StringBuilder sb = new StringBuilder();
            xmlSerializer.Serialize(new StringWriter(sb), partsCar, namespaces);

            return sb.ToString().Trim();
        }
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var carsSales = context
              .Customers
              .Select(x => new ExportCustomerTotalSalesDTO()
              {
                  Name = x.Name,
                  BoughtCars = x.Sales.Count,
                  SpentMoney = x.Sales.Sum(s => s.Car.PartCars.Sum(p => p.Part.Price))
              })
              .Where(x => x.BoughtCars >= 1)
              .OrderByDescending(x => x.SpentMoney)
              .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCustomerTotalSalesDTO[]), new XmlRootAttribute("customers"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            StringBuilder sb = new StringBuilder();
            xmlSerializer.Serialize(new StringWriter(sb), carsSales, namespaces);

            return sb.ToString().Trim();
        }
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context
             .Sales
             .Select(x => new ExportSaleDTO()
             {
                 Car = new ExportCarSaleDTO()
                 {
                     Make = x.Car.Make,
                     Model = x.Car.Model,
                     TravelledDistance = x.Car.TravelledDistance
                 },
                 CustomerName = x.Customer.Name,
                 Discount = x.Discount,
                 Price = x.Car.PartCars.Sum(x => x.Part.Price),
                 PriceWithDiscount = x.Car.PartCars.Sum(c => c.Part.Price) -
                                        x.Car.PartCars.Sum(c => c.Part.Price) * x.Discount / 100
             })
             .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportSaleDTO[]), new XmlRootAttribute("sales"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            StringBuilder sb = new StringBuilder();
            xmlSerializer.Serialize(new StringWriter(sb), sales, namespaces);

            return sb.ToString().Trim();
        }
        private static void InitializeMapper()
        {
            //Mapper.Initialize(cfg => { cfg.AddProfile<ProductShopProfile>(); });
        }
    }
}