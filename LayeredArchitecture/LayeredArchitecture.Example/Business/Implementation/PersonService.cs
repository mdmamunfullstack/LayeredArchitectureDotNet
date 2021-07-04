// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.DataAccess.Base;
using LayeredArchitecture.Example.DataAccess;
using LayeredArchitecture.Example.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using LayeredArchitecture.Base.Aspects;

namespace LayeredArchitecture.Example.Business
{
    /// <summary>
    /// Service for managing Person entities in the System.
    /// </summary>
    [LogAspect]
    public class PersonService : IPersonService
    {
        private readonly DbConnectionScopeFactory dbConnectionScopeFactory;
        private readonly IPersonDao personDao;
        private readonly IAddressDao addressDao;
        private readonly IPersonAddressDao personAddressDao;

        public PersonService(IServiceProvider services)
        {
            this.dbConnectionScopeFactory = services.GetService<DbConnectionScopeFactory>();

            // Poor Man Property Injection here:
            this.personDao = services.GetRequiredService<IPersonDao>();
            this.addressDao = services.GetRequiredService<IAddressDao>();
            this.personAddressDao = services.GetRequiredService<IPersonAddressDao>();
        }

        public async Task AddOrUpdateAddressAsync(Address address, CancellationToken cancellationToken = default)
        {
            using (var scope = dbConnectionScopeFactory.Create())
            {
                if(address.Id == 0)
                {
                    await addressDao.InsertAsync(address, cancellationToken);
                } 
                else
                {
                    await addressDao.UpdateAsync(address, cancellationToken);
                }
                
                scope.Commit();
            }
        }

        public async Task DeletePersonAsync(Person person, CancellationToken cancellationToken = default)
        {
            using (var scope = dbConnectionScopeFactory.Create())
            {
                await personDao.DeleteAsync(person, cancellationToken);

                scope.Commit();
            }
        }

        public async Task AddOrUpdatePersonAsync(Person person, CancellationToken cancellationToken = default)
        {
            using(var scope = dbConnectionScopeFactory.Create())
            {
                if(person.Id == 0)
                {
                    await personDao.InsertAsync(person, cancellationToken);
                } 
                else
                {
                    await personDao.UpdateAsync(person, cancellationToken);
                }
                
                scope.Commit();
            }
        }

        public async Task AssignAddressAsync(int personId, int addressId, DateTime validFrom, CancellationToken cancellationToken = default)
        {
            var personAddress = new PersonAddress
            {
                AddressId = addressId,
                PersonId = personId,
            };

            using (var scope = dbConnectionScopeFactory.Create())
            {
                await personAddressDao.InsertAsync(personAddress, cancellationToken);

                scope.Commit();
            }
        }

        public async Task<List<PersonAddress>> GetPersonAddressAllAsync(CancellationToken cancellationToken = default)
        {
            using (var scope = dbConnectionScopeFactory.Create())
            {
                return await personAddressDao
                    .QueryAsync(x => x
                        .Include(pa => pa.Person)
                        .Include(pa => pa.Address));
            }
        }

        public Task<List<Person>> GetAll(CancellationToken cancellationToken = default)
        {
            using (var scope = dbConnectionScopeFactory.Create())
            {
                return personDao.QueryAsync(cancellationToken: cancellationToken);
            }
        }
    }
}
