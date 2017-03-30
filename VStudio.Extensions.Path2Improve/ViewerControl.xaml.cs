//------------------------------------------------------------------------------
// <copyright file="ViewerControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace VStudio.Extensions.Path2Improve
{
    using System.Windows.Controls;
    using ViewModels;
    using Visor.VStudio;
    using System.ComponentModel.Composition;
    using System.Windows;
    
    /// <summary>
    /// Interaction logic for ViewerControl.
    /// </summary>
    [Export(typeof(IChildWindow))]
    [ExportMetadata("Title", "Story Viewer")]
    public partial class ViewerControl : UserControl, IChildWindow, ITitledWindow
    {
        private IPlugableWindow _parentWindow;
        public string Title { get { return "Story Viewer"; } }
        UserControl IChildWindow.Content { get { return this; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewerControl"/> class.
        /// </summary>
        public ViewerControl()
        {
            DataContext = MainViewModel.ViewModel;
            this.InitializeComponent();
        }

        private void OnComboBoxKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Enter)
            {
                MainViewModel.ViewModel.SelectedFilter = (sender as ComboBox).Text;
            }
        }
        
        private void OnAddToIgnoreValuesCommandClick(object sender, RoutedEventArgs e)
        {
            var message = (sender as Button).CommandParameter;
            if(MainViewModel.ViewModel.AddToIgnoreValuesCommand.CanExecute(message))
            {
                MainViewModel.ViewModel.AddToIgnoreValuesCommand.Execute(message);
            }
        }

        public void SetParentWindow(IPlugableWindow window)
        {
            _parentWindow = window;
        }
    }
}