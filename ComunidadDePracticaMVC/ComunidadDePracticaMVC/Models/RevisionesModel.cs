using System;
using System.Collections.Generic;
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
        public int cantidadRevisores { get; set; }

    }

}