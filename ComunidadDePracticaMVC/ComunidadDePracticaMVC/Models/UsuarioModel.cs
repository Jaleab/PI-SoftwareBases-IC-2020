using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ComunidadDePracticaMVC.Models
{


    public class UsuarioModel
    {
        [Display(Name = "Correo")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "Ingrese su nombre")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage ="Ingrese su apellido")]
        [Display(Name = "Apellido1")]
        public string Apellido1 { get; set; }

        
        [Display(Name = "Apellido2")]
        public string Apellido2 { get; set; }

        
        [Display(Name = "Ciudad")]
        public string Ciudad { get; set; }

        [Required(ErrorMessage = "Ingrese un pais")]
        [Display(Name = "Pais")]
        public string Pais { get; set; }

        [Required(ErrorMessage = "Ingrese un idioma")]
        [Display(Name = "Idioma")]
        public string Idioma { get; set; }

        [Display(Name = "Merito")]
        public string Merito { get; set; }

        [Display(Name = "Peso")]
        public string Peso { get; set; }

        [Display(Name = "Categoria")]
        public string Categoria { get; set; }

        [Required(ErrorMessage = "Ingrese una habilidad.")]
        [Display(Name = "Habilidad")]
        public string Habilidad { get; set; }

        [Required(ErrorMessage = "Ingrese un hobbie")]
        [Display(Name = "Hobbie")]
        public string Hobbie { get; set; }

    }
}