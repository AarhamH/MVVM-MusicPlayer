using PalmClient.Enums;
using PalmData.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalmClient.Events
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
