using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DeamonTool.Helper;

namespace DeamonTool.Repository
{
    public abstract class MongoBaseProvider<T> where T : class
    {

        /// <summary>
        /// 默认数据库名
        /// 如果连接字符串未指定数据库名, 则使用该数据库名
        /// </summary>
        public string DefaultDBName = string.Empty;

        private string databaseName = string.Empty;

        public string DatabaseName
        {
            get
            {
                if (string.IsNullOrEmpty(databaseName))
                {
                    return DefaultDBName;
                }
                return databaseName;
            }
            private set { this.databaseName = value; }
        }
        public string collectionName = "NoName";
        public MongoClient client = null;
        public IMongoDatabase database = null;
        public MongoBaseProvider(string conUrl)
        {

            var mongoConnectionUrl = new MongoUrl(conUrl);
            if (!string.IsNullOrEmpty(mongoConnectionUrl.DatabaseName))
            {
                DatabaseName = mongoConnectionUrl.DatabaseName;
            }

            var setting = MongoClientSettings.FromUrl(mongoConnectionUrl);
            client = new MongoClient(setting);
        }

        public IMongoCollection<T> GetCollection()
        {
            var database = client.GetDatabase(DatabaseName);
            return database.GetCollection<T>(collectionName);
        }

        #region 新增
       
        public bool Insert(T model)
        {
            try
            {
                var database = client.GetDatabase(DatabaseName);
                var myCollection = database.GetCollection<T>(collectionName);
                var task = myCollection.InsertOneAsync(model);
                task.Wait();
                return task.IsCompleted;
            }
            catch (Exception ex)
            {
                Log.LogError("Insert one:" + ex.Message);
                throw;
            }
        }


        #endregion

        #region 修改

        public UpdateResult UpdateOne(Expression<Func<T, bool>> filter, UpdateDefinition<T> update, bool isUpsert = false)
        {
            try
            {
                var database = client.GetDatabase(DatabaseName);
                var myCollection = database.GetCollection<T>(collectionName);
                UpdateOptions options = new UpdateOptions() { IsUpsert = isUpsert };

                return myCollection.UpdateOneAsync<T>(filter, update, options).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Log.LogError("UpdateOne:" + ex.Message);
                throw;
                //  return null;
            }
        }

        public UpdateResult UpdateAll(Expression<Func<T, bool>> filter, UpdateDefinition<T> update, bool isUpsert = false)
        {
            try
            {
                var database = client.GetDatabase(DatabaseName);
                var myCollection = database.GetCollection<T>(collectionName);
                UpdateOptions options = new UpdateOptions() { IsUpsert = isUpsert };
                return myCollection.UpdateManyAsync<T>(filter, update, options).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Log.LogError("UpdateAll:" + ex.Message);
                throw;
                // return null;
            }

        }

        #endregion

        #region 删除
      
        public DeleteResult DeleteOne(Expression<Func<T, bool>> filter)
        {
            try
            {
                var database = client.GetDatabase(DatabaseName);
                var myCollection = database.GetCollection<T>(collectionName);
                return myCollection.DeleteOneAsync<T>(filter).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Log.LogError("DeleteOne:" + ex.Message);
                throw;

            }
        }
       
        public DeleteResult DeleteMany(Expression<Func<T, bool>> filter)
        {
            try
            {
                var database = client.GetDatabase(DatabaseName);
                var myCollection = database.GetCollection<T>(collectionName);
                return myCollection.DeleteManyAsync<T>(filter).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Log.LogError("DeleteMany:" + ex.Message);
                throw;

            }
        }

        #endregion

        #region  查询
        public List<T> Find(Expression<Func<T, bool>> filter, FindOptions<T, T> options = null)
        {
            var result = new List<T>();
            try
            {
                var database = client.GetDatabase(DatabaseName);
                var myCollection = database.GetCollection<T>(collectionName);
                result = myCollection.FindAsync<T>(filter, options).GetAwaiter().GetResult().ToListAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Log.LogError("Find:" + ex.Message);
                throw;
            }
            return result;
        }

    
        public List<T> Aggregate(PipelineDefinition<T, T> pipeline, AggregateOptions options = null)
        {
            var result = new List<T>();
            try
            {
                var database = client.GetDatabase(DatabaseName);
                var myCollection = database.GetCollection<T>(collectionName);
                result = myCollection.AggregateAsync<T>(pipeline, options).GetAwaiter().GetResult().ToListAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Log.LogError("Aggregate:" + ex.Message);
                throw;
            }
            return result;
        }
        
      
        public T FindOne(Expression<Func<T, bool>> filter, FindOptions<T, T> options = null)
        {
            try
            {
                options = options ?? new FindOptions<T, T>();
                options.Limit = 1;
                if (options == null)
                {
                    options = new FindOptions<T, T>() { Limit = 1 };
                }
                else
                {
                    options.Limit = 1;
                }
                var result = Find(filter, options);
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.LogError("FindOne:" + ex.Message);
                throw;
            }
        }

        public long Count(Expression<Func<T, bool>> filter)
        {
            try
            {
                var database = client.GetDatabase(DatabaseName);
                var myCollection = database.GetCollection<T>(collectionName);
                return myCollection.CountAsync<T>(filter).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Log.LogError("Count:" + ex.Message);
                throw;

            }
        }

      
        /// <summary>
        /// 统计某字段的和
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <param name="sumKeyField">BSon关键词字段</param>
        /// <param name="sumField">BSon被统计字段</param>
        /// <returns></returns>
        public long Sum(Expression<Func<T, bool>> filter, string sumKeyField, string sumField)
        {
            try
            {
                var database = client.GetDatabase(DatabaseName);
                var myCollection = database.GetCollection<T>(collectionName);

                var aggregate = myCollection.Aggregate()
                    .Match(filter)
                    .Group(new BsonDocument { { "_id", "$" + sumKeyField }, { "sum", new BsonDocument("$sum", "$" + sumField) } });

                var results = aggregate.ToListAsync().GetAwaiter().GetResult();

                if (results.Count == 1)
                    return (long)results[0].GetElement("sum").Value;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Log.LogError("sum:" + ex.Message);
                throw;
            }
        }

        #endregion


        #region 批量写入
        public BulkWriteResult BulkWrite(IEnumerable<WriteModel<T>> models)
        {
            try
            {
                var database = client.GetDatabase(DatabaseName);
                var myCollection = database.GetCollection<T>(collectionName);
                return myCollection.BulkWriteAsync(models).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Log.LogError("BulkWrite:" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool BulkWriteOne(T entity)
        {
            InsertOneModel<T> model = new InsertOneModel<T>(entity);
            var inserResult = this.BulkWrite(new[] { model });
            if (inserResult != null && inserResult.InsertedCount == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion


        #region 辅助方法

        public bool IsObjectId(string id)
        {
            try
            {
                ObjectId obj;
                return ObjectId.TryParse(id, out obj);
            }
            catch
            {
                return false;
            }
        }
        #endregion

    }
}
