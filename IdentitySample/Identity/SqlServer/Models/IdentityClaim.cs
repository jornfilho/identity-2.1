using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentitySample.Identity.SqlServer.Models
{
    [Table("AspNetClaims")]
    public class IdentityClaim
    {
        public IdentityClaim()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Fornce�a um nome para a Claim")]
        [MaxLength(128, ErrorMessage = "Tamanho m�ximo {0} excedido")]
        [Display(Name = "Nome da Claim")]
        public string Name { get; set; }
    }
}