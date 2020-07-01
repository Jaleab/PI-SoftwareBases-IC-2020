using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ComunidadDePracticaMVC.Models
{

    public class UsuarioModel
    {
        [StringLength(100, ErrorMessage = "La dirección de correo debe ser de máximo 100 caracteres")]
        [Display(Name = "Correo")]
        public string Correo { get; set; }

        [StringLength(50, ErrorMessage = "El nombre debe ser de máximo 50 caracteres")]
        [Required(ErrorMessage = "Un nombre es requerido.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [StringLength(50, ErrorMessage = "El primer apellido debe ser de máximo 50 caracteres")]
        [Required(ErrorMessage = "Un apellido es requerido.")]
        [Display(Name = "Primer Apellido")]
        public string Apellido1 { get; set; }

        [StringLength(50, ErrorMessage = "El segundo apellido debe ser de máximo 50 caracteres")]
        [Required(ErrorMessage = "Un segundo apellido es requerido.")]
        [Display(Name = "Segundo Apellido")]
        public string Apellido2 { get; set; }

        [StringLength(50, ErrorMessage = "La ciudad debe ser de máximo 50 caracteres")]
        [Required(ErrorMessage = "Una ciudad es requerida.")]
        [Display(Name = "Ciudad")]
        public string Ciudad { get; set; }

        [StringLength(50, ErrorMessage = "El pais debe ser de máximo 50 caracteres")]
        [Required(ErrorMessage = "Un país es requerido.")]
        [Display(Name = "País")]
        public string Pais { get; set; }

        [StringLength(250, ErrorMessage = "Los idiomas deben ser máximo 250 caracteres")]
        [Required(ErrorMessage = "Un idioma es requerido.")]
        [Display(Name = "Idiomas")]
        public string Idioma { get; set; }

        [Display(Name = "Merito")]
        public string Merito { get; set; }

        [Display(Name = "Peso")]
        public string Peso { get; set; }

        [Display(Name = "Categoría")]
        public string Categoria { get; set; }

        [StringLength(250, ErrorMessage = "Las habilidades deben ser máximo 250 caracteres")]
        [Required(ErrorMessage = "Una habilidad es requerida.")]
        [Display(Name = "Habilidades")]
        public string Habilidad { get; set; }

        [StringLength(250, ErrorMessage = "Los hobbies deben ser máximo 250 caracteres")]
        [Required(ErrorMessage = "Un hobbie es requerido.")]
        [Display(Name = "Hobbies")]
        public string Hobbie { get; set; }

    }
} 