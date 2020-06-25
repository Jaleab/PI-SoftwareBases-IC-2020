using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ComunidadDePracticaMVC.Models;

namespace ComunidadDePracticaMVC.Models
{
    public class RevisionesModel
    {
        public List<ArticuloModel> ArticulosEnRevision = new List<ArticuloModel>();
        public List<UsuarioModel> Colaboradores = new List<UsuarioModel>();
        public int cantidadRevisores { get; set; }
        
    }

    public class FormularioModel
    {
        [Required]
        [Display(Name = "Opinion General: ")]
        public int OpinionGeneral { get; set; }
        [Required]
        public int Contribucion { get; set; }
        [Required]
        public int Forma { get; set; }
        [Required]
        public string Estado { get; set; }
        [Required(ErrorMessage = "Comentario Requerido")]
        public string Comentario { get; set; }
        
    }

}