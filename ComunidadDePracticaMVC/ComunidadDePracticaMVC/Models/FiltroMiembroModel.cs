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
    }

}