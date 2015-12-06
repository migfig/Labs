using System.Windows;
using System.Windows.Controls;

namespace RelatedRecords.Client.Controls
{
    /// <summary>
    /// Interaction logic for BusyProgress.xaml
    /// </summary>
    [TemplateVisualState(GroupName = GroupIndeterminateStates, Name = IndeterminateInactive)]
    [TemplateVisualState(GroupName = GroupIndeterminateStates, Name = IndeterminateActive)]
    public partial class BusyProgress : Control
    {
        private const string GroupIndeterminateStates = "IndeterminateStates";
        private const string IndeterminateInactive = "Inactive";
        private const string IndeterminateActive = "Active";

        public static readonly DependencyProperty IsIndeterminateProperty = 
            DependencyProperty.Register("IsIndeterminate", 
                typeof(bool), 
                typeof(BusyProgress), 
                new PropertyMetadata(false, OnIsIndeterminateChanged));

        public BusyProgress()
        {
            this.DefaultStyleKey = typeof(BusyProgress);
        }

        private void GotoCurrentState(bool animate)
        {
            var state = this.IsIndeterminate ? IndeterminateActive : IndeterminateInactive;

            VisualStateManager.GoToState(this, state, animate);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            GotoCurrentState(false);
        }

        private static void OnIsIndeterminateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((BusyProgress)o).GotoCurrentState(true);
        }

        public bool IsIndeterminate
        {
            get { return (bool)GetValue(IsIndeterminateProperty); }
            set { SetValue(IsIndeterminateProperty, value); }
        }
    }
}
