using StackExchange.Redis;
using System;

namespace DLock.HandsOnRedis
{
    internal class Program
    {
        const string MyKey = "SE130097";

        static void Main(string[] args)
        {
            var multiplexer = GetConnectionMultiplexer();
            var database = multiplexer.GetDatabase();

            WorkingWithStrings(database);

            WorkingWithList(database);

            WorkingWithSets(database);
        }

        static ConnectionMultiplexer GetConnectionMultiplexer()
        {
            var cfg = new ConfigurationOptions()
            {
                AllowAdmin = true,
            };

            cfg.EndPoints.Add("localhost");

            var connection = ConnectionMultiplexer.Connect(cfg);

            return connection;
        }

        static void WorkingWithStrings(IDatabase database)
        {
            Console.WriteLine("------------------");
            Console.WriteLine("WorkingWithStrings");
            Console.WriteLine("------------------");

            database.KeyDelete(MyKey);

            database.StringSet(MyKey, "Hello");

            var myString = database.StringGet(MyKey);

            Console.WriteLine(myString);
            Console.WriteLine();
        }

        static void WorkingWithList(IDatabase database)
        {

            Console.WriteLine("------------------");
            Console.WriteLine("WorkingWithList");
            Console.WriteLine("------------------");

            database.KeyDelete(MyKey);

            database.ListRightPush(MyKey, 1);
            database.ListRightPush(MyKey, 2);
            database.ListRightPush(MyKey, 3);
            database.ListRightPush(MyKey, 4);
            database.ListRightPush(MyKey, 5);

            var myList = database.ListRange(MyKey);

            foreach (var item in myList)
            {
                Console.Write(item + " ");
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        static void WorkingWithSets(IDatabase database)
        {
            Console.WriteLine("------------------");
            Console.WriteLine("WorkingWithSets");
            Console.WriteLine("------------------");

            database.KeyDelete(MyKey);

            database.SetAdd(MyKey, "A");
            database.SetAdd(MyKey, "B");
            database.SetAdd(MyKey, "A");
            database.SetAdd(MyKey, "C");
            database.SetAdd(MyKey, "C");
            database.SetAdd(MyKey, "D");
            database.SetAdd(MyKey, "F");
            database.SetAdd(MyKey, "E");

            var mySetMembers = database.SetMembers(MyKey);
            var mySetLength = database.SetLength(MyKey);

            Console.WriteLine($"Total members: {mySetLength}");

            foreach (var item in mySetMembers)
            {
                Console.Write(item + " ");
            }

            Console.WriteLine();
            Console.WriteLine();
        }
    }
}