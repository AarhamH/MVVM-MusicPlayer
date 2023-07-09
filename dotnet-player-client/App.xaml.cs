using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using dotnet_player_client.Extensions;
using dotnet_player_client.Services;
using dotnet_player_client.ViewModels;
using dotnet_player_data.Data;
using dotnet_player_data.DataEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace dotnet_player_client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider? _serviceProvider;
        protected override void OnStartup(StartupEventArgs e)
        {
            IServiceCollection services = new ServiceCollection();

            _serviceProvider = services.AddViewModels()
                                       .AddNavigation()
                                       .AddDbContextFactory()
                                       .AddStores()
                                       .AddServices()
                                       .BuildServiceProvider();

            IDbContextFactory<DataContext> dbFactory = _serviceProvider.GetRequiredService<IDbContextFactory<DataContext>>();

            Directory.CreateDirectory("data"); // Create data directory for db files or temp files.
            Directory.CreateDirectory("downloads"); // Create data directory for db files or temp files.

            Directory.CreateDirectory("banners");
            Directory.CreateDirectory("songs");

            using (var dbContext = dbFactory.CreateDbContext())
            {
                dbContext.Database.Migrate();
            }

            MainWindow = new MainWindow()
            {
                DataContext = _serviceProvider.GetRequiredService<MainViewModel>()
            };
            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
