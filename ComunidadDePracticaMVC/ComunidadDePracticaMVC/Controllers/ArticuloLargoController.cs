using ComunidadDePracticaMVC.ViewModels;
using ComunidadDePracticaMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Mvc;
using ComunidadDePracticaMVC.Services;
using System.Data;
using System.Data.SqlClient;

namespace ComunidadDePracticaMVC.Controllers
{
    public class ArticuloLargoController : Controller
    {
        public ActionResult Index()
        {
            ArticuloService servicioArticulo = new ArticuloService();
            ViewBag.listaAutoresCorreos = servicioArticulo.ObtenerAutoresCorreos();
            ViewBag.listaTopicos = servicioArticulo.ObtenerTopicos();
            return View();
        }

        [HttpPost]
        public ActionResult Guardar(ArticuloLargoViewModel model)
        {

            if (model.Titulo == null || model.Topico == null || model.Correos == null || model.Resumen == null || model.Archivo1 == null)
            {
                ArticuloService servicioArticulo = new ArticuloService();
                ViewBag.listaAutoresCorreos = servicioArticulo.ObtenerAutoresCorreos();
                ViewBag.listaTopicos = servicioArticulo.ObtenerTopicos();
                return View("Index",model);
            }
            else
            {
                ArticuloService servicioArt = new ArticuloService();
                bool exito = servicioArt.GuardarArticuloLargo(model);
                if (exito)
                {
                    ViewBag.mensaje = "Artículo ha sido guardado";
                }
                else
                {
                    ViewBag.mensaje = "Articulo no ha sido guardado por titulo duplicado";
                }
                ViewBag.listaAutoresCorreos = servicioArt.ObtenerAutoresCorreos();
                ViewBag.listaTopicos = servicioArt.ObtenerTopicos();
                return View("Index", model);
            }

        }

        public ActionResult EditarArticuloLargo(int id, string mensaje)
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            else
            {
                ViewBag.mensaje = mensaje;
            }
            ArticuloService servicioArticulo = new ArticuloService();
            ViewBag.listaAutoresCorreos = servicioArticulo.ObtenerAutoresCorreos();
            ViewBag.listaTopicos = servicioArticulo.ObtenerTopicos();
            return View(servicioArticulo.GetInfoArticulo(id));
        }

        [HttpPost]
        public ActionResult Editar(int id, ArticuloLargoViewModel model, string hilera)
        {
            ArticuloService servicioArticulo = new ArticuloService();
            bool exito = servicioArticulo.EditarArticuloLargo(id, model, hilera);
            if (exito == true)
            {
                ViewBag.mensaje = "Artículo ha sido guardado";
            }
            else
            {
                ViewBag.mensaje = "Articulo no ha sido guardado por titulo duplicado";
            }

            int articuloId = id;
            //return View(new { id = articuloId });
            return RedirectToAction("EditarArticuloLargo", "ArticuloLargo", new { id = articuloId, mensaje = ViewBag.mensaje });
        }

    }
}