using System;
using System.Collections.Generic;

#nullable disable

namespace testDatabase
{
    public partial class Equipment
    {
        public Equipment()
        {
            Maintenances = new HashSet<Maintenance>();
        }

        public int Id { get; set; }
        public string Serial { get; set; }
        public int? EqTypeId { get; set; }
        public int? PlaceId { get; set; }
        public int? StatusId { get; set; }
        public DateTime? InstallDate { get; set; }
        public int? LastMaintenanceId { get; set; }
        public DateTime? RepairDate { get; set; }

        public virtual EquipmentType EqType { get; set; }
        public virtual Maintenance LastMaintenance { get; set; }
        public virtual Post Place { get; set; }
        public virtual EquipmentStatus Status { get; set; }
        public virtual ICollection<Maintenance> Maintenances { get; set; }
    }
}
