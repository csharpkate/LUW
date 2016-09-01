using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Luw.Models
{
    public class MemberChapter
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public int ChapterId { get; set; }
        public DateTime WhenJoined { get; set; }
        public DateTime? WhenLeft { get; set; }
        public virtual Chapter Chapter { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
