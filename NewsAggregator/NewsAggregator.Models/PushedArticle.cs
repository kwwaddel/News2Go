using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Models
{
    public class PushedArticle
    {
        public int Id { get; set; }
        public String ArticleId { get; set; }
        [ForeignKey("From")]
        public String FromId { get; set; }
        public virtual ApplicationUser From { get; set; }
        [ForeignKey("To")]
        public String ToId { get; set; }
        public virtual ApplicationUser To { get; set; }
        public bool IsNew { get; set; }
    }
}
