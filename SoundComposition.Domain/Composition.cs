using System.Collections.ObjectModel;

namespace SoundComposition.Domain
{
    public partial class Composition
    {
        public Composition()
        {
            Pause = 1000;
            MaxSongs = 15;
            _instruments = new ObservableCollection<Instrument>();
            _notes = new ObservableCollection<string>();
            _octaves = new ObservableCollection<int>();
            _tempos = new ObservableCollection<float>();
            _intensities = new ObservableCollection<string>();
            _modes = new ObservableCollection<string>();
        }

        public string Name { get; set; }
        public int Pause { get; set; }
        public int MaxSongs { get; set; }

        private ObservableCollection<Instrument> _instruments;
        public ObservableCollection<Instrument> Instruments
        {
            get { return _instruments; }
            set
            {
                _instruments = value;
            }
        }

        private ObservableCollection<string> _notes;
        public ObservableCollection<string> Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
            }
        }

        private ObservableCollection<int> _octaves;
        public ObservableCollection<int> Octaves
        {
            get { return _octaves; }
            set
            {
                _octaves = value;
            }
        }

        private ObservableCollection<float> _tempos;
        public ObservableCollection<float> Tempos
        {
            get { return _tempos; }
            set
            {
                _tempos = value;
            }
        }

        private ObservableCollection<string> _intensities;
        public ObservableCollection<string> Intensities
        {
            get { return _intensities; }
            set
            {
                _intensities = value;
            }
        }

        private ObservableCollection<string> _modes;
        public ObservableCollection<string> Modes
        {
            get { return _modes; }
            set
            {
                _modes = value;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
