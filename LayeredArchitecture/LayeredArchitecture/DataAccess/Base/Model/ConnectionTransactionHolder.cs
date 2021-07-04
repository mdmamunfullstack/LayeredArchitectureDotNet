// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Data.Common;

namespace LayeredArchitecture.DataAccess.Base.Model
{
    /// <summary>
    /// Holds the current <see cref="DbConnection"/> and <see cref="DbTransaction"/> for a <see cref="DbConnectionScope"/>.
    /// </summary>
    public class ConnectionTransactionHolder
    {
        /// <summary>
        /// The current <see cref="DbConnection"/>.
        /// </summary>
        public readonly DbConnection Connection;

        /// <summary>
        /// The current <see cref="DbTransaction"/>, null if no Transaction has been started.
        /// </summary>
        public readonly DbTransaction Transaction;

        public ConnectionTransactionHolder(DbConnection connection, DbTransaction transaction)
        {
            Connection = connection;
            Transaction = transaction;
        }
    }
}
