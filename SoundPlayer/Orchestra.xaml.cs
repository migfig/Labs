using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SoundPlayer.Models
{
    /// <summary>
    /// Interaction logic for Orchestra.xaml
    /// </summary>
    public partial class Orchestra : Window
    {
        public static readonly DependencyProperty libraryProp =
            DependencyProperty.Register("Library", typeof(Library), typeof(Orchestra));

        public Orchestra()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += (o, e) =>
            {
                SetValue(libraryProp, new Library());
            };
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            var instrument = (sender as ToggleButton).Tag as Instrument;
            (GetValue(libraryProp) as Library).SelectedInstrument = instrument;
        }

        private void btnOctave_Checked(object sender, RoutedEventArgs e)
        {
            var octave = int.Parse((sender as ToggleButton).Tag.ToString());
            (GetValue(libraryProp) as Library).SelectedOctave = octave;
        }

        private void btnNote_Checked(object sender, RoutedEventArgs e)
        {
            var note = (sender as ToggleButton).Tag.ToString();
            (GetValue(libraryProp) as Library).SelectedNote = note;
        }

        private void Ellipse_TouchMove(object sender, TouchEventArgs e)
        {
            var elipse = sender as Ellipse;
            var tp = e.GetTouchPoint(elipse);
        }

        private void Ellipse_TouchUp(object sender, TouchEventArgs e)
        {
        }

        private void elpInner_TouchMove(object sender, TouchEventArgs e)
        {

        }

        private void elpInner_TouchUp(object sender, TouchEventArgs e)
        {
 
        }

        private void Grid_TouchUp(object sender, TouchEventArgs e)
        {
            var library = GetValue(libraryProp) as Library;
            if (library.SelectedInstrument != null)
            {
                var songs = library.SelectedInstrument.Songs.ToArray();

                if (!string.IsNullOrEmpty(library.SelectedNote))
                {
                    songs = songs.Where(x => x.Note == library.SelectedNote).ToArray();
                    if (library.SelectedOctave > 0)
                    {
                        songs = songs.Where(x => x.Octave == library.SelectedOctave).ToArray();
                    }
                }

                var song = songs.FirstOrDefault();
                if (null != song)
                {
                    song.Play();
                }
            }

            //foreach(var instrument in library.Instruments)
            //{
            //    using (var stream = File.CreateText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, instrument.Name + ".json")))
            //    {
            //        stream.Write(JsonConvert.SerializeObject(instrument, Formatting.Indented));
            //    }
            //}
        }
    }
}
