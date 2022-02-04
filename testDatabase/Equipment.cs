using System;
using System.Collections.Generic;

#nullable disable

namespace testDatabase
{
    public partial class Equipment
    {
        public int Id { get; set; }
        public string Serial { get; set; }
        public int? EqTypeId { get; set; }
        public int? PlaceId { get; set; }
        public int? StatusId { get; set; }

        public virtual EquipmentType EqType { get; set; }
        public virtual Post Place { get; set; }
        public virtual EquipmentStatus Status { get; set; }
    }
}
