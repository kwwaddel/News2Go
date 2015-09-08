using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Models
{
    public class Article
    {
        public String Id { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public int Popularity { get; set; }
        public int Like { get; set; }
        public int Dislike { get; set; }
        [ForeignKey("Category")]
        public String CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
