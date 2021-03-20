using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using LShort.Common.Database.Attributes;
using LShort.Common.Logging;
using LShort.Common.Models;
using SqlKata;
using SqlKata.Execution;

namespace LShort.Common.Database.Implementation
{
    public abstract class SqlRepository<TModel> : IRepository<TModel> where TModel : ModelBase
    {
        protected ISqlDatabase db;

        protected string TableName => ((Table) GetType().GetCustomAttribute(typeof(Table)))?.Name;

        protected Query Table => db.Query(TableName);

        protected PropertyInfo[] Properties => typeof(TModel).GetProperties();

        protected IAppLogger logger;

        public SqlRepository(ISqlDatabase db, IAppLogger logger)
        {
            this.db = db;
            this.logger = logger.FromSource(typeof(SqlRepository<TModel>));
            
            EnsureSchema();
        }

        protected virtual void EnsureSchema()
        {
            var query = new StringBuilder();
            query.AppendLine($"CREATE TABLE [IF NOT EXISTS] {TableName} (");
            query.AppendLine("Id uuid PRIMARY KEY,");

            foreach (var prop in Properties.Where(prop => prop.Name != "Id"))
            {
                query.AppendLine($"{prop.Name} {db.Map(prop.PropertyType)},");
            }
            
            query.Append(");");

            var result = db.Statement(query.ToString());

            if (result > 0)
            {
                logger.Information($"Table schema generated successfully for {TableName}.");
            }
        }

        public async Task<TModel> Get(Guid id)
        {
            return await Table.Where("Id", id).FirstOrDefaultAsync<TModel>();
        }

        public async Task<IList<TModel>> GetAll()
        {
            return (await Table.GetAsync<TModel>()).ToList();
        }

        public async Task<RepositoryResult> Insert(TModel obj)
        {
            try
            {
                var rowsAffected = await Table.InsertAsync(obj);
                if (rowsAffected < 1)
                {
                    logger.Error("Failed to insert object into the table.", obj);
                    return RepositoryResult.Failure;
                }

                return RepositoryResult.Success;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                return RepositoryResult.Failure;
            }
        }

        public async Task<RepositoryResult> BulkInsert(IList<TModel> docs)
        {
            try
            {
                var cols = Properties.Select(prop => prop.Name);
                var data = docs.Select(ConvertObjectToList);
                
                var rowsAffected = await Task.Run(() => Table.Insert(cols, data)); // bulk insert doesn't work with InsertAsync, lame

                switch (rowsAffected)
                {
                    case 0:
                        logger.Error("Failed to insert objects into the table.");
                        return RepositoryResult.Failure;
                    case var _ when rowsAffected < docs.Count:
                        logger.Warning($"Failed to insert {docs.Count - rowsAffected} objects out of {docs.Count}.");
                        return RepositoryResult.PartialFailure;
                    default:
                        return RepositoryResult.Success;
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                return RepositoryResult.Failure;
            }
        }

        protected IList<object> ConvertObjectToList(TModel obj)
        {
            return Properties.Select(prop => prop.GetValue(obj)).ToList();
        } 

        public async Task<RepositoryResult> Update(TModel newDoc)
        {
            try
            {
                var rowsAffected = await Table.Where("Id", newDoc.Id).UpdateAsync(newDoc);
                if (rowsAffected < 1)
                {
                    logger.Error("Failed to update object in the table.", newDoc);
                    return RepositoryResult.Failure;
                }

                return RepositoryResult.Success;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                return RepositoryResult.Failure;
            }
        }

        public async Task<RepositoryResult> BulkUpdate(IList<TModel> newObjs)
        {
            var tasks = newObjs.Select(Update);

            var results = await Task.WhenAll(tasks);

            var failCount = results.Count(result => !result.IsSuccess());

            return failCount switch
            {
                0 => RepositoryResult.Success,
                _ when failCount == newObjs.Count => RepositoryResult.Failure,
                _ => RepositoryResult.PartialFailure
            };
        }

        public async Task<RepositoryResult> Delete(Guid id)
        {
            try
            {
                var rowsAffected = await Table.Where("Id", id).DeleteAsync();
                if (rowsAffected < 1)
                {
                    logger.Error("Failed to delete object from the table.");
                    return RepositoryResult.Failure;
                }

                return RepositoryResult.Success;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                return RepositoryResult.Failure;
            }
        }

        public async Task<RepositoryResult> BulkDelete(IList<Guid> ids)
        {
            try
            {
                var rowsAffected = await Table.WhereIn("Id", ids).DeleteAsync();

                switch (rowsAffected)
                {
                    case 0:
                        logger.Error("Failed to delete objects from the table.");
                        return RepositoryResult.Failure;
                    case var _ when rowsAffected < ids.Count:
                        logger.Warning($"Failed to delete {ids.Count - rowsAffected} objects out of {ids.Count}.");
                        return RepositoryResult.PartialFailure;
                    default:
                        return RepositoryResult.Success;
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                return RepositoryResult.Failure;
            }
        }
    }
}