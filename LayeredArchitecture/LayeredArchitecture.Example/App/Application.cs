// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.Base.Aspects;
using LayeredArchitecture.DataAccess.Base;
using LayeredArchitecture.Example.Business;
using LayeredArchitecture.Example.Domain;
using System;
using System.Threading.Tasks;

namespace LayeredArchitecture.Example.App
{
    [LogAspect]
    public class Application
    {
        private readonly IPersonService personService;
        private readonly DbConnectionScopeFactory dbConnectionScopeFactory;

        public Application(DbConnectionScopeFactory dbConnectionScopeFactory, IPersonService personService)
        {
            this.dbConnectionScopeFactory = dbConnectionScopeFactory;
            this.personService = personService;
        }

        public async Task RunAsync()
        {
            // Let's do all this in a single Transaction:
            using (var dbConnectionScope = dbConnectionScopeFactory.Create())
            {
                var person0 = new Person
                {
                    FirstName = "Philipp",
                    LastName = "Wagner",
                    BirthDate = new DateTime(1912, 1, 1)
                };

                var person1 = new Person
                {
                    FirstName = "Max",
                    LastName = "Mustermann",
                    BirthDate = new DateTime(1911, 1, 1)
                };


                var address = new Address
                {
                    Name1 = "My Address"
                };

                // We're running in a ConnectionScope, so all this will be done in a single Transaction:
                await personService.AddOrUpdatePersonAsync(person0);
                await personService.AddOrUpdateAddressAsync(address);
                await personService.AssignAddressAsync(person0.Id, address.Id, DateTime.Now);

                await personService.AddOrUpdatePersonAsync(person1);
                await personService.DeletePersonAsync(person1);

                // For demonstration we could also Rollback here:
                dbConnectionScope.Commit();
            }
        }
    }
}
