// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.DataAccess.Dao;
using LayeredArchitecture.Tests.Domain;

namespace LayeredArchitecture.Tests.DataAccess
{
    public interface IPersonDao : IDao<Person>
    {

    }
}
