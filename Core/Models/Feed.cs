using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Models
{
    public class Feed
    {
        
        public Guid Id { get; set; }
        [Required]
        public string Comment { get; set; }

        public string Imagepath { get; set; }

    }
}
