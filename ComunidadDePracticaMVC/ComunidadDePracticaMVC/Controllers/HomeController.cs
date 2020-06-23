using ComunidadDePracticaMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComunidadDePracticaMVC.Services;


namespace ComunidadDePracticaMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.categoria = "";
            if (User.Identity.IsAuthenticated)
            {
                string correo = User.Identity.Name.ToString();
                UsuarioService servicioUsuarios = new UsuarioService();
                var datos = servicioUsuarios.GetDatosMiembro(correo);
                ViewBag.categoria = datos[0];
            }
            return View();
        }

        public ActionResult About()
        {

            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        

        public ActionResult ControlarForma()
        {
            ViewBag.Message = "Your ControlarForma page.";

            return View();
        }

        

    }
}