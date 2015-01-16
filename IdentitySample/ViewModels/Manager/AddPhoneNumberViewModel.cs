using System.ComponentModel.DataAnnotations;

namespace IdentitySample.ViewModels.Manager
{
    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Número do Telefone")]
        public string Number { get; set; }
    }
}
