﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using zajecia5.DOTs.Requests;
using zajecia5.DOTs.Responses;
using zajecia5.Services;

namespace zajecia5.Controllers
{
    [Route("api/promotions")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private IStudentDbService _service;

        public PromotionsController(IStudentDbService service)
        {
            _service = service;
        }
        [HttpPost]
        [Authorize(Roles ="employee")]
        public IActionResult PromoteStudent(PromoteStudentsRequest req)
        {
            var arePromoted = _service.PromoteStudents(req);
            if (!arePromoted) return StatusCode(500);
            var response = new PromoteStudentsResponse()
            {
                Semester = req.Semester+1,
                Studies = req.Studies
            };
            return Ok(response);
        }
    }
}