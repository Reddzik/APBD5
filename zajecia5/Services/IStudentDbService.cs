using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zajecia5.DOTs.Requests;
using zajecia5.RenderedModels;

namespace zajecia5.Services
{
    public interface IStudentDbService
    {
        public ICollection<Student> GetStudents();
        public void deleteStudent(string index);
        public void modifyStudentData(ModifyStudentDataRequest req);
        Student EnrollStudent(EnrollStudentRequest req);
        Boolean PromoteStudents(PromoteStudentsRequest req);
        Student GetStudent(string index);
        Boolean IsThereStudent(string index);
        Boolean CheckCredential(string user, string password);
        Student GetUserByRefreshToken(string token);
        Boolean AddRefreshTokenToUser(string token, string index);
        Student GetLoggedStudent(string login, string password);
    }
}
