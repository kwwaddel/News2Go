using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public String Comm { get; set; }

        [ForeignKey("SharedArticle")]
        public String SharedArticleId { get; set; }
        public virtual SharedArticle SharedArticle { get; set; }

        [ForeignKey("User")]
        public String UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
