using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotnet_player_client.Arguments;

namespace dotnet_player_client.Stores
{
    public class BrowserNavStorage
    {
        public event EventHandler<PLBrowseArgs>? PLBrowse;
        public int _browserPlaylistID;

        public int BrowserPlaylistID
        {
            get => _browserPlaylistID;
            set
            {
                _browserPlaylistID = value;
                PLBrowse?.Invoke(this, new PLBrowseArgs(_browserPlaylistID));
            }
        }
    }
}
