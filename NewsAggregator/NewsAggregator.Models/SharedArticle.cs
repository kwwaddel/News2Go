using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Models
{
    public class SharedArticle
    {
        public String Id { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}
