using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace Visor.VStudio
{
    public interface IChildWindow
    {
        UserControl Content { get; }
    }

    public interface ITitledWindow
    {
        string Title { get; }
    }

    public interface IPlugableWindow
    {
        void Attach();
    }

    [Export(typeof(IPlugableWindow))]
    public class PlugableWindow : UserControl, IPlugableWindow
    {
        [ImportMany]
        IEnumerable<Lazy<IChildWindow, ITitledWindow>> children;

        public void Attach()
        {
            foreach(Lazy<IChildWindow, ITitledWindow> child in children)
            {
                var tab = this.FindName("tabControl") as TabControl;
                if(tab != null)
                {
                    tab.Items.Add(new TabItem { Header = child.Metadata.Title, Content = child.Value.Content });
                }
            }            
        }
    }
}
