namespace SnackMachine.MongoDbPersistence
{
    public sealed class MongoDbConfiguration
    {
        public string ConnectionString { get; set; } = "mongodb://localhost:27017";

        public string DatabaseName { get; set; } = "SnackMachine";

        public string SnacksCollectionName { get; set; } = "Snacks";

        public string AccountCollectionName { get; set; } = "Accounts";
    }
}