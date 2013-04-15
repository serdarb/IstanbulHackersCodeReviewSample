namespace AgileWall.Domain.Repo
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;

    using Utils;
    using Entity;

    public class BaseRepo<TEntity> : IBaseRepo<TEntity> where TEntity : BaseEntity
    {
        private readonly MongoDatabase _mongoDatabase;
        private readonly MongoCollection<TEntity> _collection;


        public BaseRepo(string dbName = Consts.DBName)
        {
            _mongoDatabase = new MongoClient().GetServer().GetDatabase(dbName);
            _collection = _mongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name, WriteConcern.Acknowledged);
        }

        public MongoCursor<TEntity> FindAll()
        {
            return _collection.FindAllAs<TEntity>();
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return _collection.AsQueryable().Where(x => !x.IsDeleted);
        }

        public IQueryable<TEntity> AsOrderedQueryable()
        {
            if (!_collection.IndexExists("CreatedOn"))
            {
                _collection.CreateIndex("CreatedOn");
            }

            return AsQueryable().OrderByDescending(x => x.CreatedOn);
        }

        public TEntity GetSingle(string id)
        {
            return GetSingle(x => x.Id == ObjectId.Parse(id));
        }

        public TEntity GetSingle(Expression<Func<TEntity, bool>> expression)
        {
            return AsQueryable().Where(expression).FirstOrDefault();
        }

        public WriteConcernResult Add(TEntity entity)
        {
            return _collection.Insert(entity);
        }

        public void AddBulk(IEnumerable<TEntity> entities)
        {
            _collection.InsertBatch(entities);
        }

        public WriteConcernResult Delete(TEntity entity)
        {
            return _collection.Update(Query<TEntity>.EQ(x => x.Id, entity.Id),
                                      Update<TEntity>.Set(x => x.DeletedOn, entity.DeletedOn)
                                                     .Set(x => x.DeletedBy, entity.DeletedBy)
                                                     .Set(x => x.IsDeleted, true));
        }

        public WriteConcernResult DeleteAll()
        {
            return _collection.RemoveAll();
        }

        public WriteConcernResult Update(IMongoQuery mongoQuery, IMongoUpdate mongoUpdate)
        {
            return _collection.Update(mongoQuery, mongoUpdate);
        }
    }
}