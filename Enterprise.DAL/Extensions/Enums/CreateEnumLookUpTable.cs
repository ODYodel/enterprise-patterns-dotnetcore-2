using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Enterprise.DAL.Extensions.Enums
{
    public static class CreateEnumLookUpTable
    {
        public static void CreateEnumLookupTable(this ModelBuilder modelBuilder, bool createForeignKeys = false)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetProperties()).ToArray())
            {
                var entityType = property.DeclaringEntityType;
                var propertyType = property.ClrType;

                if (!propertyType.IsEnum)
                    continue;
                //Debugger.Launch();
                var concreteType = typeof(Lookup<>).MakeGenericType(propertyType);
                var enumLookupBuilder = modelBuilder.Entity(concreteType);
                enumLookupBuilder
                    .ToTable(property.Name)
                    .HasAlternateKey(nameof(Lookup<Enum>.Value));

                var data = Enum.GetValues(propertyType).Cast<object>()
                    .Select(v => Activator.CreateInstance(concreteType, new object[] { v }))
                    .ToArray();
                //Debugger.Launch();
                enumLookupBuilder.HasData(data);

                if (createForeignKeys)
                {
                    modelBuilder.Entity(entityType.Name)
                        .HasOne(concreteType)
                        .WithMany()
                        .HasPrincipalKey(nameof(Lookup<Enum>.Value))
                        .HasForeignKey(property.Name);
                }
            }
        }
    }
}
