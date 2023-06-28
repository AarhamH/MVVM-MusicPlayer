using Microsoft.EntityFrameworkCore;
using dotnet_player_client.Commands;
using dotnet_player_client.Services;
using dotnet_player_data.Data;
using dotnet_player_data.DataEntities;
using dotnet_player_client.Models;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using dotnet_player_client.Events;
using System.Diagnostics;
using dotnet_player_client.Stores;
using dotnet_player_client.Interfaces;
using System.Windows;
using dotnet_player_client.Extensions;
using dotnet_player_client.Core;
using dotnet_player_client.Enums;
using static System.Net.WebRequestMethods;

namespace dotnet_player_client.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public ICommand OpenLink { get; }
        public HomeViewModel()
        {
            OpenLink = new OpenLinkCommand();
        }

    }
}
