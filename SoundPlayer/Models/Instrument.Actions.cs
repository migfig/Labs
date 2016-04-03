using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;

namespace SoundPlayer.Models
{
    public partial class Instrument
    {
        public void LoadSongs()
        {
            if (null != _songs) return;

            _songs = new ObservableCollection<Song>(
                from f in Directory.GetFiles(Path, ConfigurationManager.AppSettings["songExtension"])
                select new Song(f)
            );
        }
    }
}
