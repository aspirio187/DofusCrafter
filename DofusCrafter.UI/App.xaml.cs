using DofusCrafter.Data;
using DofusCrafter.UI.Managers;
using DofusCrafter.UI.Services;
using DofusCrafter.UI.ViewModels;
using DofusCrafter.UI.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace DofusCrafter.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; } = default!;

        public App()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr-FR");
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddHttpClient("dofusdb", httpclient =>
            {
                httpclient.BaseAddress = new Uri("https://api.dofusdb.fr");
            });

            services.AddScoped<DofusCrafterDbContext>();

            services.AddSingleton<NavigationManager>();

            services.AddScoped<DofusDBService>();
            //services.AddScoped<ISalesService, SalesService>();

            services.AddTransient<ShellViewModel>();
            services.AddTransient<HomeViewModel>();
            services.AddTransient<SalesViewModel>();
            services.AddTransient<RegisterSaleViewModel>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            ShellView shellView = new ShellView();
            shellView.Show();
        }
    }
}
