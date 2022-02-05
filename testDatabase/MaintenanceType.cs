using System;
using System.Collections.Generic;

#nullable disable

namespace testDatabase
{
    public partial class MaintenanceType
    {
        public MaintenanceType()
        {
            Maintenances = new HashSet<Maintenance>();
        }

        public int Id { get; set; }
        public string MaintenanceName { get; set; }
        public int? Periodicity { get; set; }

        public virtual ICollection<Maintenance> Maintenances { get; set; }
    }
}
