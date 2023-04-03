using dotnet_player_data.DataEntities;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Events
{
    public class PlaylistBrowserChangedEventArgs : EventArgs
    {
        public int PlaylistId { get; set; }

        public PlaylistBrowserChangedEventArgs(int playlistId)
        {
            PlaylistId = playlistId;
        }
    }
}
