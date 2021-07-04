// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.DataAccess.Base;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LayeredArchitecture.DataAccess.Tests
{
    public abstract class TransactionalTestBase
    {
        protected ServiceProvider services;
        protected DbConnectionScope dbConnectionScope;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var serviceCollection = new ServiceCollection();

            RegisterDependencies(serviceCollection);

            services = serviceCollection.BuildServiceProvider();
        }

        [SetUp]
        public void Setup()
        {
            OnSetupBeforeTransaction();
            
            this.dbConnectionScope = services
                .GetRequiredService<DbConnectionScopeFactory>()
                .Create();

            OnSetupInTransaction();
        }

        protected virtual void OnSetupBeforeTransaction() { }

        protected virtual void OnSetupInTransaction() { }

        [TearDown]
        public void Teardown()
        {
            OnTeardownInTransaction();

            dbConnectionScope.Rollback();
            
            OnTeardownAfterTransaction();
        }

        protected virtual void OnTeardownInTransaction() { }

        protected virtual void OnTeardownAfterTransaction() { }

        protected abstract void RegisterDependencies(ServiceCollection services);
    }
}
