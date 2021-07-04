// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Claims;

namespace LayeredArchitecture.Base.Exensions
{
    /// <summary>
    /// Extensions to simplify working with a <see cref="ClaimsPrincipal"/> identity.
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Gets the UserId from a <see cref="ClaimsPrincipal"/>, which is given by the 
        /// <see cref="ClaimTypes.NameIdentifier"/> claim.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns>Resolved User ID</returns>
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);

            return claim != null ? claim.Value : null;
        }
    }
}
