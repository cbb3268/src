using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nib.Exercise.Models
{
    /// <summary>
    /// Information about a specific vacancy avaible at Nib
    /// </summary>
    public class Vacancy
    {        

        public int LocationId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ShortDescription { get; set; }
        


        public string City { get; set; }

        public string Month { get; set; }

        public int MonthDay { get; set; }
    }
}
