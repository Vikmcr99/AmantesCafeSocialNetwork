using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Models
{
    public class MidiaGallery
    {
        [Key]
        public int Id { get; set; }

        public string Url { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
