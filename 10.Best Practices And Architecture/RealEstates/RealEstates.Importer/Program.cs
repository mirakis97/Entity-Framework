using RealEstates.Data;
using RealEstates.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace RealEstates.Importer
{
    class Program
    {
        static void Main(string[] args)
        {
            ImportJsonFile("imot.bg-houses-Sofia-raw-data-2021-03-18.json");
            Console.WriteLine();
            ImportJsonFile("imot.bg-raw-data-2021-03-18.json");
        }

        public static void ImportJsonFile(string json)
        {
            var dbContext = new ApplicationDbContext();
            IPropertiesService propertiesService = new PropertiesService(dbContext);
            var properties = JsonSerializer.Deserialize<IEnumerable<PropertyAsJson>>(File.ReadAllText(json));

            foreach (var jsonProp in properties)
            {
                propertiesService.Add(jsonProp.District, jsonProp.Floor,
                    jsonProp.TotalFloors, jsonProp.Size, jsonProp.YardSize,
                    jsonProp.Year, jsonProp.Type, jsonProp.BuildingType, jsonProp.Price);
                Console.WriteLine(".");
            }
        }
    }
}
