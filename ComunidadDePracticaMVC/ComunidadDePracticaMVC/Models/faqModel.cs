using System.ComponentModel.DataAnnotations;

namespace ComunidadDePracticaMVC.Models
{
    public class FaqModel
    {
        
        public int Id { get; set; }
        [Required(ErrorMessage = "Una pregunta es requerida.")]
        public string Pregunta { get; set; }
        [Required(ErrorMessage = "Una respuesta es requerida.")]
        public string Respuesta { get; set; }
    }
}