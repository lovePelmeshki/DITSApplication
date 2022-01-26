using System;
using System.Collections.Generic;

#nullable disable

namespace testDatabase
{
    public partial class Line
    {
        public Line()
        {
            Stations = new HashSet<Station>();
        }

        public int Id { get; set; }
        public string LineName { get; set; }

        public virtual ICollection<Station> Stations { get; set; }
    }
}
