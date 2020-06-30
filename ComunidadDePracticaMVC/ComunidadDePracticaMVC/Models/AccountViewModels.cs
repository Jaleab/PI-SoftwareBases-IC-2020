using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComunidadDePracticaMVC.Models
{

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Una dirección de correo es requerida.")]
        [EmailAddress]
        [Display(Name = "Correo")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Una contraseña es requerida.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {
        

        [Required(ErrorMessage = "Una dirección de correo es requerida.")]
        [EmailAddress]
        [Display(Name = "Correo")]
        public string Email{ get; set; }

        [Required(ErrorMessage = "El nombre del usuario es requerido.")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El apellido del usuario es requerido.")]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El país del usuario es requerido.")]
        [Display(Name = "País")]
        public string Country { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La contraseña debe ser al menos de {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación no son iguales.")]
        public string ConfirmPassword { get; set; }
    }

    public class CookieModel {
        public string Categoria { get; set; }
        public int Merito { get; set; }
        public int Peso { get; set; }
        public string Nombre { get; set; }

    }
}
