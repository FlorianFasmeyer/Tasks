using Microsoft.VisualBasic.FileIO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using TaskList.Command;
using TaskList.Model;
using TaskList.View;

namespace TaskList.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public System.Threading.Tasks.Task LoadDataTask { get; set; }
        public ObservableCollection<SimpleTask> Tasks { get; set; }
        
        private SimpleTask? selectedTask;
        public SimpleTask? SelectedTask
        {
            get { return selectedTask; }
            set { 
                selectedTask = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand AddCommand => new(Add);
        public RelayCommand RemoveCommand => new(Remove, _ => SelectedTask != null);
        public RelayCommand EditCommand => new(Edit, _ => SelectedTask != null);
        public RelayCommand DoneCommand => new(Done, _ => SelectedTask != null && SelectedTask.Status == false);

        public MainWindowViewModel()
        {
            Tasks = new ObservableCollection<SimpleTask>();
            LoadDataTask = SimpleTask.LoadDataAsync(Tasks);
        }

        
        void Add(object parameter)
        {
            DialogView inputDialog = new DialogView();
            if (inputDialog.ShowDialog() == false)
                return;

            SimpleTask task = new(inputDialog.GetTitle, inputDialog.GetDescription);
            Tasks.Insert(0, task);
        }

        void Remove(object parameter)
        {
            if (SelectedTask != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Are you sure you want to delete this task?", 
                    "Confirmation", 
                    MessageBoxButton.YesNo, 
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    Tasks.Remove(SelectedTask);
                    SelectedTask = null;
                }
            }
        }

        void Edit(object parameter)
        {
            if(SelectedTask != null)
            {
                DialogView inputDialog = new DialogView(
                    isEditMode: true, 
                    _title: SelectedTask.Title, 
                    _description: SelectedTask.Description,
                    _isDone: SelectedTask.Status);

                if (inputDialog.ShowDialog() == false)
                    return;

                SimpleTask updatedtask = new(inputDialog.GetTitle, inputDialog.GetDescription, inputDialog.GetIsDone);
                int index = Tasks.IndexOf(SelectedTask);
                Tasks[index] = updatedtask;
            }
        }

        void Done(object parameter)
        {
            if (SelectedTask != null)
            {
                SimpleTask updatedTask = new SimpleTask(SelectedTask.Title, SelectedTask.Description)
                {
                    Status = !SelectedTask.Status
                };

                int index = Tasks.IndexOf(SelectedTask);                
                Tasks[index] = updatedTask;
            }            
        }

        internal void OnMainWIndowClosing(CancelEventArgs e)
        {
            SimpleTask.SaveData(Tasks);
        }
    }
}
