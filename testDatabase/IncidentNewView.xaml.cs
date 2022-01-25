using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Linq;
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
    /// Interaction logic for IncidentNewView.xaml
    /// </summary>
    public partial class IncidentNewView : Window
    {
        
        public IncidentNewView()
        {
            
            InitializeComponent();
            using (ditsdbContext db = new ditsdbContext())
            {
                //var employees = from emp in db.Employees
                //                select emp;
                //EmployeeComboBox.ItemsSource = employees.ToList();
                EmployeeComboBox.ItemsSource = MainWindow.GetAllEmployees(); 
                var status = from s in db.IncidentStatuses
                             select s;
                StatusComboBox.ItemsSource = status.ToList();
                StatusComboBox.SelectedIndex = 0;
            }
        }

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
                    StatusId = (int?)StatusComboBox.SelectedValue
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
    }
}
