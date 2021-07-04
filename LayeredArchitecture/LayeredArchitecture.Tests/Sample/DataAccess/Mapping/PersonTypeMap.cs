// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.DataAccess.Mapping;
using LayeredArchitecture.Tests.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LayeredArchitecture.Tests.DataAccess.Mappings
{
    public class PersonTypeMap : EntityTypeMap<Person>
    {
        protected override void InternalMap(EntityTypeBuilder<Person> builder)
        {
            builder
                .ToTable("Person", "dbo");

            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .HasColumnName("PersonID")
                .ValueGeneratedOnAdd();

            builder
                .Property(x => x.FirstName)
                .HasColumnName("FirstName");

            builder
                .Property(x => x.LastName)
                .HasColumnName("LastName");

            builder
                .Property(x => x.BirthDate)
                .HasColumnName("BirthDate");
        }
    }
}
