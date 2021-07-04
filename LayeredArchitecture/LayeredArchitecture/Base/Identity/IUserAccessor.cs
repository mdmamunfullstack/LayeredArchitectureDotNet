// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Security.Claims;

namespace LayeredArchitecture.Base.Identity
{
    /// <summary>
    /// Accesses the <see cref="ClaimsPrincipal"/> for the current context, for example through 
    /// a <see cref="IHttpContextAccessor"/> or other implementations.
    /// </summary>
    public interface IUserAccessor
    {
        /// <summary>
        /// Access the <see cref="ClaimsPrincipal"/> for this context.
        /// </summary>
        ClaimsPrincipal User { get; }
    }
}
