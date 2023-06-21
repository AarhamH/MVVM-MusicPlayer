using dotnet_player_client.Extensions;
using dotnet_player_client.Models;
using dotnet_player_client.Services;
using dotnet_player_client.Stores;
using dotnet_player_data.DataEntities;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace dotnet_player_client.Commands
{
    public class AddSongAsyncCommand : CommandBase
    {
        private readonly IMusicPlayerService _musicService;
        private readonly MediaStore _mediaStore;
        private readonly ObservableCollection<MediaModel>? _observableSongs;

        public AddSongAsyncCommand()
        {

        }

        public override void Execute(object? parameter)
        {
            Process.Start("explorer.exe", @"C:\Users");
        }

    }
}
