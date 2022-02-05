using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace testDatabase
{
    /// <summary>
    /// Interaction logic for EquipmentEditView.xaml
    /// </summary>
    public partial class EquipmentEditView : Window
    {
        int counter = 0;
        private Equipment _selectedEquipment = null;
        private int _selectedLine = -1;
        private int _selectedStation = -1;
        private int _selectedPost = -1;
        public EquipmentEditView(Equipment eqipment)
        {
            InitializeComponent();
            _selectedEquipment = eqipment;
            Refresh();
        }

        private void Refresh()
        {

            counter = 0;
            using (ditsdbContext db = new ditsdbContext())
            {
                DateTime? dt = DateTime.MinValue;
                DateTime nextMaintenanceDate = DateTime.MinValue;
                int periodicity = 0;
                var eqInfo = from eq in db.Equipment
                             where eq.Id == _selectedEquipment.Id

                             join eqType in db.EquipmentTypes
                             on eq.EqTypeId equals eqType.Id

                             join status in db.EquipmentStatuses
                             on eq.StatusId equals status.Id

                             join post in db.Posts
                             on eq.PlaceId equals post.Id

                             join station in db.Stations
                             on post.StationId equals station.Id

                             join line in db.Lines
                             on station.LineId equals line.Id

                             join m in db.Maintenances
                             on eq.LastMaintenanceId equals m.Id

                             join mt in db.MaintenanceTypes
                             on m.MaintenanceTypeId equals mt.Id

                             join emp in db.Employees
                             on m.EmployeeId equals emp.Id
                             select new
                             {
                                 Id = eq.Id,
                                 Serial = eq.Serial,
                                 TypeName = eqType.TypeName,
                                 LineName = line.LineName,
                                 StationName = station.StationName,
                                 PostName = post.PostName,
                                 StatusName = status.StatusName,
                                 InstallDate = eq.InstallDate,
                                 MaintenanceDate = m.MaintenanceDate,
                                 NextDate = Convert.ToDateTime(m.MaintenanceDate).AddDays((int)mt.Periodicity),
                                 Employee = emp.Lastname, 
                             };
                DataContext = eqInfo.ToList();

                //Maintenance maintenance = (from m in db.Maintenances
                //           where m.Id == _selectedEquipment.LastMaintenanceId
                //           select m).FirstOrDefault();
                //MaintenanceType mtype = (from t in db.MaintenanceTypes
                //                        where t.Id == maintenance.MaintenanceTypeId
                //                        select t).FirstOrDefault();
                //dt = maintenance.MaintenanceDate;
                //periodicity = (int)mtype.Periodicity;
                //nextMaintenanceDate = Convert.ToDateTime(dt).AddDays(periodicity);
                //string nmdt = nextMaintenanceDate.ToString();
                ////NextMaintenanceDateTextBox.Text = nextMaintenanceDate.ToString();

                
                

                switch (_selectedEquipment.StatusId)
                {
                    case 1: //Установлено
                        MoveToRepairButton.Visibility = Visibility.Visible;
                        MaintenanceButton.Visibility = Visibility.Visible;
                        ChangeButton.Visibility = Visibility.Visible;
                        DropButton.Visibility = Visibility.Visible;
                        InstallButton.Visibility = Visibility.Collapsed;
                        MoveFromRepairButton.Visibility = Visibility.Collapsed;

                        ChangeStackPanel.Visibility = Visibility.Collapsed;
                        InstallStackPanel.Visibility = Visibility.Collapsed;
                        MaintenanceStackPanel.Visibility = Visibility.Collapsed;
                        break;
                    case 2: //Готово к работе
                        MoveToRepairButton.Visibility = Visibility.Visible;
                        MaintenanceButton.Visibility = Visibility.Collapsed;
                        ChangeButton.Visibility = Visibility.Collapsed;
                        InstallButton.Visibility = Visibility.Visible;
                        DropButton.Visibility = Visibility.Collapsed;
                        MoveFromRepairButton.Visibility = Visibility.Collapsed;

                        ChangeStackPanel.Visibility = Visibility.Collapsed;
                        InstallStackPanel.Visibility = Visibility.Collapsed;
                        MaintenanceStackPanel.Visibility = Visibility.Collapsed;


                        break;
                    case 3: //Неисправно
                        MoveToRepairButton.Visibility = Visibility.Visible;
                        MaintenanceButton.Visibility = Visibility.Collapsed;
                        ChangeButton.Visibility = Visibility.Collapsed;
                        InstallButton.Visibility = Visibility.Collapsed;
                        MoveFromRepairButton.Visibility = Visibility.Collapsed;
                        DropButton.Visibility = Visibility.Collapsed;

                        ChangeStackPanel.Visibility = Visibility.Collapsed;
                        MaintenanceStackPanel.Visibility = Visibility.Collapsed;
                        InstallStackPanel.Visibility = Visibility.Collapsed;
                        break;
                    case 4: //В ремонте
                        MoveToRepairButton.Visibility = Visibility.Collapsed;
                        MaintenanceButton.Visibility = Visibility.Collapsed;
                        ChangeButton.Visibility = Visibility.Collapsed;
                        InstallButton.Visibility = Visibility.Collapsed;
                        DropButton.Visibility = Visibility.Collapsed;
                        MoveFromRepairButton.Visibility = Visibility.Visible;

                        ChangeStackPanel.Visibility = Visibility.Collapsed;
                        InstallStackPanel.Visibility = Visibility.Collapsed;
                        MaintenanceStackPanel.Visibility = Visibility.Collapsed;
                        break;

                }
            }

        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            counter++;
            switch (counter % 2)
            {
                case 0:
                    ChangeStackPanel.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    ChangeStackPanel.Visibility = Visibility.Visible;
                    break;
            }
            using (ditsdbContext db = new ditsdbContext())
            {
                var equip = from eq in db.Equipment
                            where (eq.EqTypeId == _selectedEquipment.EqTypeId && eq.StatusId == 2)
                            select eq;
                ChangeEqComboBox.ItemsSource = equip.ToList();
            }

        }
        private void DoChangeEqButton_Click(object sender, RoutedEventArgs e)
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                if (ChangeEqComboBox.SelectedValue != null)
                {
                    int newEquipmentId = (int)ChangeEqComboBox.SelectedValue;

                    Equipment mainEquipment = (from eq in db.Equipment
                                   where eq.Id == _selectedEquipment.Id
                                   select eq).FirstOrDefault();
                    Equipment newEquipment = (from eq in db.Equipment
                                              where eq.Id == newEquipmentId
                                              select eq).FirstOrDefault();
                    newEquipment.PlaceId = mainEquipment.PlaceId;
                    newEquipment.StatusId = mainEquipment.StatusId;
                    newEquipment.InstallDate = DateTime.Now;

                    mainEquipment.PlaceId = 66;
                    mainEquipment.StatusId = 3;
                    mainEquipment.InstallDate = DateTime.Now;
                    db.SaveChanges();
                    _selectedEquipment = mainEquipment;
                    counter = 0;
                    ChangeStackPanel.Visibility = Visibility.Collapsed;
                    ChangeEqComboBox.ItemsSource = null;
                    Refresh();


                }

            }
        }

        private void MoveToRepairButton_Click(object sender, RoutedEventArgs e)
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                Equipment equp = (from eq in db.Equipment
                                  where eq.Id == _selectedEquipment.Id
                                  select eq).FirstOrDefault();
                equp.PlaceId = 67;
                equp.StatusId = 4;
                equp.InstallDate = DateTime.Now;
                db.SaveChanges();
                _selectedEquipment = equp;
                Refresh();
            }
        }

        private void MoveFromRepairButton_Click(object sender, RoutedEventArgs e)
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                Equipment equip = (from eq in db.Equipment
                                   where eq.Id == _selectedEquipment.Id
                                   select eq).FirstOrDefault();
                equip.PlaceId = 65;
                equip.StatusId = 2;
                equip.InstallDate = DateTime.Now;
                db.SaveChanges();
                _selectedEquipment = equip;
                Refresh();
            }
        }

        private void ChangeLineComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ChangeLineComboBox.SelectedValue != null)
            {
                _selectedLine = (int)ChangeLineComboBox.SelectedValue;
                ChangeStationComboBox.ItemsSource = MainWindow.GetStationsByLine(_selectedLine);
                ChangePostComboBox.ItemsSource = null;
            }

        }

        private void ChangeStationComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ChangeStationComboBox.SelectedValue != null)
            {
                _selectedStation = (int)ChangeStationComboBox.SelectedValue;
                ChangePostComboBox.ItemsSource = MainWindow.GetPostsByStation(_selectedStation);
            }
        }

        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            counter++;
            switch (counter % 2)
            {
                case 0:
                    InstallStackPanel.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    InstallStackPanel.Visibility = Visibility.Visible;
                    break;
            }
            ChangeLineComboBox.ItemsSource = MainWindow.GetLines();
        }

        private void DoChangeButton_Click(object sender, RoutedEventArgs e)
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                Equipment equip = (from eq in db.Equipment
                                   where eq.Id == _selectedEquipment.Id
                                   select eq).FirstOrDefault();
                if (ChangeLineComboBox.SelectedValue != null &&
                    ChangeStationComboBox.SelectedValue != null &&
                    ChangePostComboBox.SelectedValue != null)
                {
                    equip.PlaceId = (int)ChangePostComboBox.SelectedValue;
                    equip.InstallDate = DateTime.Now;
                    equip.StatusId = 1;
                    db.SaveChanges();
                    _selectedEquipment = equip;
                    Refresh();

                    //Change Stack Panel
                    counter = 0;
                    InstallStackPanel.Visibility = Visibility.Collapsed;
                    ChangeStationComboBox.ItemsSource = null;
                    ChangePostComboBox.ItemsSource = null;
                }
            }
        }

        private void DropButton_Click(object sender, RoutedEventArgs e)
        {
            using (ditsdbContext db = new ditsdbContext())
            {
                Equipment equip = (from eq in db.Equipment
                                  where eq.Id == _selectedEquipment.Id
                                  select eq).FirstOrDefault();
                equip.PlaceId = 66;
                equip.StatusId = 3;
                equip.InstallDate = DateTime.Now;
                db.SaveChanges();
                _selectedEquipment = equip;
                Refresh();
            }
        }

        private void MaintenanceButton_Click(object sender, RoutedEventArgs e)
        {
            counter++;
            switch (counter % 2)
            {
                case 0:
                    MaintenanceStackPanel.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    MaintenanceStackPanel.Visibility = Visibility.Visible;
                    break;
            }
            using (ditsdbContext db = new ditsdbContext())
            {
                EmployeeComboBox.ItemsSource = MainWindow.GetAllEmployees();
            }

        }

        private void DoMaintenanceButton_Click(object sender, RoutedEventArgs e)
        {
            if (MaintenanceDatePicker.SelectedDate != null && EmployeeComboBox.SelectedValue != null)
            {
                using (ditsdbContext db = new ditsdbContext())
                {
                    Maintenance maintenance = new Maintenance
                    {
                        MaintenanceTypeId = 1,
                        MaintenanceDate = MaintenanceDatePicker.SelectedDate,
                        EquipmentId = _selectedEquipment.Id,
                        EmployeeId = (int)EmployeeComboBox.SelectedValue
                    };
                    db.Maintenances.Add(maintenance);
                    db.SaveChanges();

                    Equipment equipment = (from eq in db.Equipment
                                           where eq.Id == _selectedEquipment.Id
                                           select eq).FirstOrDefault();
                    equipment.LastMaintenanceId = maintenance.Id;
                    db.SaveChanges();
                    Refresh();

                }
            }
        }

        private void TodayButton_Click(object sender, RoutedEventArgs e)
        {
            MaintenanceDatePicker.SelectedDate = DateTime.Now;
        }
    }

}
