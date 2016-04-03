using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SoundPlayer.Models
{
    public partial class Library
    {
        private string _selectedNote;
        public string SelectedNote
        {
            get { return _selectedNote; }
            set
            {
                _selectedNote = value;
                OnPropertyChanged();
                if(null != SelectedComposition)
                {
                    SelectedComposition.Notes = new ObservableCollection<string>{ value };
                }
            }
        }

        private IEnumerable<string> _notes;
        public IEnumerable<string> Notes
        {
            get
            {
                if (_notes == null)
                {
                    _notes = (from i in _instruments
                              from s in i.Songs
                              select s.Note).Distinct();
                }
                return _notes;
            }
        }

        private int _selectedOctave;
        public int SelectedOctave
        {
            get { return _selectedOctave; }
            set
            {
                _selectedOctave = value;
                OnPropertyChanged();
                if (null != SelectedComposition)
                {
                    SelectedComposition.Octaves = new ObservableCollection<int> { value };
                }
            }
        }

        private IEnumerable<int> _octaves;
        public IEnumerable<int> Octaves
        {
            get
            {
                if(_octaves == null)
                {
                    _octaves = (from i in _instruments
                                from s in i.Songs
                                select s.Octave).Distinct();
                }
                return _octaves;
            }
        }

        private IEnumerable<float> _tempos;
        public IEnumerable<float> Tempos
        {
            get
            {
                if(_tempos == null)
                {
                    _tempos = (from i in _instruments
                               from s in i.Songs
                               select s.Tempo).Distinct();
                }
                return _tempos;
            }
        }

        private IEnumerable<string> _intensities;
        public IEnumerable<string> Intensities
        {
            get
            {
                if(_intensities == null)
                {
                    _intensities = (from i in _instruments
                                    from s in i.Songs
                                    select s.Intensity).Distinct();
                }
                return _intensities;
            }
        }

        private IEnumerable<string> _modes;
        public IEnumerable<string> Modes
        {
            get
            {
                if(_modes == null)
                {
                    _modes = (from i in _instruments
                              from s in i.Songs
                              select s.Mode).Distinct();
                }
                return _modes;
            }
        }
    }
}
