using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComunidadDePracticaMVC.Models
{
    public class FiltroMiembroModel
    {
        public string paisSeleccionado { get; set; }

        public IEnumerable<SelectListItem> Paises { get; set; }

        public string habilidadSeleccionado { get; set; }

        public IEnumerable<SelectListItem> Habilidades { get; set; }

        public string idiomaSeleccionado { get; set; }

        public IEnumerable<SelectListItem> Idiomas { get; set; }
        public string hobbieSeleccionado { get; set; }

        public IEnumerable<SelectListItem> Hobbies { get; set; }

        public List<string> hobbiesSeleccionados { get; set; }
        public List<string> idiomasSeleccionados { get; set; }
        public List<string> habilidadesSeleccionados { get; set; }
        public List<string> paisesSeleccionados { get; set; }

    }

    public class DataModel
    {
        string titulo;
        List<int> datos;
        List<string> nombres;

        public List<string> categorias { get; set; }
        public List<string> etiquetas { get; set; }
        public List<int> counts { get; set; }
    }
}