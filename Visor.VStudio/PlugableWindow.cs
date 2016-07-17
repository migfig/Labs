using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using EnvDTE80;
using EnvDTE;
using Trainer.Domain;

namespace Visor.VStudio
{
    public interface IChildWindow
    {
        UserControl Content { get; }
        void SetParentWindow(IPlugableWindow window);
    }

    public interface ITitledWindow
    {
        string Title { get; }
    }

    public interface IPlugableWindow
    {
        string ProgId { get; }
        void Attach();
        DTE2 Dte { get; set; }
        void Log(string format, params string[] message);
        bool AddCode(Component component);
        bool ViewCode(Component component);
        string ViewCode(ViewCodeArgs e);
    }

    [Export(typeof(IPlugableWindow))]
    public partial class PlugableWindow : UserControl, IPlugableWindow
    {
        [ImportMany]
        IEnumerable<Lazy<IChildWindow, ITitledWindow>> children;

        private OutputWindowPane _outputPane = null;

        public DTE2 Dte { get; set; }
        public string ProgId { get { return Properties.Settings.Default.VisualStudioProgId; } }

        public void Attach()
        {
            foreach(Lazy<IChildWindow, ITitledWindow> child in children)
            {
                var tab = this.FindName("tabControl") as TabControl;
                if(tab != null)
                {
                    child.Value.SetParentWindow(this);
                    tab.Items.Add(new TabItem { Header = child.Metadata.Title, Content = child.Value.Content });
                }
            }            
        }

        public void Log(string format, params string[] message)
        {
            Log(string.Format(format, message));
        }

        public bool AddCode(Component component)
        {
            return AddCode(component, null);
        }

        public bool ViewCode(Component component)
        {
            return ViewCode(new ViewCodeArgs(ProgId, component.TargetFile, 1, component.TargetProject)).Length.Equals(0);
        }
    }
}
