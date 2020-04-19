using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using zajecia5.DOTs.Requests;
using zajecia5.DOTs.Responses;
using zajecia5.Models;
using zajecia5.Services;

namespace zajecia5.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private IStudentDbService _service; 
        public EnrollmentsController(IStudentDbService service)
        {
            _service = service;
        }
        [HttpPost]
        [Authorize(Roles = "employee")]
        public IActionResult EnrollStudent(EnrollStudentRequest req)
        {
            _service.EnrollStudent(req);
            var response = new EnrollStudentResponse();
            response.LastName = req.LastName;
            response.Semester = 1;
            response.StartDate = DateTime.Now;
            return CreatedAtAction("EnrollStudent", response);

        }
    }
}