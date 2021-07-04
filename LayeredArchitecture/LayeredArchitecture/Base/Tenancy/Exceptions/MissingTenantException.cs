// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace LayeredArchitecture.Base.Tenancy.Exceptions
{
    [Serializable]
    public class MissingTenantException : Exception
    {
        public MissingTenantException()
        {
        }

        public MissingTenantException(string message) : base(message)
        {
        }

        public MissingTenantException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MissingTenantException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
