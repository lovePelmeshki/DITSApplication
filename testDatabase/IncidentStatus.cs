using System;
using System.Collections.Generic;

#nullable disable

namespace testDatabase
{
    public partial class IncidentStatus
    {
        public IncidentStatus()
        {
            IncidentResponders = new HashSet<Incident>();
            IncidentStatuses = new HashSet<Incident>();
        }

        public int Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Incident> IncidentResponders { get; set; }
        public virtual ICollection<Incident> IncidentStatuses { get; set; }
    }
}
