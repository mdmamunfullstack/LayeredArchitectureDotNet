// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LayeredArchitecture.DataAccess.Dao
{
    /// <summary>
    /// The Data Access Object.
    /// </summary>
    /// <typeparam name="TEntityType">The Type of Entity for this DAO</typeparam>
    public interface IDao<TEntityType>
         where TEntityType : class
    {
        /// <summary>
        /// Inserts a <see cref="TEntityType"/>.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        Task InsertAsync(TEntityType entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Inserts a range of <see cref="TEntityType"/>.
        /// </summary>
        /// <param name="entities">Entities to insert</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        Task InsertRangeAsync(IEnumerable<TEntityType> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds a <see cref="TEntityType"/> by its Primary Key.
        /// </summary>
        /// <param name="keyValues">Primary Key</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        ValueTask<TEntityType> FindAsync(object[] keyValues, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds a <see cref="TEntityType"/> by its Primary Key.
        /// </summary>
        /// <param name="keyValues">Primary Key</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        ValueTask<TEntityType> FindAsync(params object[] keyValues);

        /// <summary>
        /// Deletes a <see cref="TEntityType"/> from the database.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        Task DeleteAsync(TEntityType entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a range of <see cref="TEntityType"/> from the database.
        /// </summary>
        /// <param name="entities">Entities</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        Task DeleteRangeAsync(IEnumerable<TEntityType> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates a <see cref="TEntityType"/>.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        Task UpdateAsync(TEntityType entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates a range of <see cref="TEntityType"/>.
        /// </summary>
        /// <param name="entities">Entities</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        Task UpdateRangeAsync(IEnumerable<TEntityType> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Queries the Database asynchronously for a List of <see cref="TEntityType"/> based on an <see cref="IQueryable{TEntityType}"/> 
        /// passed to the method.
        /// </summary>
        /// <param name="query">Query to execute</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Results</returns>
        Task<List<TEntityType>> QueryAsync(Func<IQueryable<TEntityType>, IQueryable<TEntityType>> query = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Queries the Database asynchronously for a List of <see cref="TEntityType"/> based on a raw SQL Query. The result can be further 
        /// filtered or extended using the <paramref name="config"/> parameter.
        /// </summary>
        /// <param name="sql">SQL Query</param>
        /// <param name="config">Additional Query</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Results</returns>
        Task<List<TEntityType>> SqlQueryAsync(FormattableString sql, Func<IQueryable<TEntityType>, IQueryable<TEntityType>> config = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes raw SQL and returns the number of rows affected.
        /// </summary>
        /// <param name="sql">SQL Query</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Results</returns>
        Task<int> ExecuteRawSqlInterpolatedAsync(FormattableString sql, CancellationToken cancellationToken = default);


        /// <summary>
        /// Executes raw SQL and returns the number of rows affected.
        /// </summary>
        /// <param name="sql">SQL Query</param>
        /// <param name="parameters">Parameters</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Results</returns>
        Task<int> ExecuteRawSqlAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default);
    }
}
