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
        public ActionResult FiltrarMiembro(FiltroMiembroModel filtro)
        {
            FiltrarMiembroService filtrarServicio = new FiltrarMiembroService();
            ViewBag.listaPaises = filtrarServicio.getPaises();
            ViewBag.listaIdiomas = filtrarServicio.getIdiomas();
            ViewBag.listaHabilidades = filtrarServicio.getHabilidades();
            ViewBag.listaHobbies = filtrarServicio.getHobbies();
            return View(filtro);
        }

        [HttpPost]
        public ActionResult Consultar(FiltroMiembroModel filtro)
        {
            /*
            FiltrarMiembroService filtrarServicio = new FiltrarMiembroService();
            List<MiembroModel> usuarios = filtrarServicio.getUsuarios();

            List<MiembroModel> filterResultes = new List<MiembroModel>();

            // display all 

            // findall
            if (filtro.paisSeleccionado != "")
                filterResultes.AddRange(usuarios.FindAll(model => model.pais == filtro.paisSeleccionado));
            if (filtro.habilidadSeleccionado != "")
                filterResultes.AddRange(usuarios.FindAll(model => model.habilidad == filtro.habilidadSeleccionado));
            if (filtro.hobbieSeleccionado != "")
                filterResultes.AddRange(usuarios.FindAll(model => model.hobbie == filtro.hobbieSeleccionado));
            if (filtro.idiomaSeleccionado != "")
                filterResultes.AddRange(usuarios.FindAll(model => model.idioma == filtro.idiomaSeleccionado));

            var noDupsList = new HashSet<MiembroModel>(filterResultes).ToList();  

            return View(noDupsList); */
            FiltrarMiembroService filtrarServicio = new FiltrarMiembroService();
            List<MiembroModel> usuarios = filtrarServicio.GetDatosPersonalesByFiltro(filtro);

            return View(usuarios);

        }

    }
}