// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.DataAccess.Tests;
using LayeredArchitecture.Tests.DataAccess;
using LayeredArchitecture.Tests.Domain;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace LayeredArchitecture.Tests
{
    [TestFixture]
    public class DaoTests : TransactionalTestBase
    {
        private IPersonDao personDao;

        // Initialize Members by using the Service Locator...
        protected override void OnSetupBeforeTransaction()
        {
            personDao = services.GetRequiredService<IPersonDao>();
        }

        [Test]
        public async Task TestInsertAsync()
        {
            var p = new Person
            {
                FirstName = "Philipp",
                LastName = "Wagner",
                BirthDate = new DateTime(2020, 1, 3)
            };

            await personDao.InsertAsync(p);

            Assert.NotNull(p.Id);

            var persons = await personDao.QueryAsync();

            Assert.IsNotNull(persons);
            Assert.AreEqual(1, persons.Count);
        }

        protected override void RegisterDependencies(ServiceCollection services)
        {
            Bootstrapper.RegisterDependencies(services);
        }
    }
}