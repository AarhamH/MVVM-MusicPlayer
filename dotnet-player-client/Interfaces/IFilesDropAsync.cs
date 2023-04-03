using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Interfaces
{
    public interface IFilesDropAsync
    {
        Task OnFilesDroppedAsync(string[] files, object? parameter);
    }
}
