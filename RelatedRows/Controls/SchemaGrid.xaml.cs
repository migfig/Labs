using System.Windows.Controls;

namespace RelatedRows.Controls
{
    /// <summary>
    /// Interaction logic for Grids.xaml
    /// </summary>
    public partial class SchemaGrid : UserControl
    {
        public SchemaGrid()
        {
            InitializeComponent();
        }

        private void OnActionsClick(object sender, System.Windows.RoutedEventArgs e)
        {
            var button = sender as Button;
            button.ContextMenu.IsOpen = true;
            e.Handled = true;
        }
    }
}
