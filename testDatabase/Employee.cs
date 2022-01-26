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
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Patronymic { get; set; }
        public int? DepartmentId { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<Incident> Incidents { get; set; }
    }
}
