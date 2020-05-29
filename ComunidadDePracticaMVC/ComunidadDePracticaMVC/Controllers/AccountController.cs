using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComunidadDePracticaMVC.Models;

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


        public ActionResult LoginAction(LoginViewModel model)
        {
            //Default result
            ActionResult result = View();
            AccountDBHandle accountDbHandle = new AccountDBHandle();
            if (accountDbHandle.loginUser(model) != -1)
            {
                //Genera cookie de autenticacion

                result = RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Text = "Invalid Username or Password";
                result = View(model);
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
        public ActionResult RegisterAction(RegisterViewModel model)
        {
            //Default result
            ActionResult result = View();
            AccountDBHandle accountDbHandle = new AccountDBHandle();
            if (accountDbHandle.registerUser(model) != -1)
            {
                //TODO hacer que diga que pudo completar la accion
                result = RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Text = "Username already exist!";
                result = View(model);
            }
            return result;
        }

        //
        // POST: /Account/Register
        
    }
}