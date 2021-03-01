using System;
using System.ComponentModel.DataAnnotations;

namespace DesafioWiPro.Domain.Entities
{
    public class Carteira
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigátorio")]
        [MaxLength(3, ErrorMessage = "Este campo deve ter  3  caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve ter  3  caracteres")]
        public string Moeda { get; set; }


        [Required(ErrorMessage = "Este campo é obrigátorio")]
        public DateTime Data_Inicio { get; set; }


        [Required(ErrorMessage = "Este campo é obrigátorio")]
        public DateTime Data_Fim { get; set; } 
    }
}
