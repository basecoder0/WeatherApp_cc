using System.ComponentModel.DataAnnotations;

namespace WeatherApp_cc.Models
{
    //Model for Index view
    public class IndexModel
    {        
        [Required(ErrorMessage = "Please Enter a User Name")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }
    }
}
