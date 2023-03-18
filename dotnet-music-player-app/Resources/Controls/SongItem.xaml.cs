using System.Windows;
using System.Windows.Controls;


namespace dotnet_music_player_app.Resources.Controls
{

    public partial class SongItem : UserControl
    {
        public SongItem()
        {
            InitializeComponent();
        }

        public string Title {  get { return (string)GetValue(titleProperty); }
                               set { SetValue(titleProperty, value); } }

        public static readonly DependencyProperty titleProperty = DependencyProperty.Register("Title", typeof(string),typeof(SongItem));

        public string Number
        {
            get { return (string)GetValue(numberProperty); }
            set { SetValue(numberProperty, value); }
        }

        public static readonly DependencyProperty numberProperty = DependencyProperty.Register("Number", typeof(string), typeof(SongItem));

        public string Time
        {
            get { return (string)GetValue(timeProperty); }
            set { SetValue(timeProperty, value); }
        }

        public static readonly DependencyProperty timeProperty = DependencyProperty.Register("Time", typeof(string), typeof(SongItem));

        public bool IsActive
        {
            get { return (bool)GetValue(activeProperty); }
            set { SetValue(activeProperty, value); }
        }

        public static readonly DependencyProperty activeProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(SongItem));

    }
}
