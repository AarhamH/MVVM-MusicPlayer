namespace dotnet_player_client.Observables
{
    public class ObsWrapper<O> : ObsObject
    {
        private O? _object;
        public O? Object
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
