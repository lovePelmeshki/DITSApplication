using System.Windows;
using System.Linq;

namespace testDatabase
{
    /// <summary>
    /// Interaction logic for EquipmentEditView.xaml
    /// </summary>
    public partial class EquipmentEditView : Window
    {
        private Equipment _selectedEquipment = null;
        public EquipmentEditView(Equipment eqipment)
        {
            InitializeComponent();
            _selectedEquipment = eqipment;
            Refresh();
        }

        private void Refresh()
        {
 

            using (ditsdbContext db = new ditsdbContext())
            {
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
                             select new
                             {
                                 Id = eq.Id,
                                 Serial = eq.Serial,
                                 TypeName = eqType.TypeName,
                                 LineName = line.LineName,
                                 StationName = station.StationName,
                                 PostName = post.PostName,
                                 StatusName = status.StatusName

                             };
                DataContext = eqInfo.ToList();
                switch (_selectedEquipment.StatusId)
                {
                    case 1: //Установлено
                        InstallButton.Visibility = Visibility.Collapsed;
                        MoveFromRepairButton.Visibility = Visibility.Collapsed;
                        break;
                    case 2: //Готово к работе
                        DoMaintenanceButton.Visibility = Visibility.Collapsed;
                        ChangeButton.Visibility = Visibility.Collapsed;
                        MoveFromRepairButton.Visibility = Visibility.Collapsed;
                        DropButton.Visibility = Visibility.Collapsed;

                        break;
                    case 3: //Неисправно
                        DoMaintenanceButton.Visibility = Visibility.Collapsed;
                        ChangeButton.Visibility = Visibility.Collapsed;
                        InstallButton.Visibility = Visibility.Collapsed;
                        MoveFromRepairButton.Visibility = Visibility.Collapsed;
                        DropButton.Visibility = Visibility.Collapsed;
                        break;
                    case 4: //В ремонте
                        MoveToRepairButton.Visibility = Visibility.Collapsed;
                        DoMaintenanceButton.Visibility = Visibility.Collapsed;
                        ChangeButton.Visibility = Visibility.Collapsed;
                        InstallButton.Visibility = Visibility.Collapsed;
                        DropButton.Visibility = Visibility.Collapsed;
                        MoveFromRepairButton.Visibility = Visibility.Visible;
                        break;

                }
            }

        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
           
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
                db.SaveChanges();
                _selectedEquipment = equp;
                Refresh();
            }
        }
    }
}
