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
        public List<UsuarioModel> Revisores = new List<UsuarioModel>();
        public int CantidadRevisores { get; set; }
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

    public class CalificacionesModel { 
        public int ArticuloId { get; set; }
        public string Revisor { get; set; }
        public int MeritoRevisor { get; set; }
        public string Estado { get; set; }
        public string Comentario { get; set; }
        public int Calificacion { get; set; }
    }

    public class DecisionFinalModel {
        public int ArticuloId { get; set; }
        public int NotaFinal { get; set; }
        [Required(ErrorMessage = "Es necesario que indique su decisión antes de enviar")]
        public string Decision { get; set; }
        [Required(ErrorMessage = "Un comentario es requerido.")]
        public string Comentario { get; set; }
    }
}