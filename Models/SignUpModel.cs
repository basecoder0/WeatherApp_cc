using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApp_cc.Models
{
    public class SignUpModel
    {        
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
                
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "City Required")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

       
        [Display(Name = "Zip")]
        public int Zip { get; set; }

    }
}
