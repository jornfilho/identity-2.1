using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IdentitySample.ViewModels.Claim
{
    public class SetUserClaimViewModel
    {
        public SelectList ClainsList { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Nome da Claim")]
        public string Type { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Valor da Claim")]
        public string Value { get; set; }
    }
}
