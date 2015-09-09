using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.Models
{
    public class Trip
    {
        [Key]
        public String ID { get; set; }
        [Required]
        public String Name { get; set; }

        public override string ToString()
        {
            return string.Format("\tid:{0}\tName:{1}", ID, Name);
        }
    }
}