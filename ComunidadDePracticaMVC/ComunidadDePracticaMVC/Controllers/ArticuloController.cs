using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComunidadDePracticaMVC.Services;
using ComunidadDePracticaMVC.Models;

namespace ComunidadDePracticaMVC.Controllers
{
    public class ArticuloController : Controller
    {
        // GET: Articulo
        public ActionResult InicioArticulos()
        {
            return View();
        }
        public ActionResult ConsultarArticulos()
        {
            ViewBag.Message = "Usted está observando el resumen un artículo";
            ArticuloService servicioArticulo = new ArticuloService();
            ArticuloModel modeloArticulo = servicioArticulo.GetInfoArticulo(1); // consulta el articulo con id=1 (debe ser un id generico)
            return View(modeloArticulo);
        }

    }
}
