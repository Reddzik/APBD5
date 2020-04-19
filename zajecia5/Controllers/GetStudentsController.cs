using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using zajecia5.DOTs.Requests;
using zajecia5.DOTs.Responses;
using zajecia5.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace zajecia5.Controllers
{
    [Route("api/GetStudents")]
    [ApiController]
    public class GetStudentsController : ControllerBase
    {
        private IStudentDbService _service;

        public GetStudentsController(IStudentDbService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetStudent(GetStudentRequest req)
        {
            var student = _service.GetStudent(req.IndexNumber);
            if (student == null) return StatusCode(500);
            var response = new GetStudentResponse()
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                Studies = student.Studies,
                Semester = student.Semester
            };
            return Ok(response);
        }
    }
}
