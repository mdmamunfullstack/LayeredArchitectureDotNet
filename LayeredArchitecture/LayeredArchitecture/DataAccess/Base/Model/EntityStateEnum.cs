// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace LayeredArchitecture.DataAccess.Base.Model
{
    /// <summary>
    /// A State to allow a Save Operation.
    /// </summary>
    public enum EntityStateEnum
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Added.
        /// </summary>
        Add = 1,

        /// <summary>
        /// Updated.
        /// </summary>
        Update = 2,

        /// <summary>
        /// Removed.
        /// </summary>
        Remove = 3
    }
}
