using dotnet_player_client.Core;
using dotnet_player_client.Services;
using dotnet_player_client.ViewModels;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Commands
{
    public class TogglePlaylistRemoveCommand : CommandBase
    {
        private readonly ToolbarViewModel _toolbar;
        public TogglePlaylistRemoveCommand(ToolbarViewModel toolbar)
        {
            _toolbar = toolbar;
        }

        public override void Execute(object? parameter)
        {

        }
    }
}
