using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Models
{
    public class Coffee
    {
        
        public Guid CoffeeId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Producer { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Region { get; set; }


        //[ForeignKey("User")]
        //public int UserId { get; set; }

        //public User User { get; set; }
    }
}
