using dotnet_music_player_app.Resources.ViewModel;
using Microsoft.EntityFrameworkCore;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace dotnet_music_player_app.Resources.View
{
    /// <summary>
    /// Interaction logic for SongPage.xaml
    /// </summary>
    public partial class SongPage : UserControl
    {
        public SongPage()
        {
            InitializeComponent();
        }

        public void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            bool? response = openFileDialog.ShowDialog();

            string fileName;

            SongItem songItem = new SongItem();
            if (response == true)
            {
                fileName = openFileDialog.SafeFileName;

                songItem.Title = fileName;
                songItem.Time = 2;
                MessageBox.Show(fileName);
            }

            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<SongItem>();
                connection.Insert(songItem);
            }



        }

    }
}
