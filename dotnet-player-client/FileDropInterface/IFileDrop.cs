using System.Threading.Tasks;

namespace dotnet_player_client.FileDropInterface
{
    public interface IFileDrop
    {
        Task OnFilesDropAsync(string[] files, object? parameter);
    }
}
