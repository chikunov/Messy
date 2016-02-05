using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Reflection;

namespace Messy.EFPersistence
{
    [DbConfigurationType(typeof (BaseDbContextConfiguration))]
    public class BaseDbContext : DbContext
    {
        protected string Name { get; }

        protected BaseDbContext(string contextName) : base($"{contextName}Context")
        {
            Name = contextName;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Name);

            modelBuilder.Configurations.AddFromAssembly(Assembly.GetCallingAssembly());
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Add<TablesNamingConvention>();

            Configuration.LazyLoadingEnabled = false;

            base.OnModelCreating(modelBuilder);
        }
    }
}