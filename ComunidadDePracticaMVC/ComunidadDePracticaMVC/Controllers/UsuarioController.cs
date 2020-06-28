using ComunidadDePracticaMVC.Models;
using ComunidadDePracticaMVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace ComunidadDePracticaMVC.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario

        public ActionResult DesplegarUsuario(string correo)
        {
            UsuarioService usuario = new UsuarioService();
            return View(usuario.GetProfile(correo));
        }


        public ActionResult DesplegarEditarUsuario(string correo)
        {

            UsuarioService usuario = new UsuarioService();
            return View( usuario.GetProfile(correo));
        }



        [HttpPost]
        public ActionResult EditarUsuario(UsuarioModel model)
        {

            UsuarioService usuarioDb = new UsuarioService();
            usuarioDb.EditarUsuario(model);
            return RedirectToAction("DesplegarUsuario", "Usuario", new { correo = model.Correo });
        }

        public ActionResult NuevoUsuario()
        {

            return View();
        }

        public ActionResult Formulario(string correo)
        {
            UsuarioService usuario = new UsuarioService();
            return View(usuario.GetProfile(correo));
        }

        [HttpPost]
        public String Prueba()
        {
            return "JOSTYN";
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
            ViewBag.nombre = datos[3];
            return View(usuario.GetMeritosUsuario(hilera));
        }

        public ActionResult DegradarUsuario(string hilera)
        {
            /*Agregar firma al correo para identificar que viene de la comunidad */ 
            UsuarioService usuario = new UsuarioService();
            string mensajeEvento = "No se ha podido degradar al usuario porque es Periferico";
            string[] datos = usuario.GetDatosMiembro(hilera);
        
            MailMessage mailMessage = new MailMessage
                ("comunidadDePracticaGrupo3@gmail.com", hilera);
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "Su rango fue degradado a una categoria menor.   Comunidad de practica Grupo 3";
            mailMessage.Subject = "Fue degradado";

            SmtpClient smtpClient;

            string nombreGrupo3 = "comunidadDePracticaGrupo3@gmail.com";
            string passwordGrupo3 = "comunidadPractica3.pass";

            smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new System.Net.NetworkCredential(nombreGrupo3, passwordGrupo3);

            if (String.Equals(datos[0], "Núcleo",
                   StringComparison.OrdinalIgnoreCase))
            {
                usuario.DegradarPeso(hilera, "Activo", Int32.Parse(datos[1])-2, Int32.Parse(datos[2])-2);
                mensajeEvento = "Se ha degradado correctamente";
                smtpClient.Send(mailMessage);
                smtpClient.Dispose();
            }
            else {
                if (String.Equals(datos[0], "Activo",
                   StringComparison.OrdinalIgnoreCase))
                {
                    usuario.DegradarPeso(hilera, "Periferico", Int32.Parse(datos[1])-3, Int32.Parse(datos[2])-3);
                    mensajeEvento = "Se ha degradado correctamente";
                    smtpClient.Send(mailMessage);
                    smtpClient.Dispose();
                }
            }                
            return RedirectToAction("UsuariosComunidad", "Usuario", new { mensaje = mensajeEvento });
        }

        public ActionResult MisArticulos(string correo) {
            ViewBag.correo = correo;
            return View();
        }


        public ActionResult AgregarIdioma(UsuarioModel model)            
        {
            if (model.Idioma[0] !="")
            {
                UsuarioService usuario = new UsuarioService();
                usuario.Agregar(model.Correo, model.Idioma[0],1);
            }
            else
            {}
            return RedirectToAction("DesplegarEditarUsuario", "Usuario",new {correo= model.Correo });
        }

        public ActionResult AgregarHobbie(UsuarioModel model)
        {
            if (model.Hobbie[0] != "")
            {
                UsuarioService usuario = new UsuarioService();
                usuario.Agregar(model.Correo, model.Hobbie[0],2);
            }
            else
            { }
            return RedirectToAction("DesplegarEditarUsuario", "Usuario", new { correo = model.Correo });
        }

        public ActionResult AgregarHabilidad(UsuarioModel model)
        {
            if (model.Habilidad[0] != "")
            {
                UsuarioService usuario = new UsuarioService();
                usuario.Agregar(model.Correo, model.Habilidad[0],3);
            }
            else
            { }
            return RedirectToAction("DesplegarEditarUsuario", "Usuario", new { correo = model.Correo });
        }


    }
}

