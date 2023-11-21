using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppointmentScheduling.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        [StringLength(100, ErrorMessage ="The {0} Must be at least {2} characters long.",MinimumLength =6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Compare("Password",ErrorMessage ="Password Mismatch!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string PasswordConfirmed { get; set; }
        
        [Required]
        [DisplayName("Role Name")]
        public string RoleName { get; set; }


    }
}
