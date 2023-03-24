using dotnet_player_client.Observables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Models
{
    internal class PLModel : ObsObject
    {
        private int? pl_ID;
        private bool? pl_isSelect;
        private bool? pl_isPlaying;
        private string? pl_Name;

        public int? ID
        {
            get { return pl_ID; }
            set
            {
                if(pl_ID != value)
                {
                    pl_ID = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool? IsSelected
        {
            get { return pl_isSelect; }
            set
            {
                if (pl_isSelect != value)
                {
                    pl_isSelect = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool? IsPlaying
        {
            get { return pl_isPlaying; }
            set
            {
                if (pl_isPlaying != value)
                {
                    pl_isPlaying = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Name
        {
            get { return pl_Name; }
            set
            {
                if (pl_Name != value)
                {
                    pl_Name = value;
                    OnPropertyChanged();
                }
            }
        }

    }
}
