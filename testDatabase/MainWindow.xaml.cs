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
        public Equipment _selectedEquipment = null;
        private int _selectedStatus = -1;
        public MainWindow()
        {
            InitializeComponent();
            RefreshIcintentsData();
            RefreshEmployeesData();
            RefreshStationsData();
            RefreshEquipmentData();
        }


        #region Incident

        public void RefreshIcintentsData()
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                var filter = from st in db.IncidentStatuses
                             select st;

                IncidentFilterComboBox.ItemsSource = filter.ToList();

                var incidentInfo = from inc in db.Incidents
                                   from emp in db.Employees

                                   where inc.EmployeeId == emp.Id

                                   join station in db.Stations
                                   on inc.StationId equals station.Id into stan
                                   from station in stan.DefaultIfEmpty()

                                   join post in db.Posts
                                   on inc.PostId equals post.Id into p
                                   from post in p.DefaultIfEmpty()

                                   join status in db.IncidentStatuses
                                   on inc.StatusId equals status.Id into st
                                   from status in st.DefaultIfEmpty()



                                   select new
                                   {
                                       Id = inc.Id,
                                       Title = inc.Title,
                                       Description = inc.Description,
                                       Employee = emp.Lastname,
                                       OpenDate = inc.OpenDate,
                                       CloseDate = inc.CloseDate,
                                       Status = status == null ? "---" : status.Description,
                                       StationName = station == null ? "---" : station.StationName,
                                       PostName = post == null ? "---" : post.PostName
                                   };

                icindentsDataGrid.ItemsSource = incidentInfo.ToList();
                //var allIcindents = from ic in db.Incidents
                //                   select ic;
                //icindentsDataGrid.ItemsSource = allIcindents.ToList();
            };
        }
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
        private void EditIcindentButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedIncident != null)
            {
                IncidentEditView window = new IncidentEditView(_selectedIncident);
                window.Show();
            }

        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshIcintentsData();
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
        private void IncidentFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IncidentFilterComboBox.SelectedValue != null)
            {
                _selectedStatus = (int)IncidentFilterComboBox.SelectedValue;
                using (ditsdbContext db = new ditsdbContext())
                {
                    var incidentInfo = from inc in db.Incidents
                                       from emp in db.Employees

                                       where inc.EmployeeId == emp.Id

                                       join station in db.Stations
                                       on inc.StationId equals station.Id into stan
                                       from station in stan.DefaultIfEmpty()

                                       join post in db.Posts
                                       on inc.PostId equals post.Id into p
                                       from post in p.DefaultIfEmpty()

                                       join status in db.IncidentStatuses
                                       on inc.StatusId equals status.Id
                                       where status.Id == _selectedStatus



                                       select new
                                       {
                                           Id = inc.Id,
                                           Title = inc.Title,
                                           Description = inc.Description,
                                           Employee = emp.Lastname,
                                           OpenDate = inc.OpenDate,
                                           CloseDate = inc.CloseDate,
                                           Status = status == null ? "---" : status.Description,
                                           StationName = station == null ? "---" : station.StationName,
                                           PostName = post == null ? "---" : post.PostName
                                       };
                    icindentsDataGrid.ItemsSource = incidentInfo.ToList();
                }
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
        public static Employee GetEmployee(int id)
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                var employee = (from emp in db.Employees
                                where emp.Id == id
                                select emp).FirstOrDefault();
                return employee;
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
        public static List<Line> GetLines()
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                var lines = from line in db.Lines select line;
                return lines.ToList();
            }
        }
        public static Line GetLine(int lineId)
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                var line = (from l in db.Lines
                            where l.Id == lineId
                            select l).FirstOrDefault();
                return line;

            }
        }
        public static Line GetLineByStationId(int stationId)
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                var line = (from l in db.Lines
                            join station in db.Stations
                            on l.Id equals station.LineId
                            select l).FirstOrDefault();
                return line;

            }
        }
        public static Station GetStation(int id)
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                var station = (from s in db.Stations
                               where s.Id == id
                               select s).FirstOrDefault();
                return station;
            }
        }
        public static List<Station> GetStationsByLine(int lineId)
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                var stations = from station in db.Stations
                               where station.LineId == lineId
                               select station;
                return stations.ToList();
            }
        }
        public static Post GetPost(int id)
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                var post = (from p in db.Posts
                            where p.Id == id
                            select p).FirstOrDefault();
                return post;
            }
        }
        public static List<Post> GetPostsByStation(int stationId)
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                var posts = from post in db.Posts
                            where post.StationId == stationId
                            select post;
                return posts.ToList();
            }
        }
        private void StationsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (StationsDataGrid.SelectedValue != null)
            {
                _selectedStation = GetStation((int)StationsDataGrid.SelectedValue);
                StationInfo window = new StationInfo(_selectedStation);
                window.Show();
            }

        }

        #endregion

        #region Equipment
        public void RefreshEquipmentData()
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                var equipment = from eq in db.Equipment
                                join types in db.EquipmentTypes
                                on eq.EqTypeId equals types.Id
                                
                                join clas in db.EquipmentClasses
                                on types.EquipmentClassId equals clas.Id

                                join post in db.Posts
                                on eq.PlaceId equals post.Id

                                join station in db.Stations
                                on post.StationId equals station.Id

                                join status in db.EquipmentStatuses
                                on eq.StatusId equals status.Id
                                select new
                                {
                                    Id = eq.Id,
                                    Class = clas.ClassName,
                                    types = types.TypeName,
                                    Serial = eq.Serial,
                                    Station = station.StationName,
                                    Point = post.PostName,
                                    Status = status.StatusName

                                };

                EquipmentDataGrid.ItemsSource = equipment.ToList();

            }

        }
        public Equipment GetEquipmentById(int id)
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                var equipment = (from eq in db.Equipment
                                where eq.Id == id
                                select eq).FirstOrDefault();
                return equipment;
            }
        }



        #region events


        private void EditEquipmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedEquipment != null)
            {
                var window = new EquipmentEditView(_selectedEquipment);
                window.Show();
            }
        }

        private void EquipmentDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_selectedEquipment != null)
            {
                var window = new EquipmentEditView(_selectedEquipment);
                window.Show();
            }
            //EquipmentDataGrid.SelectedValue;
        }

        #endregion

        #endregion

        private void EquipmentDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(EquipmentDataGrid.SelectedValue != null)
            {
            _selectedEquipment = GetEquipmentById((int)EquipmentDataGrid.SelectedValue);
            }

        }

        private void RefreshEquipmentButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshEquipmentData();
        }
    }
}
