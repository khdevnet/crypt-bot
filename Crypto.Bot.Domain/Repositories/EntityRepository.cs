using Crypto.Bot.Domain.Database;
using Crypto.Bot.Domain.Entity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Bot.Domain.Repositories
{
    public class EntityRepository<TEntity> where TEntity: IEntity
    {
        private readonly MongoClient mc;
        private readonly string collectionName;

        private IMongoDatabase Db => mc.GetDatabase(DbConst.BotDbName);
        private IMongoCollection<TEntity> Listings => Db.GetCollection<TEntity>(collectionName);

        public EntityRepository(MongoClient mc, string collectionName)
        {
            this.mc = mc;
            this.collectionName = collectionName;
        }

        public List<TEntity> GetAll(long chatId)
        {
            return Listings.AsQueryable().Where(l => l.ChatId == chatId).ToList();
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            return Listings.AsQueryable().ToListAsync();
        }

        public async Task InsertAsync(IEnumerable<TEntity> listings)
        {
            await Listings.InsertManyAsync(listings);
        }

        public async Task DeleteAsync(long chatId, IEnumerable<string> names)
        {
            await Listings.DeleteManyAsync(f => f.ChatId == chatId && names.Contains(f.Name));
        }
    }
}
