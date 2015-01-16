using System.ComponentModel.DataAnnotations;

namespace IdentitySample.ViewModels.Account
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }
}
