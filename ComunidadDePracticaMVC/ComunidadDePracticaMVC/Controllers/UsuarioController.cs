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
            return View(usuario.GetProfile(correo));
        }



        [HttpPost]
        public JsonResult EditarUsuario(UsuarioModel model)
        {

            UsuarioService usuarioDb = new UsuarioService();
            usuarioDb.EditarUsuario(model);
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
            mailMessage.Body = "Su rango fue degradado a una categoria menor.";
            mailMessage.Subject = "Fue degradado";

            SmtpClient smtpClient;

            string nombreGrupo3 = "comunidadDePracticaGrupo3@gmail.com";
            string passwordGrupo3 = "comunidadPractica3.pass";

            smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new System.Net.NetworkCredential(nombreGrupo3, passwordGrupo3);
            smtpClient.Send(mailMessage);
            smtpClient.Dispose();

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
                //MailMessage mailMessage = new MailMessage
                //("comunidadDePracticaGrupo3@gmail.com", correo.EmailTo);
                //mailMessage.IsBodyHtml = true;
                //mailMessage.Body = correo.EmailBody;
                //mailMessage.Subject = "*Insertar usuario* en Comunidad De Práctica: " + correo.Subject;



                //SmtpClient smtpClient;

                //string nombreGrupo3 = "comunidadDePracticaGrupo3@gmail.com";
                //string passwordGrupo3 = "comunidadPractica3.pass";

                //try
                //{
                //    // No need to specify the SMTP settings as these 
                //    // are already specified in web.config
                //    smtpClient = new SmtpClient("smtp.gmail.com");
                //    smtpClient.EnableSsl = true;
                //    smtpClient.UseDefaultCredentials = false;
                //    smtpClient.Port = 587;
                //    smtpClient.Credentials = new System.Net.NetworkCredential(nombreGrupo3, passwordGrupo3);
                //    smtpClient.Send(mailMessage);
                //    smtpClient.Dispose();
                //}
                
            return RedirectToAction("UsuariosComunidad", "Usuario", new { mensaje = mensajeEvento });
        }

        

        public ActionResult MisArticulos(string correo) {
            ViewBag.correo = correo;
            return View();
        }
       
    }
}