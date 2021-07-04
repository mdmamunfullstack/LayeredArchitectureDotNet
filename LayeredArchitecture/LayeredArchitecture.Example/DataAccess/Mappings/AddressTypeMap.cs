// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.DataAccess.Mapping;
using LayeredArchitecture.Example.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LayeredArchitecture.Example.DataAccess.Mappings
{
    public class AddressTypeMap : EntityTypeMap<Address>
    {
        protected override void InternalMap(EntityTypeBuilder<Address> builder)
        {
            builder
                .ToTable("Address", "dbo");

            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .HasColumnName("AddressID")
                .ValueGeneratedOnAdd();

            builder
                .Property(x => x.Name1)
                .HasColumnName("Name1");

            builder
                .Property(x => x.Name2)
                .HasColumnName("Name2");

            builder
                .Property(x => x.Street)
                .HasColumnName("Street");

            builder
                .Property(x => x.ZipCode)
                .HasColumnName("ZipCode");

            builder
                .Property(x => x.City)
                .HasColumnName("City");

            builder
                .Property(x => x.Country)
                .HasColumnName("Country");

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
