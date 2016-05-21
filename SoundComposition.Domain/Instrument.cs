using System.Linq;
using System.Collections.ObjectModel;
using Windows.Storage;
using Windows.UI.Composition;

namespace SoundComposition.Domain
{
    public partial class Instrument: CompositionBase
    {
        public string Path { get; set; }

        public string Name { get; set; }
        public string Class { get; set; }

        protected StorageFolder _folder;

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

        public Visual Element
        {
            get { return _element; }
        }

        public Instrument(StorageFolder folder)
        {
            _folder = folder;
            Path = folder.Path;
            Name = Path.Split('\\').Last();
        }
        public override string ToString()
        {
            return Path;
        }
    }
}
