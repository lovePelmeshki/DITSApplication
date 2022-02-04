using System;
using System.Collections.Generic;

#nullable disable

namespace testDatabase
{
    public partial class EquipmentStatus
    {
        public EquipmentStatus()
        {
            Equipment = new HashSet<Equipment>();
        }

        public int Id { get; set; }
        public string StatusName { get; set; }

        public virtual ICollection<Equipment> Equipment { get; set; }
    }
}
