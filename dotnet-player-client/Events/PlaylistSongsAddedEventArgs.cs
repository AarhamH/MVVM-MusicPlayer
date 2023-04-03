using dotnet_player_data.DataEntities;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Events
{
    public class PlaylistSongsAddedEventArgs : EventArgs
    {
        public IEnumerable<MediaEntity> Songs { get; }

        public PlaylistSongsAddedEventArgs(IEnumerable<MediaEntity> songs)
        {
            Songs = songs;
        }
    }
}
