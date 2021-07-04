// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.Base.Auditing;
using LayeredArchitecture.DataAccess.Base;
using LayeredArchitecture.DataAccess.Base.Model;
using System;

namespace LayeredArchitecture.Example.Domain
{
    /// <summary>
    /// Person to Address Assignment.
    /// </summary>
    public class PersonAddress : IAuditedEntity, IStatefulEntity
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Person ID.
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// Address ID.
        /// </summary>
        public int AddressId { get; set; }

        /// <summary>
        /// Valid From.
        /// </summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>
        /// Valid Until.
        /// </summary>
        public DateTime? ValidUntil { get; set; }

        /// <summary>
        /// Navigation Property for Person.
        /// </summary>
        public Person Person { get; set; }

        /// <summary>
        /// Navigation Property for Address.
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// Audit User.
        /// </summary>
        public string AuditUser { get; set; }

        /// <summary>
        /// Audit Operation.
        /// </summary>
        public DataChangeOperationEnum AuditOperation { get; set; }

        /// <summary>
        /// The Version of the Entity.
        /// </summary>
        public long EntityVersion { get; set; }

        /// <summary>
        /// RowVersion Concurrency Token.
        /// </summary>
        public byte[] RowVersion { get; set; }

        /// <summary>
        /// State.
        /// </summary>
        public EntityStateEnum EntityState { get; set; }
    }
}
