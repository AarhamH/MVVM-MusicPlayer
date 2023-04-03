using dotnet_player_client.Services;
using dotnet_player_client.Stores;
using dotnet_player_client.ViewModels;
using dotnet_player_data.Context;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace dotnet_player_client.Utilities
{
    public static class ServiceContainerUtil
    {
        public static IServiceCollection AddViewModels(this IServiceCollection collection)
        {
            collection.AddTransient<HomeVM>();
            collection.AddTransient<PlayListVM>();
            collection.AddTransient<DownloadVM>();
            collection.AddSingleton<PlayerVM>();
            collection.AddSingleton<ToolbarVM>();
            collection.AddSingleton<MainVM>();
            return collection;
        }

        public static IServiceCollection AddStores(this IServiceCollection collection)
        {
            collection.AddSingleton<SongStorage>();
            collection.AddSingleton<PLStorage>();
            collection.AddSingleton<BrowserNavStorage>();
            return collection;
        }

        public static IServiceCollection AddNavigation(this IServiceCollection collection)
        {
            collection.AddTransient<INavigationService>(s =>
                new NavigationService(
                    () => s.GetRequiredService<MainVM>(),
                    () => s.GetRequiredService<HomeVM>(),
                    () => s.GetRequiredService<PlayListVM>(),
                    () => s.GetRequiredService<DownloadVM>()
            ));

            return collection;
        }

        public static IServiceCollection AddServices(this IServiceCollection collection)
        {
            collection.AddSingleton<IPlayerService, PlayerService>();
            collection.AddSingleton<IYouTubeClientService, YouTubeClientService>();
            return collection;
        }

        public static IServiceCollection AddDbContextFactory(this IServiceCollection collection)
        {
            collection.AddDbContextFactory<DataContext>();
            return collection;
        }

    }
}
