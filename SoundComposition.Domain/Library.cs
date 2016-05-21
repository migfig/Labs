using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SoundComposition.Domain
{
    public partial class Library: BaseModel
    {
        private ObservableCollection<Instrument> _instruments;
        public IEnumerable<Instrument> Instruments
        {
            get { return _instruments; }
        }

        private Instrument _selectedInstrument;
        public Instrument SelectedInstrument
        {
            get { return _selectedInstrument; }
            set
            {
                _selectedInstrument = value;
                if (null != value)
                {
                    _selectedInstrument.LoadSongs().GetAwaiter().GetResult();
                }
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Composition> _compositions;
        public ObservableCollection<Composition> Compositions
        {
            get { return _compositions; }
        }

        private Composition _selectedComposition;
        public Composition SelectedComposition
        {
            get { return _selectedComposition; }
            set
            {
                _selectedComposition = value;
                if (null != value)
                {
                    _selectedComposition.Play();
                }
                OnPropertyChanged();
            }
        }

        private int _pause = 100;
        public int Pause
        {
            get { return _pause; }
            set
            {
                _pause = value;
                OnPropertyChanged();
                if (null != SelectedComposition)
                {
                    SelectedComposition.Pause = value;
                }
            }
        }

        private int _maxSongs = 20;
        public int MaxSongs
        {
            get { return _maxSongs; }
            set
            {
                _maxSongs = value;
                OnPropertyChanged();
                if (null != SelectedComposition)
                {
                    SelectedComposition.MaxSongs = value;
                }
            }
        }

        public async Task LoadInstruments()
        {
            var folder = await Windows.Storage.KnownFolders.MusicLibrary.GetFolderAsync("Samples");
            var folders = await folder.GetFoldersAsync();
            _instruments = new ObservableCollection<Instrument>(
                from d in folders
                    .Where(x => !x.Name.EndsWith("zip"))
                select new Instrument(d));

            foreach(var instrument in _instruments)
            {
                await instrument.LoadSongs();
            }
            SelectedInstrument = _instruments.FirstOrDefault();

            //LoadCompositions();
            _compositions = new ObservableCollection<Composition>();
        }

        private void LoadCompositions()
        {
            _compositions = new ObservableCollection<Composition>();
            _compositions.Add(new Composition
            {
                Name = "Sample1",
                MaxSongs = MaxSongs,
                Pause = Pause,
                Instruments = new ObservableCollection<Instrument>(Instruments),
                Notes = new ObservableCollection<string>(Notes.Where(x => x.StartsWith("C"))),
                Octaves = new ObservableCollection<int>(Octaves.Where(x => x.Equals(3))),
                Tempos = new ObservableCollection<float>(Tempos.Where(x => x.Equals(1.0F)))//,
                //Intensities = new ObservableCollection<string>(Intensities.Where(x => x.Contains("-"))),
                //Modes = new ObservableCollection<string>(Modes.Where(x => x.Contains("-")))
            });
            _compositions.Add(new Composition
            {
                Name = "Sample2",
                MaxSongs = MaxSongs,
                Pause = Pause,
                Instruments = new ObservableCollection<Instrument>(Instruments),
                Notes = new ObservableCollection<string>(Notes.Where(x => x.StartsWith("G"))),
                Octaves = new ObservableCollection<int>(Octaves.Where(x => x.Equals(5))),
                Tempos = new ObservableCollection<float>(Tempos.Where(x => x.Equals(1.0F)))//,
                //Intensities = new ObservableCollection<string>(Intensities.Where(x => x.Contains("-"))),
                //Modes = new ObservableCollection<string>(Modes.Where(x => x.Contains("-")))
            });
        }
    }
}
