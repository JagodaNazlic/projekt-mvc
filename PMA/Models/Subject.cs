using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMA.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public Make Make { get; set; }
        [RegularExpression("^[1-9]*$",ErrorMessage ="Select Make")]
        public int MakeID { get; set; }
        [RegularExpression("^[1-9]*$", ErrorMessage = "Select Make")]
        public Model Model { get; set; }
        [RegularExpression("^[1-9]*$", ErrorMessage = "Select Make")]
        public int ModelID { get; set; }

        [Required(ErrorMessage = "Provide Description")]
        public string Description { get; set; }
        [Required (ErrorMessage ="Provide Author")]
        public string Author { get; set; }
        public string ImagePath { get; set; }
    }
}
