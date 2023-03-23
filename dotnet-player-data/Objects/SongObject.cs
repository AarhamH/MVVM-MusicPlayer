namespace dotnet_player_data.Objects
{
    public class SongObjects : IDObject
    {
        public string? Path { get; set; }
        public virtual int? ListID { get; set; }
        public virtual PlayListObject? PlayList { get; set; }
    }
}
