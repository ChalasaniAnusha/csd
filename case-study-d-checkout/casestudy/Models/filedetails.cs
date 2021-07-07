using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace casestudy.Models
{
    public class filedetails
    {
        [Required]
        public string Name { get; set; }
        [Key]
        public string FileName { get; set; }
        [Required]
        public DateTime UploadDate { get; set; }


    }
}