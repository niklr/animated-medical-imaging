using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace AMI.Persistence.EntityFramework.Shared.Extensions
{
    /// <summary>
    /// Extensions related to ModelBuilder.
    /// </summary>
#pragma warning disable SA1008 // Opening parenthesis must be spaced correctly
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Applies all configurations.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void ApplyAllConfigurations(this ModelBuilder modelBuilder)
        {
            ApplyAllConfigurations(modelBuilder, typeof(ModelBuilderExtensions).Assembly);
        }

        /// <summary>
        /// Applies all configurations contained in the given assembly.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <param name="assembly">The assembly containing the configurations.</param>
        public static void ApplyAllConfigurations(this ModelBuilder modelBuilder, Assembly assembly)
        {
            var applyConfigurationMethodInfo = modelBuilder
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .First(m => m.Name.Equals("ApplyConfiguration", StringComparison.OrdinalIgnoreCase));

            var ret = assembly
                .GetTypes()
                .Select(t => (t, i: t.GetInterfaces().FirstOrDefault(i => i.Name.Equals(typeof(IEntityTypeConfiguration<>).Name, StringComparison.Ordinal))))
                .Where(it => it.i != null)
                .Select(it => (et: it.i.GetGenericArguments()[0], cfgObj: Activator.CreateInstance(it.t)))
                .Select(it => applyConfigurationMethodInfo.MakeGenericMethod(it.et).Invoke(modelBuilder, new[] { it.cfgObj }))
                .ToList();
        }
    }
#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
}
