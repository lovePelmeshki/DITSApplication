using System;
using System.Collections.Generic;

#nullable disable

namespace testDatabase
{
    public partial class Employee
    {
        public Employee()
        {
            Incidents = new HashSet<Incident>();
        }

        public int Id { get; set; }
        public string Firstname { get; set; }

        public virtual ICollection<Incident> Incidents { get; set; }
    }
}
