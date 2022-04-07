using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class User : IdentityUser
    {
        public int UserId { get; set; }

        public string Password { get; set; }
        public override string UserName { get; set; }
    }
}
