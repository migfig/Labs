using System.Collections.ObjectModel;

namespace SoundPlayer.Models
{
    public partial class Instrument
    {
        public string Path { get; set; }

        public string Name { get; set; }
        public string Class { get; set; }

        private ObservableCollection<Song> _songs;
        public ObservableCollection<Song> Songs
        {
            get { return _songs; }
        }

        private Song _selectedSong;
        public Song SelectedSong
        {
            get { return _selectedSong; }
            set
            {
                _selectedSong = value;
                if (value != null && !string.IsNullOrWhiteSpace(value.FileName))
                {
                    value.Play();
                }
            }
        }

        public Instrument(string path)
        {
            Path = path;
        }
        public override string ToString()
        {
            return Path;
        }
    }
}
