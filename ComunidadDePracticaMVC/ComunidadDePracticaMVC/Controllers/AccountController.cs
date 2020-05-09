using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComunidadDePracticaMVC.Controllers
{
    public class AccountController : Controller
    {

        //
        // GET: /Account/Login
        //[AllowAnonymous]
        public ActionResult Login(/*string returnUrl*/)
        {
            //ViewBag.ReturnUrl = returnUrl;
            return View();
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


        //
        // POST: /Account/Login


        //
        // GET: /Account/Register
        //[AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        
    }
}