using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class UserCoffee
    {
        public int Id { get; set; }
        public string Fullname { get; set; }

        public List<Coffee_User> CoffeeUsers { get; set; }
    }
}
