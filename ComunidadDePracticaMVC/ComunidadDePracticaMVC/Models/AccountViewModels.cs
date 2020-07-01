using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComunidadDePracticaMVC.Models
{

    public class LoginViewModel
    {
        [EmailAddress(ErrorMessage = "El formato de la dirección del correo no es válida.")]
        [StringLength(100, ErrorMessage = "La dirección de correo debe ser de máximo 100 caracteres")]
        [Required(ErrorMessage = "Una dirección de correo es requerida.")]
        [Display(Name = "Correo")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Una contraseña es requerida.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe ser mayor a 6 caracteres, pero menor a 100 caracteres")]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {
        [EmailAddress(ErrorMessage ="El formato de la dirección del correo no es válida.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La dirección de correo debe ser mayor a 3 caracteres, pero menor a 100 caracteres")]
        [Required(ErrorMessage = "Una dirección de correo es requerida.")]        
        [Display(Name = "Correo")]
        public string Email{ get; set; }

        [StringLength(50, ErrorMessage = "El nombre debe ser de máximo 50 caracteres")]
        [Required(ErrorMessage = "El nombre del usuario es requerido.")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "El primer apellido debe ser de máximo 50 caracteres")]
        [Required(ErrorMessage = "El apellido del usuario es requerido.")]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [StringLength(50, ErrorMessage = "El segundo apellido debe ser de máximo 50 caracteres")]
        [Required(ErrorMessage = "El país del usuario es requerido.")]
        [Display(Name = "País")]
        public string Country { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe ser mayor a 6 caracteres, pero menor a 100 caracteres")]
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "Una contraseña es requerida.")]
        public string Password { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "La confirmacion de la clave debe ser de igual tamaño.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación no son iguales.")]
        [Required(ErrorMessage = "Una confirmacion de la contraseña es requerida.")]
        public string ConfirmPassword { get; set; }
    }

    public class CookieModel {
        public string Categoria { get; set; }
        public int Merito { get; set; }
        public int Peso { get; set; }
        public string Nombre { get; set; }

    }
}
