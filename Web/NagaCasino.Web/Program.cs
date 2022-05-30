using Autofac.Extensions.DependencyInjection;
using Invedia.Core.ConfigUtils;
using Invedia.Core.EnvUtils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using TechDrum.Contract.Service;
using TechDrum.Core.Configs;
using TechDrum.Core.Utils;
using Serilog;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace TechDrum.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            OnAppStart(host);
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var webHost = Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory());
            webHost = webHost.ConfigureAppConfiguration((context, cnf) =>
            {
                var root = cnf.Build();
                var useVault = root.GetValue("UseVault", false);
                if (useVault)
                {
                    cnf.AddAzureKeyVault(
                        $"https://{root["KeyVault:Vault"]}.vault.azure.net/",
                        root["KeyVault:ClientId"],
                        root["KeyVault:ClientSecret"]);
                }
                SystemSettingModel.Configs = root;
                SystemSettingModel.Instance = root.GetSection<SystemSettingModel>("SystemSetting");
                FundistSettingModel.Instance = root.GetSection<FundistSettingModel>("FundistSetting");
                ClientSettingModel.Instance = root.GetSection<ClientSettingModel>("ClientSetting");
            });
            webHost = webHost
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder
                       .UseStartup<Startup>()
                       .UseSerilog((hostingContext, loggerConfiguration) =>
                       {
                           loggerConfiguration
                               .ReadFrom.Configuration(hostingContext.Configuration)
                               .Enrich.FromLogContext();
                       });
               });

            return webHost;
        }
        public static void ConsoleConfig()
        {
            var welcome =
                $@"Welcome {EnvHelper.MachineName}, {PlatformServices.Default.Application.ApplicationName} v{PlatformServices.Default.Application.ApplicationVersion} - {EnvHelper.CurrentEnvironment} | {
                    PlatformServices.Default.Application.RuntimeFramework.FullName
                } | {RuntimeInformation.OSDescription}";

            Console.Title = welcome;

            Console.WriteLine(welcome);
        }

        public static void OnAppStart(IHost webHost)
        {
            using var scoped = webHost.Services.CreateScope();
            var bootstrapperService = scoped.ServiceProvider.GetRequiredService<IBootstrapperService>();
            bootstrapperService.InitialAsync().Wait();
        }
    }
}
