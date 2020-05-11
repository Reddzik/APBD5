using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zajecia5.DOTs.Requests
{
    public class ModifyStudentDataRequest
    {
        public string IndexNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int IdEnrollment { get; set; }
        public string Password { get; set; }
        public string RefreshToken { get; set; }
    }
}
