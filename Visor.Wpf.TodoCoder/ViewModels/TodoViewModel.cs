using System.Collections.Generic;
using Visor.Wpf.TodoCoder.Models;

namespace Visor.Wpf.TodoCoder.ViewModels
{
    public class TodoViewModel
    {
        private static TodoViewModel _viewModel = new TodoViewModel();
        public static TodoViewModel ViewModel { get { return _viewModel; } }

        public IEnumerable<TodoItem> Items
        {
            get {
                return new List<TodoItem>
                {
                    new TodoItem
                    {
                        IsDone = false,
                        ClassName = "Sample.Code\\Models\\BaseModel.cs",
                        Code = @"public class BaseModel {
                                    public bool IsBusy {get;set;}
                                 }"
                    }
                };
            }
        }
    }
}
