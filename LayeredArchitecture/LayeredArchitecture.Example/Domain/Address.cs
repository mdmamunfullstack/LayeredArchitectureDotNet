// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.Base.Auditing;
using LayeredArchitecture.DataAccess.Base;
using LayeredArchitecture.DataAccess.Base.Model;

namespace LayeredArchitecture.Example.Domain
{
    /// <summary>
    /// Address.
    /// </summary>
    public class Address : IAuditedEntity, IStatefulEntity
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name1.
        /// </summary>
        public string Name1 { get; set; }

        /// <summary>
        /// Name2.
        /// </summary>
        public string Name2 { get; set; }

        /// <summary>
        /// Street with HouseNo.
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Zip Code.
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// City.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Country.
        /// </summary>
        public string Country { get; set; }

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
