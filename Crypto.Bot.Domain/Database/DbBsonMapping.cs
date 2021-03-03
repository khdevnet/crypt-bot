using Crypto.Bot.Domain.Entity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Crypto.Bot.Domain.Database
{
    public static class DbBsonMapping
    {
        public static void MapEntities()
        {
            var pack = new ConventionPack();
            pack.Add(new CamelCaseElementNameConvention());
            pack.Add(new EnumRepresentationConvention(BsonType.String));

            ConventionRegistry.Register(
               "CamelCaseElementNameConvention",
               pack,
               t => t.FullName.StartsWith("Crypto.Bot.Domain."));

            BsonClassMap.RegisterClassMap<Coin>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c._Id).SetIdGenerator(StringObjectIdGenerator.Instance);
            });

            BsonClassMap.RegisterClassMap<Listing>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c._Id).SetIdGenerator(StringObjectIdGenerator.Instance);
            });

            BsonClassMap.RegisterClassMap<PriceAlert>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c._Id).SetIdGenerator(StringObjectIdGenerator.Instance);
            });
        }
    }
}
