﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TokenAuthentcation.Models;

namespace TokenAuthentcation.Controllers
{
    [Produces("application/json")]
    [Route("api/Books")]
    public class BooksController : Controller
    {
        [HttpGet("[action]"), Authorize]
        public IEnumerable<Book> Get()
        {
            var currentUser = HttpContext.User;
            
            var resultBookList = new Book[] {
                new Book { Author = "Ray Bradbury",Title = "Fahrenheit 451" },
                new Book { Author = "Gabriel García Márquez", Title = "One Hundred years of Solitude" },
                new Book { Author = "George Orwell", Title = "1984" },
                new Book { Author = "Anais Nin", Title = "Delta of Venus" }
              };

            return resultBookList;
        }        

        [HttpGet("[action]"), Authorize]
        public IEnumerable<Book> GetBookWithRestriction()
        {
            var currentUser = HttpContext.User;
            int userAge = 0;
            var resultBookList = new Book[] {
              new Book { Author = "Ray Bradbury", Title = "Fahrenheit 451", AgeRestriction = false },
              new Book { Author = "Gabriel García Márquez", Title = "One Hundred years of Solitude", AgeRestriction = false },
              new Book { Author = "George Orwell", Title = "1984", AgeRestriction = false },
              new Book { Author = "Anais Nin", Title = "Delta of Venus", AgeRestriction = true }
            };

            if (currentUser.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            {
                DateTime birthDate = DateTime.Parse(currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.DateOfBirth).Value);
                userAge = DateTime.Today.Year - birthDate.Year;
            }

            if (userAge < 18)
            {
                resultBookList = resultBookList.Where(b => !b.AgeRestriction).ToArray();
            }

            return resultBookList;
        }
    }
}