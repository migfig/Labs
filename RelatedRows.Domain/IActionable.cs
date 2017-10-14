using RelatedRows.Helpers;
using System.Collections.Generic;

namespace RelatedRows.Domain
{
    public interface IActionable<T> where T: class
    {
        IEnumerable<Actionable<T>> Actions { get; }
    }

    public class Actionable<T> where T: class
    {
        public string Title { get; private set; }
        public string Icon { get; private set; }
        public Command<T> ActionCommand { get; private set; }

        public Actionable(string title, string icon, Command<T> command)
        {
            Title = title;
            Icon = icon;
            ActionCommand = command;
        }
    }
}
