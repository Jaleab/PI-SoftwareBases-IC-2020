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

        public ActionResult DesplegarUsuario(string correo = "edwin.brenes.c@gmail.com")
        {
            UsuarioService usuario = new UsuarioService();
            return View(usuario.GetProfile(correo));
        }


        public ActionResult DesplegarEditarUsuario(string correo = "edwin.brenes.c@gmail.com")
        {

            UsuarioService usuario = new UsuarioService();
            return View(usuario.GetProfile(correo));
        }



        [HttpPost]
        public JsonResult EditarUsuario( UsuarioModel model)
        {

            UsuarioService usuario = new UsuarioService();
            usuario.EditarUsuario(model);
            JsonResult result = Json(new
            {
                Correo = model.Correo,
                Nombre = model.Nombre,
                Apellido1 = model.Apellido1,
                Apellido2 = model.Apellido2,
                Ciudad = model.Ciudad,
                Pais = model.Pais,
                Idioma = model.Idioma,
                Merito = model.Merito,
                Peso = model.Peso,
                Categoria = model.Categoria,
                Habilidad = model.Habilidad,
                Hobbie = model.Hobbie,
                responseMessage = "Correo o contraseña incorrecta.",
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

        [HttpPost]
        public String Prueba()
        {
            return "JOSTYN";
        }
    }
}
