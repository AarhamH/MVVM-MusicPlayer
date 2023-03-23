using dotnet_player_client.Enumeration;
using dotnet_player_data.Objects;
using NAudio.Wave;
using System;

namespace dotnet_player_client.Arguments
{
    public class SongArgs : EventArgs
    {
        public PlayerFuncType FuncType { get; }
        public SongObjects? SongObj { get; }

        public IWaveProvider? AudioVal { get; }

        public SongArgs(PlayerFuncType funcType, SongObjects? songObj, IWaveProvider? audioVal)
        {
            FuncType = funcType;
            SongObj = songObj;
            AudioVal = audioVal;
        }
    }
}
