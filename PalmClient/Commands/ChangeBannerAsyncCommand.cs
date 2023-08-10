using PalmClient.Stores;
using PalmData.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace PalmClient.Commands
{
    public class ChangeBannerAsyncCommand:AsyncCommandBase
    {
        private readonly PlaylistStore _playlistStore;
        private readonly PlaylistBrowserNavigationStore _playlistBrowserNavigationStore;

        public ChangeBannerAsyncCommand(PlaylistStore playlistStore, PlaylistBrowserNavigationStore playlistBrowserNavigationStore)
        {
            _playlistStore = playlistStore;
            _playlistBrowserNavigationStore = playlistBrowserNavigationStore;
        }

        protected override async Task ExecuteAsync(object? parameter)
        {
            var openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                string fileName = openFileDialog.FileName;
                string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\banners" + "\\" + Path.GetFileName(fileName);

                if (!File.Exists(path))
                {
                    File.Copy(fileName, path);
                }

                await _playlistStore.ChangeBanner(_playlistBrowserNavigationStore.BrowserPlaylistId, path);
            }
        }
    }
}
