using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ComunidadDePracticaMVC.Models
{


    public class UsuarioModel
    {
        [Required]
        [Display(Name = "Correo")]
        public string Correo { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Apellido1")]
        public string Apellido1 { get; set; }


        [Display(Name = "Apellido2")]
        public string Apellido2 { get; set; }


        [Display(Name = "Ciudad")]
        public string Ciudad { get; set; }


        [Display(Name = "Foto")]
        public string Foto { get; set; }

        [Required]
        [Display(Name = "Pais")]
        public string Pais { get; set; }

        [Required]
        [Display(Name = "Categoria")]
        public string Categoria { get; set; }

        [Required]
        [Display(Name = "Habilidad")]
        public string Habilidad { get; set; }

        [Required]
        [Display(Name = "Hobbie")]
        public string Hobbie { get; set; }


    }
} 