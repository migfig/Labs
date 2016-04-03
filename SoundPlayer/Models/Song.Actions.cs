using System;
using System.Threading;
using System.Windows.Media;

namespace SoundPlayer.Models
{
    public partial class Song
    {
        public void Play()
        {
            if (!string.IsNullOrWhiteSpace(FileName))
            {
                ThreadPool.QueueUserWorkItem((object state) => {
                    var player = new MediaPlayer();
                    player.Open(new Uri(FileName));
                    player.Play();
                });
            }
        }
    }
}
