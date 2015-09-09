using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;

namespace Demo.Models
{
    public class Person
    {
        [Key]
        public String ID { get; set; }
        [Required]
        public String Name { get; set; }
        public String Description { get; set; }
        public List<Trip> Trips { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach(Trip t in Trips)
            {
                sb.AppendLine(t.ToString());
            }
            return string.Format("ID:{0}\r\nName:{1}\r\nDescription:{2}\r\nTrips:{3}", ID, Name, Description, sb.ToString());
        }
    }
}