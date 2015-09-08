using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Models
{
    public class FriendRequest
    {
        public int Id { get; set; }
        public String RequesterId { get; set; }
        public String TargetId { get; set; }
        public String TargetName { get; set; }
        public String RequesterName { get; set; }
        public bool IsActive { get; set; }
    }
}
