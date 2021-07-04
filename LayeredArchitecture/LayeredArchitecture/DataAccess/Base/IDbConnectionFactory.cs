// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Data.Common;

namespace LayeredArchitecture.DataAccess.Base
{
    /// <summary>
    /// Factory for creating a new <see cref="DbConnection"/>.
    /// </summary>
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// Creates a new <see cref="DbConnection"/>.
        /// </summary>
        /// <returns></returns>
        DbConnection Create();
    }
}
