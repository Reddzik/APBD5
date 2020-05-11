using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using zajecia5.DOTs.Requests;
using zajecia5.DOTs.Responses;
using zajecia5.Services;


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
        public IActionResult GetStudent()
        {
            return Ok(_service.GetStudents());
        }
    }
}
