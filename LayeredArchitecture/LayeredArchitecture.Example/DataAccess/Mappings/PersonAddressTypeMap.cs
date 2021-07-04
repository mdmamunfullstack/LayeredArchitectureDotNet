// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.DataAccess.Mapping;
using LayeredArchitecture.Example.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LayeredArchitecture.Example.DataAccess.Mappings
{
    public class PersonAddressTypeMap : EntityTypeMap<PersonAddress>
    {
        protected override void InternalMap(EntityTypeBuilder<PersonAddress> builder)
        {
            builder
                .ToTable("PersonAddress", "dbo");

            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .HasColumnName("PersonAddressID")
                .ValueGeneratedOnAdd();

            builder
                .Property(x => x.AddressId)
                .HasColumnName("AddressID");

            builder
                .Property(x => x.PersonId)
                .HasColumnName("PersonID");

            builder
                .Property(x => x.ValidFrom)
                .HasColumnName("ValidFrom");

            builder
                .Property(x => x.ValidUntil)
                .HasColumnName("ValidUntil");

            builder
                .HasOne(x => x.Address)
                .WithMany()
                .HasForeignKey(x => x.AddressId);

            builder
                .HasOne(x => x.Person)
                .WithMany()
                .HasForeignKey(x => x.PersonId);

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
                .Property(x => x.RowVersion)
                .HasColumnName("RowVersion")
                .IsRowVersion();

            builder
                .Ignore(x => x.EntityState);

        }
    }
}
