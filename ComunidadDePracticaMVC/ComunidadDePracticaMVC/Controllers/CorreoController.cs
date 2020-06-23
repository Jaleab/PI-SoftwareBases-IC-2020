using ComunidadDePracticaMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Web.Helpers;

namespace ComunidadDePracticaMVC.Controllers
{
    public class CorreoController : Controller
    {
        // GET: Correo
        public ActionResult Index()
        {

            return View();
        }

        public JsonResult EnviarCorreo(CorreoModel correo) {
            // Specify the from and to email address
            MailMessage mailMessage = new MailMessage
                ("comunidadDePracticaGrupo3@gmail.com", correo.EmailTo);
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "<div>"+ correo.EmailBody + "</div>"+"<p> -Enviado desde Comunidad de Práctica Grupo 3</p>";
            mailMessage.Subject = correo.Sender + " en Comunidad De Práctica: " + correo.Subject;
            
            SmtpClient smtpClient;

            string nombreGrupo3 = "comunidadDePracticaGrupo3@gmail.com";
            string passwordGrupo3 = "comunidadPractica3.pass";

            try
            {
                // No need to specify the SMTP settings as these 
                // are already specified in web.config
                smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential(nombreGrupo3, passwordGrupo3);
                smtpClient.Send(mailMessage);
                smtpClient.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                    ex.ToString());
                JsonResult errorMessage = Json(new
                {
                    to = correo.EmailTo,
                    subject = correo.Subject,
                    body = correo.EmailBody,
                    returnMessage = "Error: Correo no pudo ser enviado.",
                    newUrl = Url.Action("Index", "Home")
                }
                );
                return errorMessage;
            }
           
            JsonResult result = Json(new
            {
                to = correo.EmailTo,
                subject = correo.Subject,
                body = correo.EmailBody,
                responseMessage = "Correo enviado.",
                newUrl = Url.Action("Index", "Home")
            }
                );
            return result;
        }
    }
}