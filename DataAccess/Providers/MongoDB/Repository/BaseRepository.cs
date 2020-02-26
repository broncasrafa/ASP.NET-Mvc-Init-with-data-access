using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using DataAccess.Providers.MongoDB.Interface;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DataAccess.Providers.MongoDB.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : EntityBase
    {
        protected readonly IMongoContext _context;
        protected readonly IMongoCollection<TEntity> Collection;

        protected BaseRepository(IMongoContext context)
        {
            context = new MongoContext();

            _context = context;
            Collection = _context.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        /// <summary>
        /// Inserts a single document into a collection.
        /// </summary>
        /// <param name="entity">Entity type</param>
        public void InsertOne(TEntity entity)
        {
            entity._id = ObjectId.GenerateNewId();
            Collection.InsertOne(entity);
        }

        /// <summary>
        /// Inserts multiple documents into a collection.
        /// </summary>
        /// <param name="entities">List of Entity type</param>
        public void InsertMany(IList<TEntity> entities)
        {
            entities.ToList().ForEach(c => c._id = ObjectId.GenerateNewId());
            Collection.InsertMany(entities);
        }

        /// <summary>
        /// Updates one document within a collection.
        /// </summary>
        /// <param name="entity">Entity type</param>
        public void UpdateOne(Guid id, TEntity entity)
        {
            Collection.ReplaceOne(new BsonDocument("_id", id), entity, new ReplaceOptions { IsUpsert = true });
        }

        /// <summary>
        /// Updates multiple documents within a collection.
        /// </summary>
        /// <param name="entities">List of Entity type</param>
        public void UpdateMany(IList<TEntity> entities)
        {
            entities.ToList().ForEach(doc =>
            {
                Collection.ReplaceOne(c => c._id.Equals(doc._id), doc, new ReplaceOptions { IsUpsert = true });
            });
        }

        /// <summary>
        /// Updates an existing document or inserts a new document, depending on its document parameter.
        /// </summary>
        /// <param name="entity">Entity type</param>
        /// <returns>Entity type inserted or updated</returns>
        public TEntity Save(TEntity entity)
        {
            if(entity._id == null)
            {
                Collection.InsertOne(entity);
                return entity;
            }

            Collection.ReplaceOne(new BsonDocument("_id", entity._id), entity, new ReplaceOptions { IsUpsert = true });
            return entity;
        }

        /// <summary>
        /// Deletes at most a single document that matches a specified filter,
        /// </summary>
        /// <param name="id">id of document</param>
        public void Delete(ObjectId id)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", id);
            Collection.DeleteOne(filter);
        }

        /// <summary>
        /// Deletes at most a single document that matches a specified filter, even though multiple documents may match the specified filter.It will delete the first document.
        /// </summary>
        /// <param name="entity">Entity type</param>
        public void DeleteOne(TEntity entity)
        {
            Collection.DeleteOne(Builders<TEntity>.Filter.Eq("_id", entity._id));
        }

        /// <summary>
        /// Deletes all documents that match a specified filter.
        /// </summary>
        /// <param name="entities">List of Entity type</param>        
        public void DeleteMany(IList<TEntity> entities)
        {
            entities.ToList().ForEach(doc => Collection.DeleteOne(Builders<TEntity>.Filter.Eq("_id", doc._id)));
        }

        /// <summary>
        /// Gets all the documents from a collection.
        /// </summary>
        /// <returns>list of Entities type</returns>
        public IList<TEntity> GetAll()
        {
            return Collection.Find(new BsonDocument()).ToList();
        }

        /// <summary>
        /// Gets all the documents from a collection by specifying the projection parameter 
        /// </summary>
        /// <param name="predicate">projection parameter</param>
        /// <returns>list of Entities type</returns>
        public IList<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return Collection.Find(predicate) .ToList();
        }

        /// <summary>
        /// Search for all the documents from a collection by specifying the projection parameter
        /// </summary>
        /// <param name="predicate">projection parameter</param>
        /// <returns>list of Entities type</returns>
        public IList<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate)
        {
            return Collection.AsQueryable<TEntity>().Where(predicate.Compile()).ToList();
        }

        /// <summary>
        /// Get a single document from a collection.
        /// </summary>
        /// <param name="predicate">projection parameter</param>
        /// <returns>Entity type</returns>
        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return Collection.Find(predicate).FirstOrDefault();
        }

        /// <summary>
        /// Get a single document from a collection that matches a specified filter
        /// </summary>
        /// <param name="id">id of document</param>
        /// <returns>a document as an Entity type</returns>
        public TEntity GetById(ObjectId id)
        {
            return Collection.Find(doc => doc._id == id).FirstOrDefault();
        }

        /// <summary>
        /// Get a single document from a collection that matches a specified filter
        /// </summary>
        /// <param name="predicate">projection filter parameter</param>
        /// <returns>a single document from collection as an Entity type</returns>
        public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return Collection.AsQueryable<TEntity>().Where(predicate.Compile()).SingleOrDefault();
        }

        /// <summary>
        /// Completely empties the collection by deleting the collection itself. 
        /// While this is not exactly deleting a document, it can be considered 
        /// as a way of cleaning up the data.
        /// </summary>
        /// <returns>true if collections was cleaned successfully</returns>
        public bool DropCollection()
        {
            try
            {
                _context.GetDatabase().DropCollection(typeof(TEntity).Name);
                return true;

            }
            catch (Exception)
            {
                return false;                
            }
        }


        #region Async Methods
        /// <summary>
        /// Inserts a single document into a collection.
        /// </summary>
        /// <param name="entity">Entity type</param>
        public async Task InsertOneAsync(TEntity entity)
        {
            entity._id = ObjectId.GenerateNewId();
            await Collection.InsertOneAsync(entity);
        }

        /// <summary>
        /// Inserts multiple documents into a collection.
        /// </summary>
        /// <param name="entities">List of Entity type</param>
        public async Task InsertManyAsync(IList<TEntity> entities)
        {
            entities.ToList().ForEach(c => c._id = ObjectId.GenerateNewId());
            await Collection.InsertManyAsync(entities);
        }

        /// <summary>
        /// Updates one document within a collection.
        /// </summary>
        /// <param name="entity">Entity type</param>
        public async Task UpdateOneAsync(Guid id, TEntity entity)
        {
            await Collection.ReplaceOneAsync(new BsonDocument("_id", id), entity, new ReplaceOptions { IsUpsert = true });
        }

        /// <summary>
        /// Updates multiple documents within a collection.
        /// </summary>
        /// <param name="entities">List of Entity type</param>
        public async Task UpdateManyAsync(IList<TEntity> entities)
        {
            var updates = new List<WriteModel<TEntity>>();
            var filterBuilder = Builders<TEntity>.Filter;
            foreach(var doc in entities)
            {
                foreach (PropertyInfo prop in typeof(TEntity).GetProperties())
                {
                    if (prop.Name == "_id")
                    {
                        var filter = filterBuilder.Eq(prop.Name, prop.GetValue(doc));
                        updates.Add(new ReplaceOneModel<TEntity>(filter, doc));
                        break;
                    }
                }
            }
            BulkWriteResult result = await Collection.BulkWriteAsync(updates);
            // return result.ModifiedCount.ToString();
        }

        /// <summary>
        /// Updates an existing document or inserts a new document, depending on its document parameter.
        /// </summary>
        /// <param name="entity">Entity type</param>
        /// <returns>Entity type inserted or updated</returns>
        public async Task<TEntity> SaveAsync(TEntity entity)
        {
            if (entity._id == null)
            {
                await Collection.InsertOneAsync(entity);
                return entity;
            }

            await Collection.ReplaceOneAsync(new BsonDocument("_id", entity._id), entity, new ReplaceOptions { IsUpsert = true });
            return entity;
        }

        /// <summary>
        /// Deletes at most a single document that matches a specified filter,
        /// </summary>
        /// <param name="id">id of document</param>
        public async Task DeleteAsync(ObjectId id)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", id);
            await Collection.DeleteOneAsync(filter);
        }

        /// <summary>
        /// Deletes at most a single document that matches a specified filter, even though multiple documents may match the specified filter.It will delete the first document.
        /// </summary>
        /// <param name="entity">Entity type</param>
        public async Task DeleteOneAsync(TEntity entity)
        {
            await Collection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", entity._id));
        }

        /// <summary>
        /// Deletes all documents that match a specified filter.
        /// </summary>
        /// <param name="entities">List of Entity type</param>        
        public async Task DeleteManyAsync(IList<TEntity> entities)
        {
            var removes = new List<WriteModel<TEntity>>();
            var filterBuilder = Builders<TEntity>.Filter;
            foreach (var doc in entities)
            {
                foreach (PropertyInfo prop in typeof(TEntity).GetProperties())
                {
                    if (prop.Name == "_id")
                    {
                        var filter = filterBuilder.Eq(prop.Name, prop.GetValue(doc));
                        removes.Add(new DeleteOneModel<TEntity>(filter));
                        break;
                    }
                }
            }
            BulkWriteResult result = await Collection.BulkWriteAsync(removes);
            // return result.ModifiedCount.ToString();
        }

        /// <summary>
        /// Gets all the documents from a collection.
        /// </summary>
        /// <returns>list of Entities type</returns>
        public async Task<IList<TEntity>> GetAllAsync()
        {
            return await Collection.Find(new BsonDocument()).ToListAsync();
        }

        /// <summary>
        /// Gets all the documents from a collection by specifying the projection parameter 
        /// </summary>
        /// <param name="predicate">projection parameter</param>
        /// <returns>list of Entities type</returns>
        public async Task<IList<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Collection.Find(predicate).ToListAsync();
        }

        /// <summary>
        /// Search for all the documents from a collection by specifying the projection parameter
        /// </summary>
        /// <param name="predicate">projection parameter</param>
        /// <returns>list of Entities type</returns>
        public async Task<IList<TEntity>> SearchForAsync(Expression<Func<TEntity, bool>> predicate)
        {
            IMongoQueryable<TEntity> lista = Collection.AsQueryable<TEntity>();

            return await lista.Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Get a single document from a collection.
        /// </summary>
        /// <param name="predicate">projection parameter</param>
        /// <returns>Entity type</returns>
        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Collection.Find(predicate).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get a single document from a collection that matches a specified filter
        /// </summary>
        /// <param name="id">id of document</param>
        /// <returns>a document as an Entity type</returns>
        public async Task<TEntity> GetByIdAsync(ObjectId id)
        {
            return await Collection.Find(doc => doc._id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get a single document from a collection that matches a specified filter
        /// </summary>
        /// <param name="predicate">projection filter parameter</param>
        /// <returns>a single document from collection as an Entity type</returns>
        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            IMongoQueryable<TEntity> lista = Collection.AsQueryable<TEntity>();

            return await lista.Where(predicate).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Completely empties the collection by deleting the collection itself. 
        /// While this is not exactly deleting a document, it can be considered 
        /// as a way of cleaning up the data.
        /// </summary>
        /// <returns>true if collections was cleaned successfully</returns>
        public async Task<bool> DropCollectionAsync()
        {
            try
            {
                await _context.GetDatabase().DropCollectionAsync(typeof(TEntity).Name);
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
