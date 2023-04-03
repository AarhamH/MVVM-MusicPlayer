using dotnet_player_client.Enums;
using dotnet_player_data.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Events
{
    public class PageChangedEventArgs : EventArgs
    {
        public PageType Page { get; set; }

        public PageChangedEventArgs(PageType page)
        {
            Page = page;
        }
    }
}
