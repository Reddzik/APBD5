using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using zajecia5.Services;

namespace zajecia5.Controllers
{
    [Route("api/delete")]
    [ApiController]
    public class DeleteStudentFromDbController : ControllerBase
    {
        private readonly IStudentDbService _service;
        public DeleteStudentFromDbController(IStudentDbService service)
        {
            this._service = service;
        }
        [HttpPost("student/{index}")]
        public IActionResult DeleteStudent(string index)
        {
            _service.deleteStudent(index);
            return Ok();
        }
    }
}