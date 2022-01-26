using System;
using System.Collections.Generic;

#nullable disable

namespace testDatabase
{
    public partial class Station
    {
        public Station()
        {
            Incidents = new HashSet<Incident>();
            Posts = new HashSet<Post>();
        }

        public int Id { get; set; }
        public int LineId { get; set; }
        public string StationName { get; set; }

        public virtual Line Line { get; set; }
        public virtual ICollection<Incident> Incidents { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
