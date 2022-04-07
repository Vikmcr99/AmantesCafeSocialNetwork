using Microsoft.AspNetCore.Http;
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

        public string Url { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }
        public List<Coffee_User> CoffeeUsers { get; set; }
        public string Username { get; set; }
    }
}
