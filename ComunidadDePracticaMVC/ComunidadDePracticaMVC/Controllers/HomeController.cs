﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComunidadDePracticaMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Busqueda()
        {
            ViewBag.Message = "Your Busqueda page.";

            return View();
        }

        public ActionResult ControlarForma()
        {
            ViewBag.Message = "Your ControlarForma page.";

            return View();
        }

        public ActionResult ConsultarArticulos()
        {
            ViewBag.Message = "Usted está observando el resumen un artículo";
            return View();
        }
    }
}