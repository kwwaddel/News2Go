using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Models
{
    public class Category
    {
        [Key]
        public String Name { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Article> Articles { get; set; }

        [ForeignKey("Preference")]
        public String PreferenceId { get; set; }
        public virtual Preference Preference { get; set; }
    }
}
