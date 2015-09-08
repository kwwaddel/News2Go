using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Models
{
    public class Tag
    {
        [Key]
        public String Name { get; set; }
        [ForeignKey("Category")]
        public String CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<TagPreference> TagPreference { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            bool b = false;

            Tag t = obj as Tag;

            if (t.Name == this.Name)
                b = true;

            return b;
        }

    }
}
