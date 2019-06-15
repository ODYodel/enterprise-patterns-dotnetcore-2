using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Enterprise.Models;
using Enterprise.Models.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enterprise.DAL
{
    public class WidgetsConfiguration : IEntityTypeConfiguration<Widgets>
    {

        public void Configure(EntityTypeBuilder<Widgets> builder)
        {
            builder
                .HasKey(k => k.Id);
            builder.Property(e => e.Id).UseSqlServerIdentityColumn();

            builder.HasOne(provider => provider.WidgetProviders);

        }

    }
}
