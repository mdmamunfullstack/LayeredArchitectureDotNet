// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace LayeredArchitecture.Tests.Domain
{
    /// <summary>
    /// Person.
    /// </summary>
    public class Person
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
    }
}
