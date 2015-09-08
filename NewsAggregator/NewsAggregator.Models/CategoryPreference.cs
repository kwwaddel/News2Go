using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Models
{
    public class CategoryPreference
    {
        public int Id { get; set; }
        public String CategoryName { get; set; }
        public bool Display { get; set; }
        [ForeignKey("Preference")]
        public String PreferenceId { get; set; }
        public virtual Preference Preference { get; set; }
    }
}
