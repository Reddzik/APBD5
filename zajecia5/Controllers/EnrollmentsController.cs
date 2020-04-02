using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using zajecia5.DOTs.Requests;
using zajecia5.Models;

namespace zajecia5.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest req)
        {
            var newStudent = ParseDataFromReqToStudent(req);
            using(var connection = new SqlConnection(""))
            using(var command = new SqlCommand())
            {
                command.Connection = connection;
                connection.Open();
                try {
                    command.CommandText= "exec EnrollStudent @Studies, @IndexNumber, @FirstName, @LastName, @BirthDate;";
                    command.Parameters.AddWithValue("Studies", newStudent.Studies);
                    command.Parameters.AddWithValue("IndexNumber", newStudent.IndexNumber);
                    command.Parameters.AddWithValue("FirstName", newStudent.FirstName);
                    command.Parameters.AddWithValue("LastName", newStudent.LastName);
                    command.Parameters.AddWithValue("BirthDate", newStudent.Birthdate);

                    command.ExecuteNonQuery();

                }catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    return BadRequest();
                }
                return Ok();

            }
            return Ok();
        }
        private Student ParseDataFromReqToStudent(EnrollStudentRequest req)
        {
            var student = new Student();
            student.IndexNumber = req.IndexNumber;
            student.FirstName = req.FirstName;
            student.LastName = req.LastName;
            student.Birthdate = req.Birthdate;
            student.Studies = req.Studies;
            return student;
        }
    }
}