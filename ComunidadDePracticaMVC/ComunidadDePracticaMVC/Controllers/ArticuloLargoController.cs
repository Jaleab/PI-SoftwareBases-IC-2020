using ComunidadDePracticaMVC.ViewModels;
using System.Web.Mvc;
using ComunidadDePracticaMVC.Services;
using ComunidadDePracticaMVC.Models;
using System.Collections.Generic;

namespace ComunidadDePracticaMVC.Controllers
{
    public class ArticuloLargoController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            ArticuloService servicioArticulo = new ArticuloService();
            ViewBag.listaAutoresCorreos = servicioArticulo.ObtenerAutoresCorreos();
            ViewBag.listaTopicos = servicioArticulo.ObtenerTopicos();
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Guardar(ArticuloLargoViewModel model)
        {


            if (model.Titulo == null || model.Topico == null  || model.Resumen == null || model.Archivo1 == null)
            {
                ArticuloService servicioArticulo = new ArticuloService();
                ViewBag.listaAutoresCorreos = servicioArticulo.ObtenerAutoresCorreos();
                ViewBag.listaTopicos = servicioArticulo.ObtenerTopicos();
                return View("Index",model);
            }
            else
            {
                if (model.Correos == null)
                {
                    model.Correos = new List<string>();
                }
                //añade al usuario por defecto
                model.Correos.Add(User.Identity.Name);
                ArticuloService servicioArt = new ArticuloService();
                bool exito = servicioArt.GuardarArticuloLargo(model);
                if (exito)
                {
                    @TempData["Message"] = "Artículo ha sido guardado";
                    return RedirectToAction("MisArticulos", "Usuario");
                }
                else
                {
                    ViewBag.mensaje = "Articulo no ha sido guardado por titulo duplicado";
                    ViewBag.listaAutoresCorreos = servicioArt.ObtenerAutoresCorreos();
                    ViewBag.listaTopicos = servicioArt.ObtenerTopicos();
                    return View("Index", model);
                }
                
            }

        }
        [Authorize]
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
        public ActionResult Editar(int id, ArticuloModel model, string hilera)
        {
            ArticuloService servicioArticulo = new ArticuloService();
            bool exito = servicioArticulo.EditarArticuloLargo(id, model, hilera);
            if (exito)
            {
                @TempData["Message"] = "Artículo ha sido guardado";
                return RedirectToAction("MisArticulos", "Usuario");
            }
            else
            {
                ViewBag.mensaje = "Articulo no ha sido guardado por titulo duplicado";
                int articuloId = id;
                return RedirectToAction("EditarArticuloLargo", "ArticuloLargo", new { id = articuloId, mensaje = ViewBag.mensaje });
            }

            
        }

    }
}