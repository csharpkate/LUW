using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Luw.Models.ChapterViewModels
{
    public class DetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SubName { get; set; }

        [Display(Name="Description")]
        public string Description { get; set; }
        public string Url { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Venue { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string MeetingInfo { get; set; }
        public string Notes { get; set; }
    }
}
