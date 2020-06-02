using ComunidadDePracticaMVC.Models;
using ComunidadDePracticaMVC.ArticuloService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mail;
using System.Web.Mvc;

namespace ComunidadDePracticaMVC.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario

        public ActionResult DesplegarUsuario(string correo = "edwin.brenes.c@gmail.com")
        {
            UsuarioService usuario = new UsuarioService();
            return View(usuario.GetUsuarioCompleto(correo));
        }


        public ActionResult DesplegarEditarUsuario(string correo = "edwin.brenes.c@gmail.com")
        {
            UsuarioService usuario = new UsuarioService();
            return View(usuario.GetUsuarioCompleto(correo));
        }



        [HttpPost]
        public JsonResult EditarUsuario(UsuarioModel model)
        {

            UsuarioService usuario = new UsuarioService();
            usuario.EditarUsuario(model);

            JsonResult result = Json(new
            {

                newUrl = ""
            }
                );
            
            return result;
        }


        public ActionResult NuevoUsuario()
        {

            return View();
        }

        public ActionResult Formulario(string correo="edwin.brenes.c@gmail.com")
        {
            UsuarioService usuario = new UsuarioService();
            return View(usuario.GetProfile(correo));
        }


    }
}
