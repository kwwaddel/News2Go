using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Models
{
    public class Friend
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public String UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public String Username { get; set; }
    }
}
