using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NoSQL
{
    class Program
    {
        static void Main(string[] args)
        {
            //"Steve Jobs", date "05-05-2005", name "The story of Apple",  rating "60".
            var client = new MongoClient("mongodb://127.0.0.1:27017/");
            IMongoDatabase database = client.GetDatabase("NoSql");

            var collleciton = database.GetCollection<BsonDocument>("softuniArticles");

            List<BsonDocument> allArticles = collleciton.Find(new BsonDocument { }).ToList();
            //Problem2
            //foreach (var articles in allArticles)
            //{
            //    string name = articles.GetElement("name").Value.AsString;

            //    Console.WriteLine(name);
            //}

            //Problem3
            //collleciton.InsertOne(new BsonDocument
            //{
            //    {"author", "Steve Jobs" },
            //    {"date", "05-05-2005" },      
            //    {"name", "The story of Apple" },
            //    {"rating", "60" },
            //});
            //Problem4
            //foreach (var articles in allArticles)
            //{
            //    string name = articles.GetElement("name").Value.AsString;
            //    int newRaitng = int.Parse(articles.GetElement("rating").Value.AsString) + 10;
            //    var query = Builders<BsonDocument>.Filter.Eq("_id", articles.GetElement("_id").Value);
            //    var updateQuery = Builders<BsonDocument>.Update.Set("rating",newRaitng.ToString());

            //    collleciton.UpdateOne(query,updateQuery);
            //    Console.WriteLine($"{name} : rating {articles.GetElement("rating").Value}");
            //}
            //Problem5

            // Find the documents to delete
            var test = collleciton.Database.GetCollection<BsonDocument>("rating");
            var filter = new BsonDocument();
            var docs = test.Find(filter).ToList();

            // Get the _id values of the found documents
            var ids = docs.Select(d => d.GetElement("_id"));

            // Create an $in filter for those ids
            var idsFilter = Builders<BsonDocument>.Filter.In(d => d.GetElement("_id"), ids);

            // Delete the documents using the $in filter
            var result = test.DeleteMany(idsFilter);
            foreach (var articles in allArticles)
            {
                string name = articles.GetElement("name").Value.AsString;
                Console.WriteLine($"{name} : rating {articles.GetElement("rating").Value}");
            }
        }
    }
}
