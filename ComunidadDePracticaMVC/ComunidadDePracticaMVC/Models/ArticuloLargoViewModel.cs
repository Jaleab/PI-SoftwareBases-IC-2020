using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComunidadDePracticaMVC.ViewModels
{
    public class ArticuloLargoViewModel
    {
        [Required]
        [DisplayName("Artículo en fórmato editable")]
        public HttpPostedFileBase Archivo1 {get; set;}

        [Required]
        [DisplayName("Titulo")]
        public string Titulo { get; set; }

        [Required]
        [DisplayName("Topico")]
        public string Topico { get; set; }

        [Required]
        [DisplayName("Autor")]
        public string Autor { get; set; }

        [Required]
        [DisplayName("Correo")]
        public string Correo { get; set; }

        [Required]
        [AllowHtml]
        [DisplayName("Resumen")]
        public string Resumen { get; set; }

    }
}