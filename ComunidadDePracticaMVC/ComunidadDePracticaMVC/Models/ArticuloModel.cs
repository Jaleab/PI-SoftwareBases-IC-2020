using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;

namespace ComunidadDePracticaMVC.Models
{
    public class ArticuloModel
    {

        [Display(Name = "ArticuloId")]
        public int ArticuloId { get; set; }

        public string Autor { get; set; }

        public List<SelectListItem> Autores { get; set;  }

        [Required(ErrorMessage = "Un titulo es requerido.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Ingrese el resumen del articulo.")]
        [AllowHtml]
        public string Resumen { get; set; }


        [Required(ErrorMessage = "Ingrese el contenido del articulo.")]
        [AllowHtml]
        public string Contenido { get; set; }


        [Required(ErrorMessage = "Un topico es requerido.")]
        public string Topico { get; set; }

        public int CantidadVisitas { get; set; }
       

        public DateTime FechaPublicacion{ get; set; }

        public string TipoArchivo { get; set; }
        public byte[] Archivo { get; set; }







    }
}
