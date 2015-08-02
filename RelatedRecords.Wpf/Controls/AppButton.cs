using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Input;
using WPFSpark;

namespace RelatedRecords.Wpf.Controls
{
    public class AppButton : Button
    {
        static AppButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AppButton), 
                new FrameworkPropertyMetadata(typeof(AppButton)));
        }

        public AppButton()
            : base()
        {
            var behaviors = Interaction.GetBehaviors(this);
            behaviors.Add(new FluidMouseDragBehavior { DragButton = MouseButton.Right });
        }

        /// <summary>
        /// Gets or sets the Visibility for the Icon of the RibbonWindow that contains this Ribbon.
        /// </summary>
        public Visibility IsDefaultVisibility
        {
            get { return (Visibility)GetValue(IsDefaultVisibilityProperty); }
            set { SetValue(IsDefaultVisibilityProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for WindowIconVisibility.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty IsDefaultVisibilityProperty = DependencyProperty.Register(
            "IsDefaultVisibility",
            typeof(Visibility),
            typeof(AppButton),
            new UIPropertyMetadata(Visibility.Collapsed, OnIsDefaultVisibilityChanged));

        private static void OnIsDefaultVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
    }
}
