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

        [Required(ErrorMessage = "Un nombre es requerido.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Un apellido es requerido.")]
        [Display(Name = "Primer Apellido")]
        public string Apellido1 { get; set; }

        [Required(ErrorMessage = "Un segundo apellido es requerido.")]
        [Display(Name = "Segundo Apellido")]
        public string Apellido2 { get; set; }

        [Required(ErrorMessage = "Una ciudad es requerida.")]
        [Display(Name = "Ciudad")]
        public string Ciudad { get; set; }

        [Required(ErrorMessage = "Un país es requerido.")]
        [Display(Name = "País")]
        public string Pais { get; set; }

        [Required(ErrorMessage = "Un idioma es requerido.")]
        [Display(Name = "Idiomas")]
        public string Idioma { get; set; }

        [Display(Name = "Merito")]
        public string Merito { get; set; }

        [Display(Name = "Peso")]
        public string Peso { get; set; }

        [Display(Name = "Categoría")]
        public string Categoria { get; set; }

        [Required(ErrorMessage = "Una habilidad es requerida.")]
        [Display(Name = "Habilidades")]
        public string Habilidad { get; set; }

        [Required(ErrorMessage = "Un hobbie es requerido.")]
        [Display(Name = "Hobbies")]
        public string Hobbie { get; set; }

    }
} 