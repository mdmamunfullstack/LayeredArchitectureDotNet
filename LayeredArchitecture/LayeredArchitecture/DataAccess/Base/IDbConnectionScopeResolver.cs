// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace LayeredArchitecture.DataAccess.Base
{
    /// <summary>
    /// Used to resolve the current <see cref="DbConnectionScope"/> on the Stack.
    /// </summary>
    public interface IDbConnectionScopeResolver
    {
        /// <summary>
        /// Resolves the <see cref="DbConnectionScope"/>.
        /// </summary>
        /// <returns>Current <see cref="DbConnectionScope"/></returns>
        DbConnectionScope Resolve();
    }
}
