using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Luw.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Display(Name="First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name="Last Name")]
        [Required]
        public string LastName { get; set; }

        [Display(Name="Address")]
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        
        [MaxLength(20)]
        public string Status { get; set; }

        [DataType(DataType.Date)]
        [Display(Name="Member Since")]
        public DateTime WhenJoined { get; set; }

        [DataType(DataType.Date)]
        [Display(Name="Expires")]
        public DateTime WhenExpires { get; set; }

        public virtual IList<MemberChapter> Chapters { get; set; }
    }
}
