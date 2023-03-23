namespace dotnet_player_data.Objects
{
    public class SongObjects
    {
        public string? Path { get; set; }
        public virtual int? ListID { get; set; }
        public virtual PlayListObject? PlayList { get; set; }
    }
}
