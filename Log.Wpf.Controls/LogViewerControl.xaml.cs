//------------------------------------------------------------------------------
// <copyright file="LogViewerControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Log.Wpf.Controls
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using ViewModels;
    /// <summary>
    /// Interaction logic for LogViewerControl.
    /// </summary>
    public partial class LogViewerControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogViewerControl"/> class.
        /// </summary>
        public LogViewerControl()
        {
            DataContext = LogViewModel.ViewModel;
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Invoked '{0}'", this.ToString()),
                "LogViewer");
        }

        private void OnComboBoxKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Enter)
            {
                LogViewModel.ViewModel.SelectedFilter = (sender as ComboBox).Text;
            }
        }
    }
}