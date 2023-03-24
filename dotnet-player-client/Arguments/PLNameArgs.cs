using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Arguments
{
    public class PLNameArgs : EventArgs
    {
        public int ID {  get; set; }
        public string Name { get; set; }
        public PLNameArgs(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
