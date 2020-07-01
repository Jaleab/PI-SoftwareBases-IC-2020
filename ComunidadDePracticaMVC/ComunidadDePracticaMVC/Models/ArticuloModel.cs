using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComunidadDePracticaMVC.Models
{
    public class ArticuloModel
    {

        [Display(Name = "ArticuloId")]
        public int ArticuloId { get; set; }

        [Required(ErrorMessage = "Un autor es requerido.")]
        public string Autor { get; set; }

        public List<string> Correos { get; set; }

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

        [Required(ErrorMessage = "Un correo es requerido.")]
        public string Correo { get; set; }

        public int PuntajeLectores { get; set; }

        public int NotaRevision { get; set; }

        public int CantidadVisitas { get; set; }

        public string FechaPublicacion { get; set; }

        public string TipoArchivo { get; set; }

        [Required]
        [DisplayName("Artículo con fórmato PDF")]
        public HttpPostedFileBase Archivo { get; set; }

        public int Likes { get; set; }

        public int Dislikes { get; set; }

        public string Estado { get; set; }

    }
}
