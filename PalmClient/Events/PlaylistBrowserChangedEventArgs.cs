using PalmData.DataEntities;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalmClient.Events
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
