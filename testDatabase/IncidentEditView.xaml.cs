using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Linq;


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
            RefreshItemsSources();
           

        }

        private void RefreshItemsSources()
        {
            HistoryDataGrid.ItemsSource = GetIncidentHistory(_incident.Id);
            RespoinderComboBox.ItemsSource = MainWindow.GetAllEmployees();
            StatusComboBox.ItemsSource = MainWindow.GetAllStatuses();
            StatusComboBox.SelectedValue = _incident.StatusId;
            StationTextBox.Text = MainWindow.GetStation((int)_incident.StationId).StationName;
            if (_incident.PostId != null)
            {
                PostTextBox.Text = MainWindow.GetPost((int)_incident.PostId).PostName;
            }
            LineTextBox.Text = MainWindow.GetLineByStationId((int)_incident.StationId).LineName;
            if (_incident.EmployeeId != null)
            {
                CreatorTextBlock.Text = MainWindow.GetEmployee((int)_incident.EmployeeId).Lastname;
            }
            RespoinderComboBox.SelectedValue = _incident.ResponderId;

        }
        private IList GetIncidentHistory(int incidentId)
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                var history = from inc in db.Incidents
                              where inc.Id == incidentId

                              join his in db.IncidentHistories
                              on inc.Id equals his.IncidentId

                              join station in db.Stations
                              on his.StationId equals station.Id

                              join post in db.Posts
                              on his.PostId equals post.Id into p
                              from post in p.DefaultIfEmpty()

                              join autor in db.Employees
                              on his.AutorId equals autor.Id

                              join resp in db.Employees
                              on his.RespoinderId equals resp.Id

                              join status in db.IncidentStatuses
                              on his.StatusId equals status.Id

                              orderby his.Id descending
                              select new
                              {
                                  Id = his.Id,
                                  IncidentId = inc.Id,
                                  Station = station.StationName,
                                  Post = post==null ? "---" : post.PostName,
                                  Title = his.Title,
                                  Description = his.Description,
                                  OpenDate = his.OpenDate,
                                  CloseDate = his.CloseDate,
                                  Autor = autor.Lastname,
                                  Status = status.Description,
                                  Respoinder = resp.Lastname,
                                  Comment = his.Comment,
                                  UpdatedAt = his.UpdatedAt
                              };
                return history.ToList();
                              
            }
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
