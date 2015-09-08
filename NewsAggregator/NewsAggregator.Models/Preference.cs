using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Models
{
    public class Preference
    {
        //public int Id { get; set; }
        //public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<CategoryPreference> CategoryPrefs { get; set; }
        public virtual ICollection<TagPreference> TagPrefs{ get; set; }

        [Key, ForeignKey("User")]
        public String UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
