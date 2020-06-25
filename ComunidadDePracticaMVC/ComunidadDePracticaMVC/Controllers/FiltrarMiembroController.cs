using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ComunidadDePracticaMVC.Models;
using ComunidadDePracticaMVC.Services;

namespace ComunidadDePracticaMVC.Controllers
{
    public class FiltrarMiembroController : Controller
    {
        // GET: FiltrarMiembro
        public ActionResult FiltrarMiembro(FiltrarMiembroModel filtro)
        {
            FiltrarMiembroService filtrarServicio = new FiltrarMiembroService();
            ViewBag.listaPaises = filtrarServicio.getPaises(); 
            ViewBag.listaIdiomas = filtrarServicio.getIdiomas();
            ViewBag.listaHabilidades = filtrarServicio.getHabilidades();
            ViewBag.listaHobbies = filtrarServicio.getHobbies();
            return View(filtro);
        }
    }
}
