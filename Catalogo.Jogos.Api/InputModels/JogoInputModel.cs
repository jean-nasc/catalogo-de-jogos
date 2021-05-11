using System;
using System.ComponentModel.DataAnnotations;

namespace Catalogo.Jogos.Api.InputModels
{
    public class JogoInputModel
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome do jogo deve conter entre {1} e {0} caracteres.")]
        public string Nome { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome da produtora deve conter entre {1} e {0} caracteres.")]
        public string Produtora { get; set; }
        
        [Required]
        [Range(1, 1000, ErrorMessage = "O preço deve ser de no mínimo {0} real e no máximo {1} reais.")]
        public double Preco { get; set; }
    }
}