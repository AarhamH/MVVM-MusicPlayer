using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Core
{
    public class ObservableWrapper<T> : ObservableObject
    {
        private T? _object;
        public T? Object
        {
            get => _object;
            set
            {
                _object = value;
                OnPropertyChanged();
            }
        }
    }
}
