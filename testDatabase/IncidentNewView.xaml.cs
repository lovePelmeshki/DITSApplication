using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace testDatabase
{
    /// <summary>
    /// Interaction logic for IncidentNewView.xaml
    /// </summary>
    public partial class IncidentNewView : Window
    {
        private int _selectedLineId = -1;
        private int _selectedStationId = -1;
        private int _selectedPostId = -1;

        public IncidentNewView()
        {

            InitializeComponent();
            using (ditsdbContext db = new ditsdbContext())
            {
                //var employees = from emp in db.Employees
                //                select emp;
                //EmployeeComboBox.ItemsSource = employees.ToList();


                //EmployeeComboBox.ItemsSource = MainWindow.GetAllEmployees();
                //var status = from s in db.IncidentStatuses
                //             select s;
                //StatusComboBox.ItemsSource = status.ToList();
                //StatusComboBox.SelectedIndex = 0;

                RefreshItemsSourses();
            }
        }

        private void RefreshItemsSourses()
        {
            using (ditsdbContext db = new ditsdbContext())
            {

                EmployeeComboBox.ItemsSource = MainWindow.GetAllEmployees();
                StatusComboBox.ItemsSource = MainWindow.GetAllStatuses();
                StatusComboBox.SelectedIndex = 0;
                LineComboBox.ItemsSource = MainWindow.GetLines();

            }
        }


        private List<Station> FindStationsByLine(int lineId)
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                var stations = from station in db.Stations
                               where station.LineId == lineId
                               select station;
                return stations.ToList();
            }
        }

        private List<Post> FindPostsByStation(int stationId)
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                var posts = from post in db.Posts
                            where post.StationId == stationId
                            select post;
                return posts.ToList();
            }
        }

        #region Events


        //тут переделать запрос, добавить линии и станции
        private void CreateIncidentButton_Click(object sender, RoutedEventArgs e)
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                Incident incident = new Incident
                {
                    Title = TitleTextBox.Text,
                    Description = DescriptionTextBox.Text,
                    OpenDate = OpenDatePicker.SelectedDate,
                    EmployeeId = (int?)EmployeeComboBox.SelectedValue,
                    StatusId = (int?)StatusComboBox.SelectedValue,
                    StationId = (int?)StationComboBox.SelectedValue,
                    PostId = (int?)PostComboBox.SelectedValue // uzkoe mesto
                };
                db.Incidents.Add(incident);
                db.SaveChanges();
                Close();
            }

        }

        private void AutoFillButton_Click(object sender, RoutedEventArgs e)
        {

            TitleTextBox.Text = "Test title";
            DescriptionTextBox.Text = "Test description";
            OpenDatePicker.DisplayDate = DateTime.Now;
            EmployeeComboBox.SelectedIndex = 0;
            StatusComboBox.SelectedIndex = 0;

        }

        //Выбрать линию
        private void LineComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int lineId = (int)LineComboBox.SelectedValue;
            StationComboBox.ItemsSource = FindStationsByLine(lineId);
            PostComboBox.SelectedValue = null;
        }

        //Выбрать станцию
        private void StationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StationComboBox.SelectedValue != null)
            {
                int stationId = (int)StationComboBox.SelectedValue;
                PostComboBox.ItemsSource = FindPostsByStation(stationId);
            }

        }

        #endregion

    }
}
