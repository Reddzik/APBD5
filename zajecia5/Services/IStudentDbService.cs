﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zajecia5.DOTs.Requests;
using zajecia5.Models;

namespace zajecia5.Services
{
    public interface IStudentDbService
    {
        Student EnrollStudent(EnrollStudentRequest req);
        Boolean PromoteStudents(PromoteStudentsRequest req);
        Student GetStudent(string index);
        Boolean IsThereStudent(string index);
        Boolean CheckCredential(string user, string password);

        string GetUserByRefreshToken(string token);

        Boolean AddRefreshTokenToUser(string token, string index);
    }
}
