using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TaskList.View
{
    /// <summary>
    /// Logique d'interaction pour DialogView.xaml
    /// </summary>
    public partial class DialogView : Window
    {
        public bool GetIsDone => isDone.IsChecked.HasValue && isDone.IsChecked.Value;
        public string GetTitle => title.Text;
        public string GetDescription => description.Text;

        public DialogView(bool isEditMode = false, String? _title = null, String? _description = null, bool? _isDone = null)
        {
            InitializeComponent();

            // "Title" is the window title, not the task title.
            if (isEditMode)
            {
                Title = "Edit Task";
                title.Text = _title;
                description.Text = _description;
                isDone.IsChecked = _isDone;
            }
            else
            {
                Title = "New Task";
                isDone.IsEnabled = false;
            }
        }

        private void CLickOK(object sender, RoutedEventArgs e)
        {
            DialogResult = title.Text != null && !title.Text.Equals(String.Empty);
        }
    }
}
