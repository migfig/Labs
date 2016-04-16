using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace SoundPlayer.Models
{
    public partial class Instrument
    {        
        private Dictionary<string, int> Indexes = new Dictionary<string, int>();
        private Dictionary<string, IEnumerable<object>> Items = new Dictionary<string, IEnumerable<object>>();

        public int Get(string key)
        {
            return Indexes[key];
        }
        private int Set(string key, int value)
        {
            Indexes[key] = value;
            return value;
        }

        public int SetNext(string key)
        {
            var current = Get(key);
            if (current + 1 < Items[key].Count())
                return Set(key, current + 1);
            else
                return Set(key, 0);
        }
        public int SetPrev(string key)
        {
            var current = Get(key);
            if (current - 1 > 0)
                return Set(key, current - 1);
            else
                return Set(key, Items[key].Count()-1);
        }

        public const string kNotes = "notes";
        public string SelectedNote
        {
            get { return Notes.ElementAt(Indexes[kNotes]).ToString(); }
        }
        public IEnumerable<string> Notes
        {
            get
            {
                if (!Items.ContainsKey(kNotes))
                {
                    Items.Add(kNotes, Songs.Select(x => x.Note).Distinct());
                    Indexes.Add(kNotes, 0);
                }
                return Items[kNotes].Cast<string>();
            }
        }

        public const string kOctaves = "octaves";
        public int SelectedOctave
        {
            get { return (int)Octaves.ElementAt(Indexes[kOctaves]); }            
        }        
        public IEnumerable<int> Octaves
        {
            get
            {
                if (!Items.ContainsKey(kOctaves))
                {
                    Items.Add(kOctaves, Songs.Select(x => x.Octave).Distinct().Cast<object>());
                    Indexes.Add(kOctaves, 0);
                }
                return Items[kOctaves].Cast<int>();
            }
        }

        public const string kTempos = "tempos";
        public float SelectedTempo
        {
            get { return (float)Tempos.ElementAt(Indexes[kTempos]); }
        }
        public IEnumerable<float> Tempos
        {
            get
            {
                if (!Items.ContainsKey(kTempos))
                {
                    Items.Add(kTempos, Songs.Select(x => x.Tempo).Distinct().Cast<object>());
                    Indexes.Add(kTempos, 0);
                }
                return Items[kTempos].Cast<float>();
            }
        }

        public const string kIntensities = "intensities";
        public string SelectedIntensity
        {
            get { return (string)Intensities.ElementAt(Indexes[kIntensities]); }
        }
        public IEnumerable<string> Intensities
        {
            get
            {
                if (!Items.ContainsKey(kIntensities))
                {
                    Items.Add(kIntensities, Songs.Select(x => x.Intensity).Distinct());
                    Indexes.Add(kIntensities, 0);
                }
                return Items[kIntensities].Cast<string>();
            }
        }

        public const string kModes = "modes";
        public string SelectedMode
        {
            get { return (string)Modes.ElementAt(Indexes[kModes]); }
        }
        public IEnumerable<string> Modes
        {
            get
            {
                if (!Items.ContainsKey(kModes))
                {
                    Items.Add(kModes, Songs.Select(x => x.Mode).Distinct());
                    Indexes.Add(kModes, 0);
                }
                return Items[kModes].Cast<string>();
            }
        }

        public void LoadSongs()
        {
            if (null != _songs) return;

            _songs = new ObservableCollection<Song>(
                from f in Directory.GetFiles(Path, "*.mp3")
                select new Song(f)
            );
        }
    }
}
