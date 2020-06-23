using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ComunidadDePracticaMVC.Models
{
    public class faqModel
    {
        
        public int Id { get; set; }
        [Required(ErrorMessage = "Una pregunta es requerida.")]
        public string Pregunta { get; set; }
        [Required(ErrorMessage = "Una respuesta es requerida.")]
        public string Respuesta { get; set; }
    }
}