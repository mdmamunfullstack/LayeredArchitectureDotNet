// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Security.Claims;

namespace LayeredArchitecture.Base.Identity
{
    /// <summary>
    /// A Mock for Testing Purposes.
    /// </summary>
    public class MockUserAccessor : IUserAccessor
    {
        public ClaimsPrincipal User => BuildMockClientPrincipal();

        private ClaimsPrincipal BuildMockClientPrincipal()
        {
            var mockClaimIdentity = new ClaimsIdentity();

            mockClaimIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "MOCK_USER"));

            return new ClaimsPrincipal(mockClaimIdentity);
        }
    }
}
