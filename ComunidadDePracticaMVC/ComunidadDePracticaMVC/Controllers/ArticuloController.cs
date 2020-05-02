using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ComunidadDePracticaMVC.Models;
using ComunidadDePracticaMVC.Services;

namespace ComunidadDePracticaMVC.Controllers
{
    public class ArticuloController : Controller
    {
        // GET: Articulo
        public ActionResult Index()
        {
            ArticuloService dbArticulo = new ArticuloService();
            ModelState.Clear();
            return View(dbArticulo.GetArticulos());
        }

        // GET: Articulo/Details/5
        public ActionResult Details(int id)
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
        public ActionResult Create(ArticuloModel articulo)
        {
            try
            {
                // TODO: Add insert logic here
                if( ModelState.IsValid)
                {
                    ArticuloService dbArticulo = new ArticuloService();
                    if( dbArticulo.AddArticulo(articulo))
                    {
                        ViewBag.Message = "Articulo added successfully"; 
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Articulo/Edit/5
        [Route("/Edit/{id}")]
        public ActionResult Edit(int id)
        {
            ArticuloService dbArticulo = new ArticuloService();
            return View(dbArticulo.GetArticulos().Find(articulo => articulo.ArticuloId == id));
        }

        // POST: Articulo/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ArticuloModel articulo)
        {
            try
            {
                ArticuloService dbArticulo = new ArticuloService();
                dbArticulo.CambiarArticuloContenido(articulo); 

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Articulo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Articulo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, ArticuloModel articulo)
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
    }
}
