using System;
using System.Collections.Concurrent;
using Windows.Foundation;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.System.Threading;

namespace SoundComposition.Domain
{
    public partial class Song: IPlayer
    {
        private static ConcurrentStack<string> stack = new ConcurrentStack<string>();

        public async void Play()
        {
            if (!string.IsNullOrWhiteSpace(FileName))
            {
                await ThreadPool.RunAsync(async (IAsyncAction state) => {
                    var player = BackgroundMediaPlayer.Current;
                    player.MediaEnded += async (o, e) => {
                        if(stack.Count > 0)
                        {
                            var song = string.Empty;
                            if(stack.TryPop(out song))
                            {
                                if (player.CurrentState != MediaPlayerState.Playing)
                                {
                                    player.SetFileSource(await StorageFile.GetFileFromPathAsync(song));
                                    player.Volume = Volume;
                                    player.Play();
                                }
                                else
                                {
                                    stack.Push(FileName);
                                }
                            }
                        }
                    };

                    if (player.CurrentState == MediaPlayerState.Playing)
                    {
                        stack.Push(FileName);
                    }
                    else
                    {
                        player.SetFileSource(await StorageFile.GetFileFromPathAsync(FileName));
                        player.Volume = Volume;
                        player.Play();

                    }
                });
            }
        }
    }
}
