// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.DataAccess.Dao;
using LayeredArchitecture.Example.Domain;

namespace LayeredArchitecture.Example.DataAccess
{
    public interface IPersonAddressDao 
        : IDao<PersonAddress>
    {
    }
}
