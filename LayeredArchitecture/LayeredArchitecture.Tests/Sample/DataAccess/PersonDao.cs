// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.Base.Identity;
using LayeredArchitecture.DataAccess.Base;
using LayeredArchitecture.DataAccess.Dao;
using LayeredArchitecture.DataAccess.Sql;
using LayeredArchitecture.Tests.Domain;

namespace LayeredArchitecture.Tests.DataAccess
{

    public class PersonDao : DaoBase<Person>, IPersonDao
    {
        public PersonDao(IDbConnectionScopeResolver dbConnectionScopeResolver, IUserAccessor userAccessor, SqlClient sqlClient) 
            : base(dbConnectionScopeResolver, userAccessor, sqlClient)
        {
        }
    }
}
