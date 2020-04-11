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
        public Student EnrollStudent(EnrollStudentRequest req)
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
                    return null;
                }
            }
            Console.WriteLine("Udało się dodać studenta!");
            return newStudent;
        }

        public Boolean PromoteStudents(PromoteStudentsRequest req)
        {
            using(var connection = new SqlConnection())
            using (var command = new SqlCommand())
            {
                connection.ConnectionString = _ConnectionString;
                command.Connection = connection;
                connection.Open();
                var transaction = connection.BeginTransaction();
                command.Transaction = transaction;
                try {
                    command.CommandText = "Exec PromoteStudents @semester, @studiesName;";
                    command.Parameters.AddWithValue("semester", req.Semester);
                    command.Parameters.AddWithValue("studiesName", req.Studies);
                }
                catch(SqlException ex)
                {
                    Console.WriteLine(ex);
                    transaction.Rollback();
                    return false;
                }
                transaction.Commit();
            }
            return true;
        }

        public Student GetStudent(string index)
        {
            using(var connection = new SqlConnection(_ConnectionString))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                connection.Open();
                try
                {
                    command.CommandText = "Exec getStudent @index";
                    command.Parameters.AddWithValue("index", index);
                    var dataReaded = command.ExecuteReader();
                    var newStudent = new Student();
                    while (!dataReaded.Read())
                    {
                        newStudent.FirstName = dataReaded["FirstName"].ToString();
                        newStudent.LastName = dataReaded["LastName"].ToString();
                        newStudent.Studies = dataReaded["Name"].ToString();
                        newStudent.Semester = (int)dataReaded["Semester"];
                    }
                    return newStudent;

                }
                catch(SqlException ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
            }

        }
        public Boolean IsThereStudent(string index)
        {
            using (var connection = new SqlConnection(_ConnectionString))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                connection.Open();
                try
                {
                    command.CommandText = "exec findStudent @index;";
                    command.Parameters.AddWithValue("index", index);
                    var reader = command.ExecuteReader();
                    return reader.Read();
                }catch(SqlException ex)
                {
                    Console.WriteLine(ex);
                    return false;
                }
            }
        }
    }
}
