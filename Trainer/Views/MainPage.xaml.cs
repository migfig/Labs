using Trainer.Domain;
using Trainer.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Trainer.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var presentation = (sender as Button).Tag as Presentation;
            MainViewModel.ViewModel.SelectedPresentationTitle = presentation.Title;
            (DataContext as MainPageViewModel).GotoSlides(presentation.Title);
        }
    }
}
