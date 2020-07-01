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

        [StringLength(300, MinimumLength = 5, ErrorMessage = "Debe ser de al menos 50 caracteres, pero menor a 300 caracteres")]
        [Required(ErrorMessage = "El título es requerido.")]
        [Display(Name = "Titulo")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El topico es requerido.")]
        [DisplayName("Topico")]
        public string Topico { get; set; }

        [Required]
        [DisplayName("Autor")]
        public string Autor { get; set; }

        [Required]
        [DisplayName("Correo")]
        public string Correo { get; set; }

        [DisplayName("Correos")]
        public List<string> Correos { get; set; }

        [Required]
        [AllowHtml]
        [DisplayName("Resumen")]
        public string Resumen { get; set; }

        public int ArticuloId { get; set; }

    }
}