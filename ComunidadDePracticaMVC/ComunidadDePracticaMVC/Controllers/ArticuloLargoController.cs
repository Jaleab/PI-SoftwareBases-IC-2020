using ComunidadDePracticaMVC.ViewModels;
using ComunidadDePracticaMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComunidadDePracticaMVC.Services;

namespace ComunidadDePracticaMVC.Controllers
{
    public class ArticuloLargoController : Controller
    {
        // GET: ArticuloLargo
        public ActionResult Index()
        {
            if (TempData["Message"] != null) {
                ViewBag.Message = TempData["Message"].ToString();
            }
            ArticuloService servicioArticulo = new ArticuloService();
            ViewBag.listaAutoresCorreos = servicioArticulo.ObtenerAutoresCorreos();
            ViewBag.listaTopicos = servicioArticulo.ObtenerTopicos();
            return View();
        }

        [HttpPost]
        public ActionResult Guardar(ArticuloLargoViewModel model) {

            if (model == null)
            {                
                ArticuloService servicioArticulo = new ArticuloService();
                ViewBag.listaAutoresCorreos = servicioArticulo.ObtenerAutoresCorreos();
                ViewBag.listaTopicos = servicioArticulo.ObtenerTopicos();
                return View("Index", "ArticuloLargo");
            }
            else {
                string RutaSitio = Server.MapPath("~/"); //guardar local
                string PathArchivo1 = Path.Combine(RutaSitio + "/Files/") + model.Archivo1.FileName; //guardar local
                model.Archivo1.SaveAs(PathArchivo1);
                ArticuloService servicioArt = new ArticuloService();
                servicioArt.GuardarArticuloLargo(model);
                //@TempData["Message"] = "Su archivo ha sido enviado a revisión";           
                ViewBag.mensaje = "Su archivo ha sido enviado a revisión";
                ViewBag.listaAutoresCorreos = servicioArt.ObtenerAutoresCorreos();
                ViewBag.listaTopicos = servicioArt.ObtenerTopicos();
                return View("Index");
            }

        }

    }
}