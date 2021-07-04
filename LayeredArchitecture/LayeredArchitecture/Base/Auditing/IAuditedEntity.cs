// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace LayeredArchitecture.Base.Auditing
{
    /// <summary>
    /// A class implementing this interface supplies a User ID and may supply a Entity Version or 
    /// receive it from a database generated value (e.g. AFTER INSERT / UPDATE Triggers).
    /// </summary>
    public interface IAuditedEntity
    {
        /// <summary>
        /// A UserId.
        /// </summary>
        string AuditUser { get; set; }

        /// <summary>
        /// Audit Operation (Insert, Update, Delete)
        /// </summary>
        DataChangeOperationEnum AuditOperation { get; set; }

        /// <summary>
        /// Version.
        /// </summary>
        long EntityVersion { get; set; }
    }
}
