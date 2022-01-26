using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace testDatabase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Incident _selectedIncident = null;
        public Station _selectedStation = null;
        public MainWindow()
        {
            InitializeComponent();
            RefreshIcintentsData();
            RefreshEmployeesData();
            RefreshStationsData();
        }


        #region Incident

        private void icindentsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if (_selectedIncident != null)
            {
                IncidentEditView window = new IncidentEditView(_selectedIncident);
                window.Show();
            }
        }

        private void NewIcintentButton_Click(object sender, RoutedEventArgs e)
        {
            IncidentNewView view = new IncidentNewView();
            view.Show();
        }

        public void RefreshIcintentsData()
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                var incidentInfo = from inc in db.Incidents
                                   from emp in db.Employees
                                   
                                   where inc.EmployeeId == emp.Id
                                   join status in db.IncidentStatuses
                                   on inc.StatusId equals status.Id into st
                                   from status in st.DefaultIfEmpty()
                                   select new
                                   {
                                       Id = inc.Id,
                                       Title = inc.Title,
                                       Description = inc.Description,
                                       Firstname = emp.Firstname,
                                       Status = status == null ? "---" : status.Description
                                   };

                icindentsDataGrid.ItemsSource = incidentInfo.ToList();
                //var allIcindents = from ic in db.Incidents
                //                   select ic;
                //icindentsDataGrid.ItemsSource = allIcindents.ToList();
            };
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshIcintentsData();
        }

        private void EditIcindentButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedIncident != null)
            {
                IncidentEditView window = new IncidentEditView(_selectedIncident);
                window.Show();
            }

        }

        private void icindentsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            
            if (icindentsDataGrid.SelectedValue != null)
            {
                _selectedIncident = GetIncidentById((int)icindentsDataGrid.SelectedValue);
            }

            //_selectedIncident = icindentsDataGrid.SelectedItem as Incident;
            if (_selectedIncident == null)
            {
                IdTextBlock.Text = "Empty";
            }
            else
            {
                IdTextBlock.Text = _selectedIncident.Id.ToString();
            }





        }
        public static List<IncidentStatus> GetAllStatuses()
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                var status = from st in db.IncidentStatuses
                             select st;
                return status.ToList();
            }
        }

        public static Incident GetIncidentById(int id)
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                var incident = (from inc in db.Incidents
                                where inc.Id == id
                                select inc).FirstOrDefault();
                return incident;
            }
        }

        public static List<IncidentHistory> GetIncidentHistoryById(int incidentId)
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                var history = from h in db.IncidentHistories
                              where h.IncidentId == incidentId
                              select h;
                return history.ToList();
            }
        }

        #endregion

        #region Employees
        public void RefreshEmployeesData()
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                var employeeInfo = from emp in db.Employees
                                   join dep in db.Departments
                                   on emp.DepartmentId equals dep.Id
                                   select new
                                   {
                                       Id = emp.Id,
                                       Lastname = emp.Lastname,
                                       Firstname = emp.Firstname,
                                       Patronymic = emp.Patronymic,
                                       DepartmentName = dep.DepartmentName
                                   };
                EmployeeDataGrid.ItemsSource = employeeInfo.ToList();
            }
            
        }
        public static List<Employee> GetAllEmployees()
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                var employees = from emp in db.Employees
                                select emp;
                return employees.ToList();
            }
        }

        #endregion

        #region Stations
        public void RefreshStationsData()
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                var stationInfo = from s in db.Stations
                                  join l in db.Lines
                                  on s.LineId equals l.Id
                                  select new
                                  {
                                      Id = s.Id,
                                      LineName = l.LineName,
                                      StationName = s.StationName,
                                  };
                StationsDataGrid.ItemsSource = stationInfo.ToList();

            }
        }
        public Station FindStationById(int id)
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                var station = (from s in db.Stations
                               where s.Id == id
                               select s).FirstOrDefault();
                return station;
            }
        }
        private void StationsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (StationsDataGrid.SelectedValue != null)
            {
                _selectedStation = FindStationById((int)StationsDataGrid.SelectedValue);
                StationInfo window = new StationInfo(_selectedStation);
                window.Show();
            }
            
        }

        #endregion


    }
}
