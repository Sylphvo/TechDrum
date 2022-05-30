using System.Reflection;
using Invedia.Core.ConfigUtils;
using Invedia.Core.Constants;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TechDrum.Contract.Repository.Models.User;

namespace TechDrum.Repository.Data
{
    public sealed partial class AppDbContext : IdentityDbContext<UserEntity>
    {
        public static readonly ILoggerFactory LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(
            builder =>
            {
                builder
                    .AddFilter((category, level) =>
                        category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                    .AddConsole();
            });

        public readonly int CommandTimeoutInSecond = 20 * 60;

        public AppDbContext()
        {
            Database.SetCommandTimeout(CommandTimeoutInSecond);
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.SetCommandTimeout(CommandTimeoutInSecond);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config =
                    new ConfigurationBuilder()
                        .AddJsonFile(ConfigurationFileName.ConnectionConfig, false, true)
                        .Build();

                var connectionString =
                    config.GetValueByEnv<string>(ConfigurationSectionName.ConnectionStrings);


                optionsBuilder.UseSqlServer(connectionString, sqlServerOptionsAction =>
                {
                    sqlServerOptionsAction.MigrationsAssembly(
                        typeof(AppDbContext).GetTypeInfo().Assembly.GetName().Name);

                    sqlServerOptionsAction.MigrationsHistoryTable("Migration");
                });
            }

            optionsBuilder.UseLoggerFactory(LoggerFactory);
        }
    }
}
