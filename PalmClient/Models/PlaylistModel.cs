using PalmClient.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PalmClient.Models
{
    public class PlaylistModel : ObservableObject
    {
        private int? _id;
        private bool? _isSelected;
        private bool? _isPlaying;
        private string? _name;
        private string? _banner;
        private DateTime? _creationDate;

        public int? Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool? IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool? IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                if (_isPlaying != value)
                {
                    _isPlaying = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Banner
        {
            get { return _banner; }
            set
            {
                if (_banner != value)
                {
                    _banner = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime? CreationDate
        {
            get { return _creationDate; }
            set
            {
                if (_creationDate != value)
                {
                    _creationDate = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
