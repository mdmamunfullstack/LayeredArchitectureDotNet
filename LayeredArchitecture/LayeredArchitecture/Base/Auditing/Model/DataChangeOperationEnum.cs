// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace LayeredArchitecture.Base.Auditing
{
    /// <summary>
    /// The Operation executed on the Audited Entity.
    /// </summary>
    public enum DataChangeOperationEnum
    {
        /// <summary>
        /// Insert.
        /// </summary>
        Insert = 1,

        /// <summary>
        /// Update.
        /// </summary>
        Update = 2,

        /// <summary>
        /// Delete.
        /// </summary>
        Delete = 3
    }
}
