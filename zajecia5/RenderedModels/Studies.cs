using System;
using System.Collections.Generic;

namespace zajecia5.RenderedModels
{
    public partial class Studies
    {
        public Studies()
        {
            Enrollments = new HashSet<Enrollment>();
        }

        public int IdStudy { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
