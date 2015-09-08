using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Models
{
    public class SharedArticleUser
    {
        public int Id { get; set; }
        public int MadeRating { get; set; }
        [ForeignKey("SharedArticle")]
        public String SharedArticleId { get; set; }
        //[InverseProperty("Users")]
        public virtual SharedArticle SharedArticle { get; set; }
        [ForeignKey("User")]
        public String UserId { get; set; }
        //[InverseProperty("SharedArticles")]
        public virtual ApplicationUser User { get; set; }
        
    }
}
