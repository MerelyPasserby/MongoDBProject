using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_3
{
    internal class MongoDBService
    {
        private readonly IMongoDatabase _database;

        public MongoDBService(string connectionString, string dbName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(dbName);
        }
        
        public IMongoCollection<TEntity> GetCollection<TEntity>(string collectionName)
        {
            return _database.GetCollection<TEntity>(collectionName);
        }
        
        public void Insert<TEntity>(string collectionName, TEntity entity)
        {
            var collection = GetCollection<TEntity>(collectionName);
            collection.InsertOne(entity);
        }
       
        public List<TEntity> GetAll<TEntity>(string collectionName)
        {
            var collection = GetCollection<TEntity>(collectionName);
            return collection.Find(Builders<TEntity>.Filter.Empty).ToList();
        }
        
        public List<TEntity> Find<TEntity>(string collectionName, FilterDefinition<TEntity> filter)
        {
            var collection = GetCollection<TEntity>(collectionName);
            return collection.Find(filter).ToList();
        }
      
        public bool Update<TEntity>(string collectionName, FilterDefinition<TEntity> filter,TEntity entity)
        {
            var collection = GetCollection<TEntity>(collectionName);
            return collection.ReplaceOne(filter, entity).ModifiedCount == 1;
        }

        public bool Delete<TEntity>(string collectionName, FilterDefinition<TEntity> filter)
        {
            var collection = GetCollection<TEntity>(collectionName);
            return collection.DeleteOne(filter).DeletedCount == 1;
        }

    }
}
