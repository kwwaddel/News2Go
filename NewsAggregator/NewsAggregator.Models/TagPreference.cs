using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Models
{
    public class TagPreference
    {
        public int Id { get; set; }
        [ForeignKey("Tag")]
        public String TagId { get; set; }
        public virtual Tag Tag { get; set; }
        public int Rating { get; set; }
        [ForeignKey("Preference")]
        public String PreferenceId { get; set; }
        public virtual Preference Preference { get; set; }
    }
}
