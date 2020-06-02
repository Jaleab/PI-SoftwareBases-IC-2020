using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ComunidadDePracticaMVC.ViewModels
{
    public class ArticuloLargoViewModel
    {
        [Required]
        [DisplayName("Archivo 1")]
        public HttpPostedFileBase Archivo1 {get; set;}

        [Required]
        [DisplayName("Titulo")]
        public string Titulo { get; set; }

        [Required]
        [DisplayName("Autor")]
        public string Autor { get; set; }

        [Required]
        [DisplayName("Resumen")]
        public string Resumen { get; set; }

    }
}