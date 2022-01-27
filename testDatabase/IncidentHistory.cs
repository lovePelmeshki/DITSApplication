using System;
using System.Collections.Generic;

#nullable disable

namespace testDatabase
{
    public partial class IncidentHistory
    {
        public int Id { get; set; }
        public int IncidentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? OpenDate { get; set; }
        public int? AutorId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? StatusId { get; set; }
        public int? StationId { get; set; }
        public int? PostId { get; set; }
        public DateTime? CloseDate { get; set; }
        public int? RespoinderId { get; set; }
        public string Comment { get; set; }
    }
}
