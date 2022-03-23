using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Coffee_User
    {
        public int Id { get; set; }

        public Guid CoffeeId { get; set; }

        public Coffee Coffee { get; set; }

        public int UserId { get; set; }

        public UserCoffee Usercoffee { get; set; }
    }
}
