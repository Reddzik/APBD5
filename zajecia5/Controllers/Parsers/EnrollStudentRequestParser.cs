
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using zajecia5.DOTs.Requests;
using zajecia5.Models;

namespace zajecia5.Controllers.Parsers
{
    public class EnrollStudentRequestParser
    {
        public static Student ParseFromReqToStudent(EnrollStudentRequest req)
        {
            var student = new Student();
            student.IndexNumber = req.IndexNumber;
            student.FirstName = req.FirstName;
            student.LastName = req.LastName;
            student.Birthdate = DateTime.Parse(req.Birthdate);
            student.Studies = req.Studies;
            return student;
        }
    }
}
