using System.Collections.Generic;
using System.Web.Mvc;

namespace ComunidadDePracticaMVC.Models
{
    public class FiltroCategoriaMiembroModel
    {
        public string categoriaSeleccionada { get; set; }

        public IEnumerable<SelectListItem> categorias { get; set; }

        public string pu { get; set; }

        public IEnumerable<SelectListItem> Habilidades { get; set; }

        public string idiomaSeleccionado { get; set; }

        public IEnumerable<SelectListItem> Idiomas { get; set; }
        public string hobbieSeleccionado { get; set; }

        public IEnumerable<SelectListItem> Hobbies { get; set; }

        public List<string> hobbiesSeleccionados { get; set; }
        public List<string> idiomasSeleccionados { get; set; }
        public List<string> habilidadesSeleccionados { get; set; }
        public List<string> paisesSeleccionados { get; set; }

        public class DataModel
        {
            public List<string> categorias { get; set; }
            public List<string> etiquetas { get; set; }
            public List<int> counts { get; set; }
        }
    }
}