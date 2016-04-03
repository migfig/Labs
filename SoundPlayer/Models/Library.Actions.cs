using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundPlayer.Models
{
    public partial class Library
    {
        public IEnumerable<string> Notes
        {
            get
            {
                return (from i in _instruments
                        from s in i.Songs
                        select s.Note).Distinct();
            }
        }

        public IEnumerable<int> Octaves
        {
            get
            {
                return (from i in _instruments
                        from s in i.Songs
                        select s.Octave).Distinct();
            }
        }

        public IEnumerable<float> Tempos
        {
            get
            {
                return (from i in _instruments
                        from s in i.Songs
                        select s.Tempo).Distinct();
            }
        }

        public IEnumerable<string> Intensities
        {
            get
            {
                return (from i in _instruments
                        from s in i.Songs
                        select s.Intensity).Distinct();
            }
        }

        public IEnumerable<string> Modes
        {
            get
            {
                return (from i in _instruments
                        from s in i.Songs
                        select s.Mode).Distinct();
            }
        }
    }
}
