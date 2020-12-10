using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMA.Models.ViewModel
{
    public class SubjectViewModel
    {
        public Subject Subject { get; set; }
        public IEnumerable <Make>Makes { get; set; }
        public IEnumerable<Model> Models { get; set; }
    }


}
