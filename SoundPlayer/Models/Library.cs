using Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;

namespace SoundPlayer.Models
{
    public class Library: BaseModel
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
                _selectedInstrument.LoadSongs();
                OnPropertyChanged();
            }
        }

        public Library()
        {
            LoadInstruments();
        }

        private void LoadInstruments()
        {
            _instruments = new ObservableCollection<Instrument>(
                from d in Directory.GetDirectories(ConfigurationManager.AppSettings["instrumentsDirectory"])
                    .Where(x => !x.EndsWith("zip"))
                select new Instrument(d));
            SelectedInstrument = _instruments.FirstOrDefault();
        }
    }
}
