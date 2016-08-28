using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace Luw.Models.MemberViewModels
{
    public class MemberCreateViewModel
    {
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "Address")]
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set;}

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(20)]
        public string Status { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Member Since")]
        public DateTime WhenJoined { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Expires")]
        public DateTime WhenExpires { get; set; }

        [Display(Name = "Chapter 1")]
        public int Chapter1 { get; set; }

        [Display(Name = "Chapter 2")]
        public int Chapter2 { get; set; }

        public List<SelectListItem> Chapters { get; set; }

        public string Note { get; set; }
    }
}
