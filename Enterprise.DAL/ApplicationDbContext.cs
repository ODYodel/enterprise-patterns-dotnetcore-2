using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Enterprise.DAL.Extensions.Enums;
using Enterprise.DAL.Extensions.Resolvers;
using Enterprise.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Enterprise.DAL
{
    //Identity provides the DbSet for user authentication tables via IdentityDbContext
    public class ApplicationDbContext
        : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
    {
        private IModelBuilderUtilities _modelBuilderUtilities;
        private IUserResolverService _userResolver;
        public DbSet<Widgets> Widgets { get; set; }
        public DbSet<WidgetProviders> WidgetProviders { get; set; }
        public DbSet<WidgetResources> WidgetResources { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                    IModelBuilderUtilities modelBuilderUtilities,
                                    IUserResolverService userResolverService)
            : base(options)
        {
            _modelBuilderUtilities = modelBuilderUtilities;
            _userResolver = userResolverService;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Override base nvarchar of max to 256
            _modelBuilderUtilities.SetDefaultNVarChar(256, modelBuilder);

            //Grab all table configurations from the tbaleconfigurations folder
            Assembly tableConfigurationAssembly = GetType().Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(tableConfigurationAssembly);

            modelBuilder.CreateEnumLookupTable(createForeignKeys: true);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _modelBuilderUtilities.AddCreatedDateAndModifiedDate(ChangeTracker.Entries(), _userResolver.GetUserId());

            return await base.SaveChangesAsync(true, cancellationToken);
        }
        public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
        {
            private readonly IHttpContextAccessor _context;
            public ApplicationDbContext CreateDbContext(string[] args)
            {
                var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
                var modelBuilderUtilities = new ModelBuilderUtilities();
                var userResolverService = new UserResolverService(_context);
                builder.UseSqlServer(@"Server = (localdb)\mssqllocaldb; Database = Enterprisei2; Trusted_Connection = True; ConnectRetryCount = 0",
                    optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(ApplicationDbContext).GetTypeInfo().Assembly.GetName().Name));

                return new ApplicationDbContext(builder.Options, modelBuilderUtilities, userResolverService);
            }
        }

    }
}
