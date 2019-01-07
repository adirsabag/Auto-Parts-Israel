using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace HW2.Models
{
    public class Admin
    {
        [Required]
        public string AdminFirstName { get; set; }

        [Required]
        public string AdminLastName { get; set; }

        [Key]
        [Required]
        public string AdminUsername { get; set; }

        [Required]
        public string AdminPassword { get; set; }

        [Required]
        public string AdminEmail { get; set; }

        [Required]
        public string AdminCode { get; set; }

    }
}