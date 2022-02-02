using System;
using System.Collections.Generic;

#nullable disable

namespace testDatabase
{
    public partial class EquipmentType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public int? EquipmentClassId { get; set; }

        public virtual EquipmentClass EquipmentClass { get; set; }
    }
}
