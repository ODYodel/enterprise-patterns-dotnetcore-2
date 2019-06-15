using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using Enterprise.DAL.Extensions.Resolvers;
using Enterprise.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Enterprise.DAL
{
    public interface IModelBuilderUtilities
    {
        void SetDefaultNVarChar(int nVarCharSize, ModelBuilder builder);
        void AddCreatedDateAndModifiedDate(IEnumerable<EntityEntry> enumerable, int userId);
    }
    public class ModelBuilderUtilities : IModelBuilderUtilities
    {
        public ModelBuilderUtilities()
        {
        }

        public void AddCreatedDateAndModifiedDate(IEnumerable<EntityEntry> entities, int userId)
        {
            entities.Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
            foreach (var entry in entities)
            {
                // try and convert to an Auditable Entity
                var entity = entry.Entity as BaseDbModel;
                // call PrepareSave on the entity, telling it the state it is in
                SetCreatedAndModified(entity, entry.State, userId);
            }
        }

        public void SetDefaultNVarChar(int nVarCharSize, ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(string)))
            {
                property.AsProperty().Builder
                    .HasMaxLength(nVarCharSize, ConfigurationSource.Convention);
            }
        }
        private void SetCreatedAndModified(BaseDbModel entity, EntityState state, int userId)
        {
            if (entity != null)
            {
                var now = DateTime.UtcNow;

                if (state == EntityState.Added)
                {
                    entity.CreatedById = userId; //?? "unknown";
                    entity.CreatedDate = now;
                }
                entity.ModifiedById = userId;// ?? 0;
                entity.ModifiedDate = now;
            }
        }
    }
}
