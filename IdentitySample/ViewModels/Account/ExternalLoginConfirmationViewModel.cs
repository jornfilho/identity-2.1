﻿using System.ComponentModel.DataAnnotations;

namespace IdentitySample.ViewModels.Account
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Sobrenome")]
        public string LastName { get; set; }
    }
}
