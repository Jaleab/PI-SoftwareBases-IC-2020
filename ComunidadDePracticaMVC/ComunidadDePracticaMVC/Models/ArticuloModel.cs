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

        [AllowHtml]
        [StringLength(30000, MinimumLength = 3000, ErrorMessage = "El contenido debe ser de al menos 3000 caracteres, pero menor a 30000 caracteres")]
        [Required(ErrorMessage = "El contenido es requerido.")]
        [Display(Name = "Contenido")]
        public string Contenido { get; set; }

        [AllowHtml]
        [StringLength(1500, MinimumLength = 500, ErrorMessage = "El contenido debe ser de al menos 500 caracteres, pero menor a 1500 caracteres")]
        [Required(ErrorMessage = "El resumen es requerido.")]
        [Display(Name = "Resumen")]
        public string Resumen { get; set; }

        [StringLength(300, MinimumLength = 50, ErrorMessage = "El contenido debe ser de al menos 50 caracteres, pero menor a 300 caracteres")]
        [Required(ErrorMessage = "El título es requerido.")]
        [Display(Name = "Titulo")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El topico es requerido.")]
        public string Topico { get; set; }

        [Required(ErrorMessage = "Un correo es requerido.")]
        public string Correo { get; set; }

        public int PuntajeLectores { get; set; }

        public int NotaRevision { get; set; }

        public int CantidadVisitas { get; set; }

        public string FechaPublicacion { get; set; }

        public string TipoArchivo { get; set; }

        [Required(ErrorMessage = "El archivo del artículo en formato PDF es requerido.")]
        [Display(Name = "Archivo del artículo en formato PDF")]
        public HttpPostedFileBase Archivo { get; set; }

        public int Likes { get; set; }

        public int Dislikes { get; set; }

        public string Estado { get; set; }

    }
}
