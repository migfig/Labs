using System;
using System.Linq;
using System.Threading;

namespace SoundPlayer.Models
{
    public partial class Composition : IPlayer
    {
        public void Play()
        {
            var songs = from i in Instruments
                        from s in i.Songs
                        where Notes.Contains(s.Note)
                            && Octaves.Contains(s.Octave)
                            && Tempos.Contains(s.Tempo)
                            || Intensities.Contains(s.Intensity)
                            || Modes.Contains(s.Mode)
                        select s;

            songs.ToList().ForEach(x =>
            {
                x.Play();
                Thread.Sleep(TimeSpan.FromMilliseconds(1000));
            });
        }
    }
}
