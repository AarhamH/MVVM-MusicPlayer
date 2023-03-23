using dotnet_player_client.Enumeration;
using System.Collections.Generic;


namespace dotnet_player_client.Arguments
{
    public class PageChangeArgs
    {
        public PageType Page { get; set; }

        public PageChangeArgs(PageType page) {  this.Page = page; }
    }
}
