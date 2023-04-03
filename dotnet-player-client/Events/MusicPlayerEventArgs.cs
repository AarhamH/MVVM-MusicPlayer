using dotnet_player_client.Enums;
using dotnet_player_data.DataEntities;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Events
{
    public class MusicPlayerEventArgs : EventArgs
    {
        public PlayerEventType Type { get; }
        public MediaEntity? Media { get; }
        public IWaveProvider? Audio { get; }

        public MusicPlayerEventArgs(PlayerEventType type, MediaEntity? media, IWaveProvider? audio)
        {
            Type = type;
            Media = media;
            Audio = audio;
        }
    }
}
