using System;
using System.Collections.Generic;

#nullable disable

namespace testDatabase
{
    public partial class Maintenance
    {
        public Maintenance()
        {
            Equipment = new HashSet<Equipment>();
        }

        public int Id { get; set; }
        public int? MaintenanceTypeId { get; set; }
        public DateTime? MaintenanceDate { get; set; }
        public int? EquipmentId { get; set; }
        public int? EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Equipment EquipmentNavigation { get; set; }
        public virtual MaintenanceType MaintenanceType { get; set; }
        public virtual ICollection<Equipment> Equipment { get; set; }
    }
}
