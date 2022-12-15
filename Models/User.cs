using System;
using System.ComponentModel.DataAnnotations;

namespace UnitOfWork.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
