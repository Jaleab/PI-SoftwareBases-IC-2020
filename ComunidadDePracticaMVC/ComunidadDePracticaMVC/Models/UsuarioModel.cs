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

        [Required(ErrorMessage ="Ingrese su primer  apellido")]
        [Display(Name = "Apellido1")]
        public string Apellido1 { get; set; }

        [Required(ErrorMessage = "Ingrese su segundo apellido")]
        [Display(Name = "Apellido2")]
        public string Apellido2 { get; set; }

        [Required(ErrorMessage = "Ingrese su ciudad")]
        [Display(Name = "Ciudad")]
        public string Ciudad { get; set; }

        [Required(ErrorMessage = "Ingrese un pais")]
        [Display(Name = "Pais")]
        public string Pais { get; set; }

        
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

    public class UsuarioModelCompleto
    {

        [Display(Name = "Idioma")]
        public List<string> Idioma { get; set; }

        [Display(Name = "Usuario")]
        public List<UsuarioModel> Usuario { get; set; }

        [Display(Name = "Articulo")]
        public List<ArticuloModel> Articulo { get; set; }

        [Display(Name = "Hobbie")]
        public List<string> Hobbie { get; set; }

        [Display(Name = "Habilidad")]
        public List<string> Habilidad { get; set; }



    }
}