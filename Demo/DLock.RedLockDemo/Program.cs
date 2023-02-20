// See https://aka.ms/new-console-template for more information
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

var cfg = new ConfigurationOptions()
{
    AllowAdmin = true,
};

cfg.EndPoints.Add("localhost");

var multiplexer = ConnectionMultiplexer.Connect(cfg);

var multiplexers = new List<RedLockMultiplexer>
{
    multiplexer
};

const string MyKey = "SE130097";
var redLockFactory = RedLockFactory.Create(multiplexers);
var database = multiplexer.GetDatabase();
database.KeyDelete(MyKey);

Parallel.For(0, 7, (threadId) =>
{
    while (true)
    {
        using (var redLock = redLockFactory.CreateLock(
            resource: MyKey,
            expiryTime: TimeSpan.FromSeconds(60),
            waitTime: TimeSpan.FromSeconds(10),
            retryTime: TimeSpan.FromSeconds(0.5)))
        {
            IncreaseNumber(database, threadId);
        }

        Thread.Sleep(new Random().Next(1000));
    }
});

static void IncreaseNumber(IDatabase database, int threadId)
{
    if (!int.TryParse(database.StringGet(MyKey), out int value))
    {
        value = 0;
    }

    Console.WriteLine($"Thread {threadId}: {value}");

    value += 1;

    database.StringSet(MyKey, value);
}