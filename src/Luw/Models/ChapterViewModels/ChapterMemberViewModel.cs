using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Luw.Models.ChapterViewModels
{
    public class ChapterMemberViewModel
    {
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name ="Member Since")]
        public DateTime WhenJoined { get; set; }
        public string Email { get; set; }
    }
}
