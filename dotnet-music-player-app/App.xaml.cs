using System;
using System.Windows;
using dotnet_music_player_app.Resources.Utils;
using dotnet_music_player_app.Resources.ViewModel;

namespace dotnet_music_player_app
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static string databaseName = "SongItem.db";
        static string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string databasePath = System.IO.Path.Combine(folderPath, databaseName);
    }

    
}
