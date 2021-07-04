// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.Base.Aspects;
using LayeredArchitecture.Base.Identity;
using LayeredArchitecture.DataAccess.Base;
using LayeredArchitecture.DataAccess.Dao;
using LayeredArchitecture.DataAccess.Sql;
using LayeredArchitecture.Example.Domain;

namespace LayeredArchitecture.Example.DataAccess
{
    [LogAspect]
    public class AddressDao : DaoBase<Address>, IAddressDao
    {
        public AddressDao(IDbConnectionScopeResolver dbConnectionScopeResolver, IUserAccessor userAccessor, SqlClient sqlClient) 
            : base(dbConnectionScopeResolver, userAccessor, sqlClient) { }
    }
}
