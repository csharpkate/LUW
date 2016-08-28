using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Luw.Models
{
    public class ApplicationUserNote
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public Guid AddedBy { get; set; }
        public DateTime WhenAdded { get; set; }
        public string Note { get; set; }
    }
}
