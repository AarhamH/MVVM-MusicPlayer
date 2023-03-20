using SQLite;
namespace dotnet_music_player_app.Resources.ViewModel
{
    public class SongItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public int Time { get; set; }

    }
}   