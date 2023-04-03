using dotnet_player_client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Stores
{
    public class PlaylistBrowserNavigationStore
    {
        public event EventHandler<PlaylistBrowserChangedEventArgs>? PlaylistBrowserChanged;
        private int _browserPlaylistId;
        public int BrowserPlaylistId
        {
            get => _browserPlaylistId;
            set
            {
                _browserPlaylistId = value;
                PlaylistBrowserChanged?.Invoke(this, new PlaylistBrowserChangedEventArgs(_browserPlaylistId));
            }
        }
    }
}
