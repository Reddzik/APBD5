using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace zajecia5.DOTs.Requests
{
    public class PromoteStudentsRequest
    {
        [Required]
        public string Studies { get; set; }
        [Required]
        public int Semester { get; set; }
    }
}
