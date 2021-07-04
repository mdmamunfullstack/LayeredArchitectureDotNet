// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.Base.Auditing;
using LayeredArchitecture.Base.Exensions;
using LayeredArchitecture.Base.Identity;
using LayeredArchitecture.DataAccess.Base;
using LayeredArchitecture.DataAccess.Base.Model;
using LayeredArchitecture.DataAccess.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LayeredArchitecture.DataAccess.Dao
{
    public abstract class DaoBase<TEntityType> : IDao<TEntityType>
        where TEntityType : class
    {
        private readonly SqlClient sqlClient;
        private readonly IUserAccessor userAccessor;
        private readonly IDbConnectionScopeResolver dbConnectionScopeResolver;

        public DaoBase(IDbConnectionScopeResolver dbConnectionScopeResolver, IUserAccessor userAccessor, SqlClient sqlClient) 
        {
            this.dbConnectionScopeResolver = dbConnectionScopeResolver;
            this.userAccessor = userAccessor;
            this.sqlClient = sqlClient;
        }

        public async Task<List<TEntityType>> QueryAsync(Func<IQueryable<TEntityType>, IQueryable<TEntityType>> query, CancellationToken cancellationToken = default)
        {
            var result = DbConnectionScope.GetConnection();

            return await sqlClient.QueryAsync(result.Connection, result.Transaction, query, cancellationToken);
        }

        public async Task<List<TEntityType>> SqlQueryAsync(FormattableString sql, Func<IQueryable<TEntityType>, IQueryable<TEntityType>> config = null, CancellationToken cancellationToken = default)
        {
            var result = DbConnectionScope.GetConnection();

            return await sqlClient.SqlQuery(result.Connection, result.Transaction, sql, config, cancellationToken);
        }

        public async Task<int> ExecuteRawSqlAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken)
        {
            var result = DbConnectionScope.GetConnection();

            return await sqlClient.ExecuteRawSqlAsync(result.Connection, result.Transaction, sql, parameters, cancellationToken);
        }

        public async Task<int> ExecuteRawSqlInterpolatedAsync(FormattableString sql, CancellationToken cancellationToken)
        {
            var result = DbConnectionScope.GetConnection();

            return await sqlClient.ExecuteRawSqlInterpolatedAsync(result.Connection, result.Transaction, sql, cancellationToken);
        }

        public async Task InsertAsync(TEntityType entity, CancellationToken cancellationToken = default)
        {   
            var result = DbConnectionScope.GetConnection();

            await HandleAuditedEntity (result, entity, DataChangeOperationEnum.Insert, cancellationToken);
            await sqlClient.InsertAsync(result.Connection, result.Transaction, entity, cancellationToken);
        }

        public async Task InsertRangeAsync(IEnumerable<TEntityType> entities, CancellationToken cancellationToken = default)
        {
            var result = DbConnectionScope.GetConnection();

            await HandleAuditedEntities(result, entities, DataChangeOperationEnum.Insert, cancellationToken);
            await sqlClient.InsertRangeAsync(result.Connection, result.Transaction, entities, cancellationToken);
        }

        public async ValueTask<TEntityType> FindAsync(object[] keyValues, CancellationToken cancellationToken = default)
        {
            var result = DbConnectionScope.GetConnection();

            return await sqlClient.FindAsync<TEntityType>(result.Connection, result.Transaction, keyValues, cancellationToken);
        }

        public async ValueTask<TEntityType> FindAsync(params object[] keyValues)
        {
            var result = DbConnectionScope.GetConnection();

            return await sqlClient.FindAsync<TEntityType>(result.Connection, result.Transaction, keyValues);
        }

        public async Task DeleteAsync(TEntityType entity, CancellationToken cancellationToken = default)
        {
            var result = DbConnectionScope.GetConnection();

            await HandleAuditedEntity(result, entity, DataChangeOperationEnum.Delete, cancellationToken);
            await sqlClient.DeleteAsync(result.Connection, result.Transaction, entity, cancellationToken);
        }

        public async Task DeleteRangeAsync(IEnumerable<TEntityType> entities, CancellationToken cancellationToken = default)
        {
            var result = DbConnectionScope.GetConnection();

            await HandleAuditedEntities(result, entities, DataChangeOperationEnum.Delete, cancellationToken);
            await sqlClient.DeleteRangeAsync(result.Connection, result.Transaction, entities, cancellationToken);
        }

        public async Task UpdateAsync(TEntityType entity, CancellationToken cancellationToken = default)
        {
            var result = DbConnectionScope.GetConnection();

            await HandleAuditedEntity(result, entity, DataChangeOperationEnum.Update, cancellationToken);
            await sqlClient.UpdateAsync(result.Connection, result.Transaction, entity, cancellationToken);
        }

        public async Task UpdateRangeAsync(IEnumerable<TEntityType> entities, CancellationToken cancellationToken = default)
        {
            var result = DbConnectionScope.GetConnection();

            await HandleAuditedEntities(result, entities, DataChangeOperationEnum.Update, cancellationToken);
            await sqlClient.UpdateRangeAsync(result.Connection, result.Transaction, entities, cancellationToken);
        }

        protected DbConnectionScope DbConnectionScope => dbConnectionScopeResolver.Resolve();

        #region Audit Information

        private async Task HandleAuditedEntity(ConnectionTransactionHolder connectionTransactionHolder, TEntityType entity, DataChangeOperationEnum operation, CancellationToken cancellationToken)
        {
            await HandleAuditedEntities(connectionTransactionHolder, new[] { entity }, operation, cancellationToken);
        }

        private async Task HandleAuditedEntities(ConnectionTransactionHolder connectionTransactionHolder, IEnumerable<TEntityType> entities, DataChangeOperationEnum operation, CancellationToken cancellationToken)
        {
            var isAuditedEntityType = typeof(IAuditedEntity).IsAssignableFrom(typeof(TEntityType));

            if (!isAuditedEntityType)
            {
                return;
            }

            var userId = GetUserId();

            foreach (var entity in entities)
            {
                ((IAuditedEntity)entity).AuditUser = userId;
                ((IAuditedEntity)entity).AuditOperation = operation;
                ((IAuditedEntity)entity).EntityVersion = ((IAuditedEntity)entity).EntityVersion + 1;
            }

            // This isn't probably the best way. When we have an audited entity we need to update the deleted entities first, so the deleted operation
            // is reflected in the Temporal Table when actually deleting the entity. This is done to keep us from maintaining yet another Audit-table,
            // when we could keep this information simply in the audited record itself.
            //
            // The Entity Version won't increase for the Operation.
            if (operation == DataChangeOperationEnum.Delete)
            {
                await sqlClient.UpdateRangeAsync(connectionTransactionHolder.Connection, connectionTransactionHolder.Transaction, entities, cancellationToken);
            }
        }

        private string GetUserId()
        {
            if (userAccessor.User == null)
            {
                throw new MissingUserIdException();
            }

            var userId = userAccessor.User.GetUserId();

            if (userId == null)
            {
                throw new MissingUserIdException();
            }

            return userId;
        }

        #endregion
    }
}
