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

        public ActionResult UsuariosComunidad(string mensaje)
        {
            UsuarioService usuario = new UsuarioService();
            if (mensaje != "") {
                ViewBag.mensaje = mensaje;                               
            }
            return View(usuario.GetNombreUsuarios());
        }

        public ActionResult VerMeritosUsuario(string hilera)
        {
            UsuarioService usuario = new UsuarioService();
            string[] datos = usuario.GetDatosMiembro(hilera);
            ViewBag.categoriaMiembro = datos[0];
            ViewBag.merito = datos[1];
            ViewBag.peso = datos[2];
            return View(usuario.GetMeritosUsuario(hilera));
        }

        public ActionResult DegradarUsuario(string hilera)
        {

            UsuarioService usuario = new UsuarioService();
            string mensajeEvento = "No se ha podido degradar al usuario porque es Periferico";
            string[] datos = usuario.GetDatosMiembro(hilera);
            if (String.Equals(datos[0], "Nucleo",
                   StringComparison.OrdinalIgnoreCase))
            {
                usuario.DegradarPeso(hilera, "Activo", Int32.Parse(datos[1])-2, Int32.Parse(datos[2]));
                mensajeEvento = "Se ha degradado correctamente";
            }
            else {
                if (String.Equals(datos[0], "Activo",
                   StringComparison.OrdinalIgnoreCase))
                {
                    usuario.DegradarPeso(hilera, "Periferico", Int32.Parse(datos[1])-2, Int32.Parse(datos[2]));
                    mensajeEvento = "Se ha degradado correctamente";
                }
            }
            string correo = hilera;
            //return RedirectToAction("UsuariosComunidad");
            return RedirectToAction("UsuariosComunidad", "Usuario", new { mensaje = mensajeEvento });
        }


       
    }
}