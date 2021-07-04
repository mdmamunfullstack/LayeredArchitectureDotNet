// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.Example.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LayeredArchitecture.Example.Business
{
    /// <summary>
    /// Service for managing Person entities in the System.
    /// </summary>
    public interface IPersonService
    {
        /// <summary>
        /// Adds or updates a Person.
        /// </summary>
        /// <param name="person">Person</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Task result</returns>
        Task AddOrUpdatePersonAsync(Person person, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a Person.
        /// </summary>
        /// <param name="person">Person</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Task result</returns>
        Task DeletePersonAsync(Person person, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds or updates an Address.
        /// </summary>
        /// <param name="address">Address</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Task result</returns>
        Task AddOrUpdateAddressAsync(Address address, CancellationToken cancellationToken = default);

        /// <summary>
        /// Assigns an Address to a Person and gives it a Start Date for the Assignment.
        /// </summary>
        /// <param name="personId">Person Database ID</param>
        /// <param name="addressId">Address Database ID</param>
        /// <param name="validFrom">Start of the Assignment</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Task result</returns>
        Task AssignAddressAsync(int personId, int addressId, DateTime validFrom, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns all PersonAddress assignments in the database.
        /// </summary>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>All PersonAddress assignments in the database</returns>
        Task<List<PersonAddress>> GetPersonAddressAllAsync(CancellationToken cancellationToken = default);
    }
}
