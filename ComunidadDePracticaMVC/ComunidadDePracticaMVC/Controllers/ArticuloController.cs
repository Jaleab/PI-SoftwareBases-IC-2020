using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComunidadDePracticaMVC.Services;
using ComunidadDePracticaMVC.Models;

namespace PassParameter.Controllers

{
    public class ArticuloController : Controller
    {
        // GET: Articulo

        public ActionResult InicioArticulos()
        {
            return View();
        }

        public ActionResult Index(int id)
        {
            return View();
        }
        public ActionResult ConsultarArticulos(int id)
        {
            ViewBag.Message = "Usted está observando el resumen un artículo";
            ArticuloService servicioParaverResumen = new ArticuloService();
            ModelState.Clear();
            return View(servicioParaverResumen.GetInfoArticulo(id)); // 
        }

        // GET: Buscar por id Articulo
        public ActionResult BuscarArticuloID(int id)
        {
            ArticuloService dbhandle = new ArticuloService();
            ModelState.Clear();
            return View(dbhandle.GetInfoArticulo(id));
        }


        // GET: Articulo/Details/5
        public ActionResult Details()
        {
            return View();
        }

        // GET: Articulo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Articulo/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Articulo/Edit/5
        public ActionResult EditarArticulo(int id)
        {
            ArticuloService dbArticulo = new ArticuloService();
            return View(dbArticulo.GetInfoArticulo(id));
        }

        // POST: Articulo/Edit/5
        [HttpPost]
        public ActionResult EditarArticulo(int id, ArticuloModel articulo)
        {
            try
            {
                // TODO: Add update logic here

                ArticuloService dbhandle = new ArticuloService();
                Console.Write(articulo);
                dbhandle.EditarArticulo(id, articulo);

                return View(); 
            }
            catch
            {
                return View();
            }
        }

        // GET: Articulo/BorrarArticulo/5
        public ActionResult BorrarArticulo(int id)
        {
            return View();
        }

        // POST: Articulo/BorrarArticulo/5
        [HttpPost]
        public ActionResult BorrarArticulo(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Articulo/Busqueda/1
        //public ContentResult Busqueda(int id)
        public ActionResult Busqueda(int id)
        {
            ArticuloService dbhandle = new ArticuloService();
            ModelState.Clear();
            return View(dbhandle.GetInfoArticulo(id));
        }

        public JsonResult getArticulosInfo(int pageNumber, int pageSize)
        {

            ArticuloService serviceArt = new ArticuloService();
            List<ArticuloModel> articuloList = serviceArt.GetArticuloConditional(pageNumber, pageSize);
            return Json(articuloList, JsonRequestBehavior.AllowGet);

        }


    }
}
