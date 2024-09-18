using MongoDB.Bson;
using MongoDB.Driver;
using System.Xml.Linq;

internal class Program
{
  private static async Task Main(string[] args)
  {
    const string connectionString = "mongodb+srv://max:12345@cluster0.5ozyvnm.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";

    var settings = MongoClientSettings.FromConnectionString(connectionString);

    settings.ServerApi = new ServerApi(ServerApiVersion.V1);

    var client = new MongoClient(settings);

    try
    {
      var result = client.GetDatabase("test").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
      Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
    }

    using (var cursor = await client.ListDatabasesAsync())
    {
      var databases = cursor.ToList();
      foreach (var database in databases)
      {
        Console.WriteLine(database);
      }
    }

    var db = client.GetDatabase("test");
    await db.CreateCollectionAsync("people", cancellationToken: new CancellationToken());

    var collection = db.GetCollection<BsonDocument>("people");

    BsonElement el1 = new BsonElement("person1", new BsonString("Tom"));

    BsonElement el2 = new BsonElement("person2", new BsonString("Alex"));

    BsonElement el3 = new BsonElement("dateTime", new BsonDateTime(DateTime.Now));

    BsonDocument doc = new BsonDocument(el1);
    doc.Add(el2);
    doc.Add(el3);

    collection.InsertOne(doc);
  }
}