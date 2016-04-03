using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SoundPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty selectedInstrumentProp =
            DependencyProperty.Register("SelectedInstrument", typeof(string), typeof(MainWindow));

        public static readonly DependencyProperty instrumentsProp =
            DependencyProperty.Register("Instruments", typeof(ObservableCollection<string>), typeof(MainWindow));

        public static readonly DependencyProperty selectedSongProp =
            DependencyProperty.Register("SelectedSong", typeof(string), typeof(MainWindow));

        public static readonly DependencyProperty songsProp = 
            DependencyProperty.Register("Songs", typeof(ObservableCollection<string>), typeof(MainWindow));

        public ObservableCollection<string> Songs
        {
            get { return (ObservableCollection<string>)GetValue(songsProp); }
        }

        public string SelectedSong
        {
            get { return (string)GetValue(selectedSongProp); }
            set
            {
                SetValue(selectedSongProp, value);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    var player = new MediaPlayer();
                    player.Open(new Uri(value));
                    player.Play();
                }
            }
        }

        public ObservableCollection<string> Instruments
        {
            get { return (ObservableCollection<string>)GetValue(instrumentsProp); }
        }

        public string SelectedInstrument
        {
            get { return (string)GetValue(selectedInstrumentProp); }
            set
            {
                SetValue(selectedInstrumentProp, value);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += (o, e) =>
            {
                SetValue(instrumentsProp, new ObservableCollection<string>(
                    Directory.GetDirectories(@"C:\Users\migfig\Music\Samples").Where(x => !x.EndsWith("zip"))));
                var instrument = ((ObservableCollection<string>)GetValue(instrumentsProp)).First();
                SelectedInstrument = instrument;
                SetValue(songsProp, new ObservableCollection<string>(
                    Directory.GetFiles(instrument, "*.mp3")));
            };
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = sender as ListBox;
            if (list.SelectedIndex >= 0)
            {
                var song = list.Items[list.SelectedIndex] as string;
                SelectedSong = song;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = sender as ComboBox;
            if (list.SelectedIndex >= 0)
            {
                var instrument = list.Items[list.SelectedIndex] as string;
                SelectedInstrument = instrument;
                SetValue(songsProp, new ObservableCollection<string>(
                    Directory.GetFiles(instrument, "*.mp3")));
            }

        }
    }
}
