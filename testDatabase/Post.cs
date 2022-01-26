﻿using System;
using System.Collections.Generic;

#nullable disable

namespace testDatabase
{
    public partial class Post
    {
        public Post()
        {
            Incidents = new HashSet<Incident>();
        }

        public int Id { get; set; }
        public string PostName { get; set; }
        public int? StationId { get; set; }

        public virtual Station Station { get; set; }
        public virtual ICollection<Incident> Incidents { get; set; }
    }
}
