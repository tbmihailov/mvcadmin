using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcAdminResearch.Models
{
    public class Note
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Subject { get; set; }
        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}