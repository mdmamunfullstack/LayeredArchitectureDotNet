// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.DataAccess.Base;
using LayeredArchitecture.Tests.DataAccess;
using LayeredArchitecture.Tests.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LayeredArchitecture.Tests.Business
{
    /// <summary>
    /// Service for managing Person entities in the System.
    /// </summary>
    public class PersonService : IPersonService
    {
        private readonly DbConnectionScopeFactory dbConnectionScopeFactory;
        private readonly IPersonDao personDao;

        public PersonService(DbConnectionScopeFactory dbConnectionScopeFactory, IPersonDao personDao)
        {
            this.dbConnectionScopeFactory = dbConnectionScopeFactory;
            this.personDao = personDao;
        }

        public async Task AddOrUpdatePersonAsync(Person person, CancellationToken cancellationToken = default)
        {
            using(var context = dbConnectionScopeFactory.Create())
            {
                if(person.Id == 0)
                {
                    await personDao.UpdateAsync(person, cancellationToken);
                } 
                else
                {
                    await personDao.InsertAsync(person, cancellationToken);
                }
                
            }
        }

        public async Task<List<Person>> GetAllPersonsAsync(CancellationToken cancellationToken = default)
        {
            using (var context = dbConnectionScopeFactory.Create())
            {
                return await personDao.QueryAsync();
            }
        }
    }
}
