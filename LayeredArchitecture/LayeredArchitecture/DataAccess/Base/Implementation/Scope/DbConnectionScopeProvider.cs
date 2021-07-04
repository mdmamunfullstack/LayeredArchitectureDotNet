// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace LayeredArchitecture.DataAccess.Base
{
    /// <summary>
    /// Resolves the current <see cref="DbConnectionScope"/> on the <see cref="AsyncLocalStorage"/> for the given call. 
    /// </summary>
    public class DbConnectionScopeProvider : IDbConnectionScopeResolver
    {
        public DbConnectionScope Resolve()
        {
            var immutableStack = AsyncLocalStorage.GetStack();

            if(immutableStack.IsEmpty)
            {
                throw new Exception("There is no active DbConnectionScope");
            }

            return immutableStack.Peek();
        }
    }
}
