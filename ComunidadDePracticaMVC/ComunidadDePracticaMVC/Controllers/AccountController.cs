using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ComunidadDePracticaMVC.Models;
using ComunidadDePracticaMVC.Services;
using System.Web.Script.Serialization;

namespace ComunidadDePracticaMVC.Controllers
{
    public class AccountController : Controller
    {

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoginAction(LoginViewModel model)
        {
            //Default result
            JsonResult result;
            AccountService accountDbHandle = new AccountService();
            if (accountDbHandle.LoginUser(model) != -1)
            {
                UsuarioService usuarioService = new UsuarioService();

                var jsonUsuario = new JavaScriptSerializer().Serialize(usuarioService.GetCookieInfo(model.Email));

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                model.Email,
                DateTime.Now,
                DateTime.Now.AddMinutes(30),
                true /*isPersistent*/,
                jsonUsuario /*userinfo*/,
                FormsAuthentication.FormsCookiePath);

                // Encrypt the ticket.
                string encTicket = FormsAuthentication.Encrypt(ticket);

                // Create the cookie.
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                result = Json(new
                {
                    
                    responseStatus = 1,
                    responseMessage = "Bienvenido " + model.Email +"!",
                    newUrl = Url.Action("Index", "Home") 
                }
                );
            }
            else
            {
                result = Json(new
                {
                    responseStatus = -1,
                    responseMessage = "Correo o contraseña incorrecta.",
                    newUrl = "" 
                }
                );
            }

            return result;
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public JsonResult RegisterAction(RegisterViewModel model)
        {
            JsonResult result;
            AccountService accountDbHandle = new AccountService();
            if (accountDbHandle.RegisterUser(model) != -1)
            {
                UsuarioService usuarioService = new UsuarioService();
                var jsonUsuario = new JavaScriptSerializer().Serialize(usuarioService.GetCookieInfo(model.Email));

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                model.Email,
                DateTime.Now,
                DateTime.Now.AddMinutes(30),
                true /*isPersistent*/,
                jsonUsuario /*userinfo*/,
                FormsAuthentication.FormsCookiePath);

                // Encrypt the ticket.
                string encTicket = FormsAuthentication.Encrypt(ticket);

                // Create the cookie.
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                result = Json(new
                    {
                        responseStatus = 1,
                        responseMessage = "Cuenta Registrada.",
                        newUrl = Url.Action("Index", "Home") 
                    }
                );
            }
            else
            {
                result = Json(new
                    {
                        responseStatus = -1,
                        responseMessage = "No se pudo registrar la cuenta, correo ya existe.",
                        newUrl = "" 
                    }
                );
            }

            
            return result;
        }

        [Authorize]
        public ActionResult MyProfile(String Name)
        {

            ViewBag.title = Name;
            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        


    }
}