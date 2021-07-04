// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.Tests.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LayeredArchitecture.Tests.Business
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
        /// Gets all <see cref="Person"/>.
        /// </summary>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>All <see cref="Person"/> found</returns>
        Task<List<Person>> GetAllPersonsAsync(CancellationToken cancellationToken = default);
    }
}
