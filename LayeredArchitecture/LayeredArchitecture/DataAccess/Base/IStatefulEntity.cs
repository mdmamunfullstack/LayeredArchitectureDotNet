// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.DataAccess.Base.Model;

namespace LayeredArchitecture.DataAccess.Base
{
    public interface IStatefulEntity
    {
        EntityStateEnum EntityState { get; set; }
    }
}
