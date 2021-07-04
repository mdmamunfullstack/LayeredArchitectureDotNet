﻿// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LayeredArchitecture.DataAccess.Mapping
{
    /// <summary>
    /// A base class for providing simplified access to a <see cref="EntityTypeBuilder{TEntityType}"/> for a 
    /// given <see cref="TEntityType"/>. This is used to enable mappings for each type individually.
    /// </summary>
    /// <typeparam name="TEntityType"></typeparam>
    public abstract class EntityTypeMap<TEntityType> : IEntityTypeMap
            where TEntityType : class
    {
        /// <summary>
        /// Implements the <see cref="IEntityTypeMap"/>.
        /// </summary>
        /// <param name="builder"><see cref="ModelBuilder"/> passed from the <see cref="DbContext"/></param>
        public void Map(ModelBuilder builder)
        {
            InternalMap(builder.Entity<TEntityType>());
        }

        /// <summary>
        /// Implementy the Entity Type configuration for a <see cref="TEntityType"/>.
        /// </summary>
        /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> to configure</param>
        protected abstract void InternalMap(EntityTypeBuilder<TEntityType> builder);
    }
}
