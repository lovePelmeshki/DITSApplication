using System;
using System.Collections.Generic;

#nullable disable

namespace testDatabase
{
    public partial class Station
    {
        public int Id { get; set; }
        public int LineId { get; set; }
        public string StationName { get; set; }

        public virtual Line Line { get; set; }
    }
}
