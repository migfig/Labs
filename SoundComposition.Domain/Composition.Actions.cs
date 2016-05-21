using System.Collections.Generic;
using System.Linq;

namespace SoundComposition.Domain
{
    public partial class Composition : IPlayer
    {
        public void Play()
        {
            var songs = from i in Instruments
                        from s in i.Songs
                        where Notes.Any() ? Notes.Contains(s.Note) : true
                            && Octaves.Any() ? Octaves.Contains(s.Octave) : true
                            && Tempos.Any() ? Tempos.Contains(s.Tempo) : true
                            && Intensities.Any() ? Intensities.Contains(s.Intensity) : true
                            && Modes.Any() ? Modes.Contains(s.Mode) : true
                        group s by s.Instrument into grouping
                        select grouping;

            var songsDict = new Dictionary<string, List<Song>>();
            foreach (IGrouping<string, Song> grouping in songs)
            {
                var lstSongs = new List<Song>();
                foreach (var s in grouping.Take(MaxSongs))
                {
                    lstSongs.Add(s);
                }
                songsDict.Add(grouping.Key, lstSongs);
            }

            foreach(var instrument in songsDict.Keys)
            {
                var song = songsDict[instrument].ElementAt(0);
                song.Play();
                songsDict[instrument].RemoveAt(0);
            }
        }
    }
}
