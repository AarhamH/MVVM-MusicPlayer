using Microsoft.EntityFrameworkCore;
using PalmClient.Commands;
using PalmClient.Services;
using PalmData.Data;
using PalmData.DataEntities;
using PalmClient.Models;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PalmClient.Events;
using System.Diagnostics;
using PalmClient.Stores;
using PalmClient.Interfaces;
using System.Windows;
using PalmClient.Extensions;
using PalmClient.Core;
using PalmClient.Enums;
using static System.Net.WebRequestMethods;

namespace PalmClient.ViewModels
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
