using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace P110_Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, StringLength(100)]
        public string Firstname { get; set; }

        [StringLength(100)]
        public string Lastname { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        public int CityId { get; set; }

        public virtual City City { get; set; }
    }
}
