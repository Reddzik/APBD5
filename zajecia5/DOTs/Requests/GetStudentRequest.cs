using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace zajecia5.DOTs.Requests
{
    public class GetStudentRequest
    {
        [Required]
       public string IndexNumber { set; get; }
    }
}
