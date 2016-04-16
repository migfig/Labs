using SoundPlayer.Models;
using System.Windows;

namespace SoundPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty libraryProp = 
            DependencyProperty.Register("Library", typeof(Library), typeof(MainWindow));

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += (o, e) =>
            {
                SetValue(libraryProp, new Library());
                cmdOrchestra_Click(this, null);
            };
        }

        private void cmdOrchestra_Click(object sender, RoutedEventArgs e)
        {
            var orchestra = new Orchestra();
            orchestra.Show();
        }
    }
}
