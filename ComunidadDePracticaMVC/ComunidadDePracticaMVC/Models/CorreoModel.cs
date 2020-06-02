using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComunidadDePracticaMVC.Models
{
    public class CorreoModel
    {
        [EmailAddress]
        [Display(Name = "Para")]
        [Required(ErrorMessage = "El destinatario es requerido.")]
        public string EmailTo { get; set; }

        [Required(ErrorMessage = "El asunto es requerido.")]
        [Display(Name = "Asunto")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "El contenido del correo es requerido.")]
        [AllowHtml]
        public string EmailBody { get; set; }

        public string Sender { get; set; }
    }
}