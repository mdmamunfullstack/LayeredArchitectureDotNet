// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.Base.Auditing;
using LayeredArchitecture.DataAccess.Base;
using LayeredArchitecture.DataAccess.Base.Model;
using System;

namespace LayeredArchitecture.Example.Domain
{
    /// <summary>
    /// Person.
    /// </summary>
    public class Person : IAuditedEntity, IStatefulEntity
    {
        /// <summary>
        /// The Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// First Name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Birth Date.
        /// </summary>
        public DateTime BirthDate { get; set; }

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
