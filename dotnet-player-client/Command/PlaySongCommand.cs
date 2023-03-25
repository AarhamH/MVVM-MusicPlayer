﻿using dotnet_player_client.Services;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Command
{
    public class PlaySongCommand : BaseCommand
    {
        private readonly IPlayerService _musicService;
        public PlaySongCommand(IPlayerService musicService)
        {
            _musicService = musicService;
        }

        public override void Execute(object? parameter)
        {
            if (parameter is int SongId)
            {
                if (_musicService.CurrentSong?.Id == SongId)
                {
                    if (_musicService.PlayerState != PlaybackState.Stopped)
                    {
                        _musicService.PlayPause();
                    }
                    else _musicService.RePlay();
                }
                else
                {
                    _musicService.Play(SongId);
                }
            }
        }
    }
}
