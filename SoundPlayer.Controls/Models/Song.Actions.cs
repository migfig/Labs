using System;
using System.Threading;
using System.Windows.Media;

namespace SoundPlayer.Models
{
    public partial class Song: IPlayer
    {
        public void Play()
        {
            if (!string.IsNullOrWhiteSpace(FileName))
            {
                ThreadPool.QueueUserWorkItem((object state) => {
                    var player = new MediaPlayer();
                    player.Open(new Uri(FileName));
                    player.SpeedRatio = SpeedRatio;
                    player.Balance = Balance;
                    player.Volume = Volume;
                    player.MediaEnded += (o, e) => player.Close();
                    player.Play();
                });
            }
        }
    }
}
