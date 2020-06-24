using System;
using System.Collections.Generic;
using System.Linq;
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

        //
        // GET: /Account/Login
        //[AllowAnonymous]

        [HttpPost]
        public JsonResult LoginAction(LoginViewModel model)
        {
            //Default result
            JsonResult result;
            AccountDBHandle accountDbHandle = new AccountDBHandle();
            if (accountDbHandle.loginUser(model) != -1)
            {
                UsuarioService usuarioService = new UsuarioService();
                //TODO get ROL de USUARIO
                var jsonUsuario = new JavaScriptSerializer().Serialize(usuarioService.GetCookieInfo(model.Email));

                //FormsAuthentication.SetAuthCookie(jsonUsuario, true);
                //FormsAuthentication.SetAuthCookie(model.Email, true);

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

                //TODO hacer que diga que pudo completar la accion
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
            //ViewBag.ReturnUrl = returnUrl;
            return result;
        }

        public ActionResult loginPRUEBA(/*string returnUrl*/)
        {
            //CREA COOKIE
            HttpCookie userInfo = new HttpCookie("userInfo");
            userInfo["UserName"] = "Annathurai";
            userInfo["UserColor"] = "Black";
            //expira en un minuto
            userInfo.Expires.Add(new TimeSpan(0, 1, 0));
            Response.Cookies.Add(userInfo);
            //la elimina en un minuto
            userInfo.Expires = DateTime.Now.AddMinutes(1);
            //ViewBag.ReturnUrl = returnUrl;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }
        //
        // POST: /Account/Login


        //
        // GET: /Account/Register
        //[AllowAnonymous]
        [HttpPost]
        public JsonResult RegisterAction(RegisterViewModel model)
        {
            JsonResult result;
            AccountDBHandle accountDbHandle = new AccountDBHandle();
            if (accountDbHandle.registerUser(model) != -1)
            {
                UsuarioService usuarioService = new UsuarioService();
                //TODO get ROL de USUARIO
                var jsonUsuario = new JavaScriptSerializer().Serialize(usuarioService.GetCookieInfo(model.Email));

                //FormsAuthentication.SetAuthCookie(jsonUsuario, true);
                //FormsAuthentication.SetAuthCookie(model.Email, true);

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


                //TODO hacer que diga que pudo completar la accion
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

        //
        // POST: /Account/Register
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