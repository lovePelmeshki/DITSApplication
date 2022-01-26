using System.Windows;


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
            StationTextBox.Text = MainWindow.GetStation((int)incident.StationId).StationName;
            if (incident.PostId != null)
            {
                PostTextBox.Text = MainWindow.GetPost((int)incident.PostId).PostName;
            }
            LineTextBox.Text = MainWindow.GetLineByStationId((int)incident.StationId).LineName;
            if (incident.EmployeeId !=null)
            {
                CreatorTextBlock.Text = MainWindow.GetEmployee((int)incident.EmployeeId).Lastname;
            }
            RespoinderComboBox.SelectedValue = incident.ResponderId;


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
