using System;
using System.Collections.Generic;

#nullable disable

namespace testDatabase
{
    public partial class Incident
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? OpenDate { get; set; }
        public int? EmployeeId { get; set; }
        public int? StatusId { get; set; }
        public int? ResponderId { get; set; }
        public DateTime? CloseDate { get; set; }
        public string Comment { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual IncidentStatus Responder { get; set; }
        public virtual IncidentStatus Status { get; set; }
    }
}
