using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ComunidadDePracticaMVC.Models;

namespace ComunidadDePracticaMVC.Controllers
{
    public class FiltrarMiembroController : Controller
    {
        // GET: FiltrarMiembro
        public ActionResult FiltrarMiembro(FiltrarMiembroModel filtro)
        {
            filtro.Paises = new SelectList("Costa Rica", "Panama");
            filtro.Idiomas = new SelectList("Costa Rica", "Panama");
            filtro.Habilidades = new SelectList("Costa Rica", "Panama");
            filtro.Hobbies = new SelectList("Costa Rica", "Panama");
            return View(filtro);
        }
    }
}
