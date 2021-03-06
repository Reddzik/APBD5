﻿using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using zajecia5.Controllers.Parsers;
using zajecia5.DOTs.Requests;
using zajecia5.RenderedModels;

namespace zajecia5.Services
{
    public class SqlServerStudentDbService : IStudentDbService
    {
        private const string _ConnectionString = "Data Source=db-mssql;Initial Catalog=s18819;Integrated Security=True";
        private readonly s18819Context _context;
        public SqlServerStudentDbService(s18819Context context)
        {
            this._context = context;
        }
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
                    command.Parameters.AddWithValue("BirthDate", newStudent.BirthDate);
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
            using (var connection = new SqlConnection())
            using (var command = new SqlCommand())
            {
                connection.ConnectionString = _ConnectionString;
                command.Connection = connection;
                connection.Open();
                var transaction = connection.BeginTransaction();
                command.Transaction = transaction;
                try
                {
                    command.CommandText = "Exec PromoteStudents @semester, @studiesName;";
                    command.Parameters.AddWithValue("semester", req.Semester);
                    command.Parameters.AddWithValue("studiesName", req.Studies);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                    transaction.Rollback();
                    return false;
                }
                transaction.Commit();
                return true;
            }
        }

        public Student GetStudent(string index)
        {
            using (var connection = new SqlConnection(_ConnectionString))
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
                        newStudent.IndexNumber = dataReaded["IndexNumber"].ToString();
                        newStudent.FirstName = dataReaded["FirstName"].ToString();
                        newStudent.LastName = dataReaded["LastName"].ToString();
                        newStudent.IdEnrollment = (int)dataReaded["Semester"];
                    }
                    return newStudent;

                }
                catch (SqlException ex)
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
                    command.CommandText = "Select 1 from student where IndexNumber = @index";
                    command.Parameters.AddWithValue("index", index);
                    var reader = command.ExecuteReader();
                    return reader.Read();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                    return false;
                }
            }
        }

        public bool CheckCredential(string user, string password)
        {
            using (var connection = new SqlConnection(_ConnectionString))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                connection.Open();
                try
                {
                    command.CommandText = "Select 1 from student where IndexNumber = @user and password = @password; ";
                    command.Parameters.AddWithValue("user", user);
                    command.Parameters.AddWithValue("password", password);
                    return command.ExecuteReader().Read();

                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        public Student GetUserByRefreshToken(string token)
        {
            using (var connection = new SqlConnection(_ConnectionString))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                connection.Open();
                try
                {
                    command.CommandText = "Select * from Student where RefreshToken = @token";
                    command.Parameters.AddWithValue("token", token);
                    var dataReaded = command.ExecuteReader();
                    var newStudent = new Student();
                    if (dataReaded.Read())
                    {
                        newStudent.IndexNumber = dataReaded["IndexNumber"].ToString();
                        newStudent.FirstName = dataReaded["FirstName"].ToString();
                        newStudent.LastName = dataReaded["LastName"].ToString();
                    }
                    return newStudent;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
            }
        }

        public Student GetLoggedStudent(string login, string password)
        {
            using (var connection = new SqlConnection(_ConnectionString))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                connection.Open();
                try
                {
                    command.CommandText = "Select * from Student where IndexNumber = @login and password = @password";
                    command.Parameters.AddWithValue("login", login);
                    command.Parameters.AddWithValue("password", password);
                    var dataReaded = command.ExecuteReader();
                    var newStudent = new Student();
                    if (dataReaded.Read())
                    {
                        newStudent.IndexNumber = dataReaded["IndexNumber"].ToString();
                        newStudent.FirstName = dataReaded["FirstName"].ToString();
                        newStudent.LastName = dataReaded["LastName"].ToString();
                    }
                    return newStudent;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
            }

        }
        public Boolean AddRefreshTokenToUser(string token, string index)
        {
            using (var connection = new SqlConnection(_ConnectionString))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                connection.Open();
                try
                {
                    command.CommandText = "exec addRefreshToken @token, @index";
                    command.Parameters.AddWithValue("token", token);
                    command.Parameters.AddWithValue("index", index);
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                    return false;
                }
            }
        }

        public ICollection<Student> GetStudents()
        {
            return _context.Student.ToList();
        }

        public void deleteStudent(string index)
        {
            var studentToRemove = new Student
            {
                IndexNumber = index,
            };
            _context.Attach(studentToRemove);
            _context.Remove(studentToRemove);
            _context.SaveChanges();
        }

        public void modifyStudentData(ModifyStudentDataRequest req)
        {
            var newStudent = new Student
            {
                IndexNumber = req.IndexNumber,
                FirstName = req.FirstName,
                LastName = req.LastName,
                BirthDate = req.BirthDate,
                Password = req.Password
            };
            _context.Attach(newStudent);

            _context.Entry(newStudent).Property("FirstName").IsModified = true;

            _context.Entry(newStudent).Property("LastName").IsModified = true;

            _context.Entry(newStudent).Property("BirthDate").IsModified = true;

            _context.Entry(newStudent).Property("Password").IsModified = true;

            _context.SaveChanges();
        }
    }
}
