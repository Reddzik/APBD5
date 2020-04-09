using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using zajecia5.Controllers.Parsers;
using zajecia5.DOTs.Requests;
using zajecia5.Models;

namespace zajecia5.Services
{
    public class SqlServerStudentDbService : IStudentDbService
    {
        private const string _ConnectionString = "Data Source=db-mssql;Initial Catalog=s18819;Integrated Security=True";
        public IActionResult EnrollStudent(EnrollStudentRequest req)
        {
            var newStudent = EnrollStudentRequestParser.ParseFromReqToStudent(req);
            using (var connection = new SqlConnection())
            using (var command = new SqlCommand())
            {
                connection.ConnectionString = _ConnectionString;
                command.Connection = connection;
                connection.Open();
                try
                {
                    command.CommandText = "Exec EnrollStudent @Studies, @IndexNumber, @FirstName, @LastName, @BirthDate;";
                    command.Parameters.AddWithValue("Studies", newStudent.Studies);
                    command.Parameters.AddWithValue("IndexNumber", newStudent.IndexNumber);
                    command.Parameters.AddWithValue("FirstName", newStudent.FirstName);
                    command.Parameters.AddWithValue("LastName", newStudent.LastName);
                    command.Parameters.AddWithValue("BirthDate", newStudent.Birthdate);
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                    return new BadRequestResult();
                }
            }
            Console.WriteLine("Udało się dodać studenta!");
            return new AcceptedResult();
        }

        public void PromoteStudents(string studies, int semester)
        {
            throw new NotImplementedException();
        }
    }
}
