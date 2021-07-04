// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace LayeredArchitecture.Base.Tenancy.Exceptions
{
    [Serializable]
    public class MissingTenantConfigurationException : Exception
    {
        public MissingTenantConfigurationException()
        {
        }

        public MissingTenantConfigurationException(string message) : base(message)
        {
        }

        public MissingTenantConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MissingTenantConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
