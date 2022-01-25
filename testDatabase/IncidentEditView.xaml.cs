using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace testDatabase
{
    /// <summary>
    /// Interaction logic for IncidentEditView.xaml
    /// </summary>
    public partial class IncidentEditView : Window
    {
        private Incident _incident;

        public IncidentEditView(Incident incident)
        {
            InitializeComponent();
            
            
            DataContext = incident;
            _incident = incident;
            HistoryDataGrid.ItemsSource = MainWindow.GetIncidentHistoryById(_incident.Id);
            RespoinderComboBox.ItemsSource = MainWindow.GetAllEmployees();
            StatusComboBox.ItemsSource = MainWindow.GetAllStatuses();
            StatusComboBox.SelectedValue = incident.StatusId;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            using (ditsdbContext db = new ditsdbContext())
            {

                _incident.Title = TitleTextBox.Text;
                _incident.Description = DescriptionTextBox.Text;
                _incident.ResponderId = RespoinderComboBox.SelectedValue as int?;
                _incident.StatusId = StatusComboBox.SelectedValue as int?;
                _incident.CloseDate = CloseDatePicker.SelectedDate;
                _incident.Comment = CommentTextBox.Text;
                db.Incidents.Update(_incident);
                db.SaveChanges();
                Close();
            }
        }


    }
}
