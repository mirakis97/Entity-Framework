using CodeFirstDemo.Models;
using System;

namespace CodeFirstDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new NewsDbContex();
            db.Database.EnsureCreated();
        }
    }
}
