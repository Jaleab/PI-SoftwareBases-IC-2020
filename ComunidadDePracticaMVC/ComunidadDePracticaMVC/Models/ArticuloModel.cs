using System;
using System.ComponentModel.DataAnnotations;

namespace ComunidadDePracticaMVC.Models
{
    public class ArticuloModel
    {

        [Display(Name = "ArticuloId")]
        public int ArticuloId { get; set; }

        [Required(ErrorMessage = "Un autor es requerido.")]
        public string Autor { get; set; }

        [Required(ErrorMessage = "Ingrese el pais del autor.")]
        public string Pais { get; set; }

        [Required(ErrorMessage = "Ingrese el contenido del articulo.")]
        public string Contenido { get; set; }

        [Required(ErrorMessage = "Ingrese el resumen del articulo.")]
        public string Resumen { get; set; }


    }
}
