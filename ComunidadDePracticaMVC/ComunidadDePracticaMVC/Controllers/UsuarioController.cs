using ComunidadDePracticaMVC.Models;
using ComunidadDePracticaMVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComunidadDePracticaMVC.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult VerUsuario(string correo = "edwin.brenes.c@gmail.com")
        {
            UsuarioService usuario = new UsuarioService();
            return View(usuario.GetProfile(correo));
        }


        public ActionResult VistaEditarUsuario(string correo = "edwin.brenes.c@gmail.com")
        {
            UsuarioService usuario = new UsuarioService();
            return View(usuario.GetProfile(correo));
        }

        [HttpPost]
        public void EditarUsuario(UsuarioModel usuarioModel)
        {

            UsuarioService usuario = new UsuarioService();
            usuario.EditarUsuario(usuarioModel);
        }

        public ActionResult NuevoUsuario()
        {

            return View();
        }

        public ActionResult Formulario(string correo = "edwin.brenes.c@gmail.com")
        {
            UsuarioService usuario = new UsuarioService();
            return View(usuario.GetProfile(correo));
        }

        public ActionResult UsuariosComunidad()
        {
            UsuarioService usuario = new UsuarioService();
            return View(usuario.GetNombreUsuarios());
        }

        public ActionResult VerMeritosUsuario(string correo)
        {
            return View();               
        }

    }
}