using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApp_cc.Models
{
    public class IndexModel
    {        
        [Required(ErrorMessage = "Please Enter a User Name")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }
    }
}
