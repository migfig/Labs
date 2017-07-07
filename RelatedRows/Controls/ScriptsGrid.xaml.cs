using System.Windows.Controls;
using System.Linq;
using RelatedRows.Domain;

namespace RelatedRows.Controls
{
    /// <summary>
    /// Interaction logic for Grids.xaml
    /// </summary>
    public partial class ScriptsGrid : UserControl
    {
        public ScriptsGrid()
        {
            InitializeComponent();
        }

        private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var script = (DataContext as WindowViewModel).SelectedScriptQuery;
            if (script != null)
                script.SelectedParameters = e.AddedCells.Select(c => c.Item as CParameter).Distinct();
        }
    }
}
