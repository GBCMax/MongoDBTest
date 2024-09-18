using MongoDB.Bson;
using MongoDB.Driver;

internal class Program
{
  private static async Task Main(string[] args)
  {
    const string connectionString = "mongodb+srv://max:12345@cluster0.5ozyvnm.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";

    var settings = MongoClientSettings.FromConnectionString(connectionString);

    settings.ServerApi = new ServerApi(ServerApiVersion.V1);

    var client = new MongoClient(settings);

    using (var cursor = await client.ListDatabasesAsync())
    {
      var databases = cursor.ToList();
      foreach (var database in databases)
      {
        Console.WriteLine(database);
      }
    }

    try
    {
      var result = client.GetDatabase("test").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
      Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
    }
  }
}