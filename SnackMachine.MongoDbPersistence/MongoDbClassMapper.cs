using MongoDB.Bson.Serialization;
using SnackMachine.Domain.SnackAggregate;

namespace SnackMachine.MongoDbPersistence
{
    public static class MongoDbClassMapper
    {
        public static void RegisterClassMaps()
        {
            BsonClassMap.RegisterClassMap<Snack>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(x => x.Id);
            });
        }
    }
}