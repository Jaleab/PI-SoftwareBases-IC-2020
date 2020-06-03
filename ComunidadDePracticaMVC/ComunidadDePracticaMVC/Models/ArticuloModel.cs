using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ComunidadDePracticaMVC.Models
{
    public class ArticuloModel
    {

        [Display(Name = "ArticuloId")]
        public int ArticuloId { get; set; }

        [Required(ErrorMessage = "Un autor es requerido.")]
        public string Autor { get; set; }

        [Required(ErrorMessage = "Ingrese el contenido del articulo.")]
        [AllowHtml]
        public string Contenido { get; set; }

        [Required(ErrorMessage = "Ingrese el resumen del articulo.")]
        [AllowHtml]
        public string Resumen { get; set; }

        [Required(ErrorMessage = "Un titulo es requerido.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Un topico es requerido.")]
        public string Topico { get; set; }

        public int PuntajeLectores { get; set; }

        public int NotaRevision { get; set; }

        public string FechaPublicacion { get; set; }

        public string TipoArchivo { get; set; }

        public int Likes { get; set; }

        public int Dislikes { get; set; }
    }
}
