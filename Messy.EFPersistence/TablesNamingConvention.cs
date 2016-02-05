using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.Infrastructure.Pluralization;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Messy.EFPersistence
{
    public class TablesNamingConvention : Convention
    {
        private readonly IPluralizationService _pluralizationService =
            DbConfiguration.DependencyResolver.GetService<IPluralizationService>();

        public TablesNamingConvention()
        {
            Types()
                .Where(t => t.Name.EndsWith("Table"))
                .Configure(t => t.ToTable(RemovePostfixAndSingularize(t.ClrType.Name)));
        }

        private string RemovePostfixAndSingularize(string text)
        {
            return _pluralizationService.Singularize(text.Remove(text.LastIndexOf("Table", StringComparison.Ordinal)));
        }
    }
}