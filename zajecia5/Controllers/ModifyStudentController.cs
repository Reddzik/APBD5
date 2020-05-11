using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using zajecia5.DOTs.Requests;
using zajecia5.Services;

namespace zajecia5.Controllers
{
    [Route("api/modify")]
    [ApiController]
    public class ModifyStudentController : ControllerBase
    {

        private readonly IStudentDbService _service; 
        public ModifyStudentController(IStudentDbService service)
        {
            this._service = service;
        }
        [HttpPost("student")]
        public IActionResult ModifyStudent(ModifyStudentDataRequest req)
        {
            _service.modifyStudentData(req);
            return Ok();
        }
    }
}