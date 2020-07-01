using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookWebApplication.Models
{
    public class CoverType
    {
        [Display(Name = "CoverType Id")]
        [Key]
        public int Id { get; set; }

        [Display(Name="CoverType Name")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
