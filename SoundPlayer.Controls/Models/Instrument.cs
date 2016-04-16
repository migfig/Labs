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

        private Dictionary<string, IEnumerable<Point>> _points = new Dictionary<string, IEnumerable<Point>>();
        public IEnumerable<Point> GetPoints(string item)
        {
            return _points[item];
        }
        public void SetPoints(string item, IEnumerable<Point> points)
        {
            _points[item] = points;
        }

        public eMovement GetMovement(string item)
        {
            var points = GetPoints(item);
            if (points != null && points.Any())
            {
                var deltaX = points.Max(x => x.X) - points.Min(x => x.X);
                var deltaY = points.Max(x => x.Y) - points.Min(x => x.Y);
                var negativeDirection = (points.Last().X < points.First().X)
                    || (points.Last().Y < points.First().Y);
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
