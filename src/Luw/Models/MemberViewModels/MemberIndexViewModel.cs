using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Luw.Models.MemberViewModels
{
    public class MemberIndexViewModel
    {
        public string Status { get; set; }
        public IList<MemberIndexRow> Members { get; set; }
    }

    public class MemberIndexRow
    {
        public string Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Expires")]
        public DateTime WhenExpires { get; set; }
        public string Chapter { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
    }
}
