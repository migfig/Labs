using Common;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SoundPlayer.Models
{
    public enum eMovement
    {
        None,
        Up,
        Down,
        Right,
        Left
    }
    public partial class Instrument: BaseModel
    {
        public string Path { get; set; }
        public string Name { get; set; }

        private IEnumerable<Point> _points;
        public IEnumerable<Point> Points
        {
            get { return _points; }
            set
            {
                _points = value;
            }
        }

        public eMovement Movement
        {
            get
            {
                if(Points != null && Points.Any())
                {
                    var deltaX = Points.Max(x => x.X) - Points.Min(x => x.X);
                    var deltaY = Points.Max(x => x.Y) - Points.Min(x => x.Y);
                    var negativeDirection = (Points.Last().X < Points.First().X)
                        || (Points.Last().Y < Points.First().Y);
                    if (deltaX > deltaY)
                    {
                        if (negativeDirection)
                            return eMovement.Left;
                        else
                            return eMovement.Right;
                    }
                    else if (deltaY > deltaX)
                    {
                        if (negativeDirection)
                            return eMovement.Down;
                        else
                            return eMovement.Up;
                    }
                }

                return eMovement.None;
            }
        }

        private ObservableCollection<Song> _songs;
        public ObservableCollection<Song> Songs
        {
            get { return _songs; }
            set
            {
                _songs = value;
            }
        }

        private Song _selectedSong;
        [JsonIgnore]
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

        public Instrument()
        {
            _songs = new ObservableCollection<Song>();
        }
        public Instrument(string path)
        {
            Path = path;
            Name = path.Split('\\').Last();
            LoadSongs();
        }
        public override string ToString()
        {
            return Path;
        }
    }
}
