using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SoundComposer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }        

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var player = BackgroundMediaPlayer.Current;
            player.AutoPlay = true;
            player.IsLoopingEnabled = true;
            //player.MediaEnded += (o, e) => player.cl
            player.SetUriSource(new Uri(@"C:\Users\migfig\Music\Samples\cello\cello_A2_1_forte_arco-normal.mp3"));
            player.Play();

        }

        private void Viewbox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (vbCanvas.Children.First() as TextBlock).Text = string.Format("VB Tapped {0}, (X:{1},Y:{2})"
                , DateTime.Now.ToString("hh:mm:ss"), e.GetPosition(vbCanvas).X, e.GetPosition(vbCanvas).Y);
        }

        private void Viewbox_Holding(object sender, HoldingRoutedEventArgs e)
        {
            (vbCanvas.Children.First() as TextBlock).Text = string.Format("VB Holding {0}, (X:{1},Y:{2})"
                , DateTime.Now.ToString("hh:mm:ss"), e.GetPosition(vbCanvas).X, e.GetPosition(vbCanvas).Y);

        }

        private void Viewbox_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            (vbCanvas.Children.First() as TextBlock).Text = string.Format("VB Manipulation Started {0}, (X:{1},Y:{2})"
                , DateTime.Now.ToString("hh:mm:ss"), e.Position.X, e.Position.Y);

        }

        private void Viewbox_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            (vbCanvas.Children.First() as TextBlock).Text = string.Format("VB Manipulation Delta {0}, (X:{1},Y:{2})"
                , DateTime.Now.ToString("hh:mm:ss"), e.Position.X, e.Position.Y);

        }

        private void Viewbox_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            (vbCanvas.Children.First() as TextBlock).Text = string.Format("VB Pointer pressed {0}, (X:{1},Y:{2})"
                , DateTime.Now.ToString("hh:mm:ss"), e.GetCurrentPoint(vbCanvas).Position.X, e.GetCurrentPoint(vbCanvas).Position.Y);

        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width <= 768)
            {
                VisualStateManager.GoToState(this, "Below768Layout", true);
            }
            else if (e.NewSize.Width <= 1130)
            {
                VisualStateManager.GoToState(this, "Below1130Layout", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "DefaultLayout", true);
            }
        }
    }
}
