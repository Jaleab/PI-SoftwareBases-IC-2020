using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComunidadDePracticaMVC.Models;
using ComunidadDePracticaMVC.Services;

namespace ComunidadDePracticaMVC.Controllers
{
    public class RevisionesController : Controller
    {
        public ActionResult ArticulosRequierenRevision()
        {
            ViewBag.categoria = "";
            ViewBag.revisiones = new RevisionesModel();
            RevisionesService servicioRevisiones = new RevisionesService();
            if (User.Identity.IsAuthenticated)
            {
                string correo = User.Identity.Name.ToString();
                UsuarioService servicioUsuarios = new UsuarioService();
                var datos = servicioUsuarios.GetDatosMiembro(correo);
                ViewBag.categoria = datos[0];
                ViewBag.revisiones = servicioRevisiones.ObtenerArticulosEnRevision();
            }
            return View();
        }

        [Authorize]
        public ActionResult MisArticulosRevision(String correo) {
            ViewBag.correo = correo;
            return View();
        }
    }


}