using ApiTester.Models;
using ApiTester.Wpf.ViewModels;
using Common;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace ApiTester.Client.Controls
{
    /// <summary>
    /// Interaction logic for TaskControl.xaml
    /// </summary>
    public partial class TaskControl : UserControl
    {
        public TaskControl()
        {
            InitializeComponent();
        }

        private void expanderTasks_Collapsed(object sender, RoutedEventArgs e)
        {
            this.Width = 200;
            this.Height = 132;

            var task = (sender as Expander).Tag as Task;
            if (MainViewModel.ViewModel.SaveTask.CanExecute(null))
            {
                try
                {
                    var taskObj = XmlHelper<Task>.Load(XElement.Load(new StringReader(task.xml)));
                    taskObj.Passed = task.Passed;
                    taskObj.ResultsObject = task.ResultsObject;
                    taskObj.SelectedCondition = task.SelectedCondition;
                    taskObj.SelectedOperator = task.SelectedOperator;
                    taskObj.SelectedProperty = task.SelectedProperty;

                    MainViewModel.ViewModel.SaveTask.Execute(taskObj);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }

        private void expanderTasks_Expanded(object sender, RoutedEventArgs e)
        {
            this.Width = 600;
            this.Height = 500;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var task = (sender as Button).Tag as Task;
            if (MainViewModel.ViewModel.RemoveTask.CanExecute(null))
            {
                MainViewModel.ViewModel.RemoveTask.Execute(task);
            }
        }
    }
}
