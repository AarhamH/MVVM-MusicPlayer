using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace dotnet_player_client.Observables
{
    public abstract class ObsObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
