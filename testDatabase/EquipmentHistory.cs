using System;
using System.Collections.Generic;

#nullable disable

namespace testDatabase
{
    public partial class EquipmentHistory
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public int? PlaceId { get; set; }
        public int? StatusId { get; set; }
        public DateTime? InstallDate { get; set; }
        public int? LastMaintenanceId { get; set; }
        public DateTime? RepairDate { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
