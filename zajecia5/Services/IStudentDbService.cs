using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zajecia5.DOTs.Requests;
using zajecia5.Models;

namespace zajecia5.Services
{
    public interface IStudentDbService
    {
        IActionResult EnrollStudent(EnrollStudentRequest req);
        void PromoteStudents(string studies, int semester);
    }
}
