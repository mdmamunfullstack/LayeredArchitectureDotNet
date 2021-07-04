// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.DataAccess.Mapping;
using LayeredArchitecture.Example.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LayeredArchitecture.Example.DataAccess.Mappings
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

            builder
                .Property(x => x.RowVersion)
                .HasColumnName("RowVersion")
                .IsRowVersion();

            builder
                .Property(x => x.AuditUser)
                .HasColumnName("AuditUser")
                .IsRequired();

            builder
                .Property(x => x.AuditOperation)
                .HasColumnName("AuditOperation")
                .HasConversion<int>()
                .IsRequired();

            builder
                .Property(x => x.EntityVersion)
                .HasColumnName("EntityVersion")
                .HasDefaultValue(0);

            builder
                .Ignore(x => x.EntityState);
        }
    }
}
