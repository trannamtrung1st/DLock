using StackExchange.Redis;

var cfg = new ConfigurationOptions()
{
    AllowAdmin = true,
};

cfg.EndPoints.Add("localhost:6379");

var connection = ConnectionMultiplexer.Connect(cfg);

var database = connection.GetDatabase();

string key = string.Empty; string value = string.Empty;
bool stop = false;

do
{
    Console.Write("Input key: ");
    key = Console.ReadLine();
    Console.Write("Input value: ");
    value = Console.ReadLine();

    if (!string.IsNullOrEmpty(key))
    {
        database.StringSet(key, value);
        Console.Clear();
        Console.Write($"Sucessfully set {key}={value}\n\n===========\n\n");
    }
    else
    {
        stop = true;
    }

} while (!stop);

var server = connection.GetServers().FirstOrDefault();

if (server != null)
{
    server.FlushAllDatabases();
}