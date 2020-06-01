using System;
using System.Collections;
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
        public ActionResult Create(ArticuloModel articulo)
        {
            ArticuloService dbhandle = new ArticuloService();
            articulo.Autores = dbhandle.FillList();
            return View();
        }



        // POST: Articulo/Create
        [HttpPost]
        public ActionResult Create()
            {
            try
            {
                return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult CrearArticulo()
        {
            ArticuloModel art1 = new ArticuloModel(); 
            ArticuloService dbhandle = new ArticuloService();
            art1.Autores = dbhandle.FillList();
            return View(art1); 
        }

        [HttpPost]
        public ActionResult CrearArticulo(ArticuloModel articulo)
        {
            // TODO: Add insert logic here
            ArticuloService dbhandle = new ArticuloService();
            dbhandle.CrearArticulo(articulo);
            Console.Write(articulo);
            string autorValue = articulo.Autor;
            return RedirectToAction("InicioArticulos");
        }

        // GET: Articulo/Edit/5
        public ActionResult EditarArticulo(int id)
        {
            ArticuloService dbArticulo = new ArticuloService();
            ArticuloModel articulo = dbArticulo.GetInfoArticulo(id);
            if (articulo.TipoArchivo == "corto")
            {
                return RedirectToAction("EditarArticuloCorto",new { id = id });
            }
            else
            {
                return RedirectToAction("EditarArticuloLargo",new { id = id });
            }
        }

        public ActionResult EditarArticuloCorto(int id)
        {
            ArticuloService dbArticulo = new ArticuloService(); 
            return View(dbArticulo.GetInfoArticulo(id)); 
        }

        public ActionResult EditarArticuloLargo(int id)
        {
            ArticuloService dbArticulo = new ArticuloService();
            return View(dbArticulo.GetInfoArticulo(id));
        }

        public ActionResult CrearArticuloLargo()
        {
            ArticuloModel art1 = new ArticuloModel();
            ArticuloService dbhandle = new ArticuloService();
            art1.Autores = dbhandle.FillList();
            return View(art1);
        }
        [HttpPost]
        public ActionResult CrearArticuloLargo(ArticuloModel articulo)
        {
            ArticuloService dbhandle = new ArticuloService();
            dbhandle.CrearArticulo(articulo);
            Console.Write(articulo);
            string autorValue = articulo.Autor;
            return RedirectToAction("InicioArticulos");
        }


        // POST: Articulo/Edit/5
        [HttpPost]
        public ActionResult EditarArticulo(int id, ArticuloModel articulo)
        {
            try
            {
                // TODO: Add update logic here

                ArticuloService dbhandle = new ArticuloService();
                dbhandle.EditarArticulo(id, articulo);
                return RedirectToAction("InicioArticulos"); 
            }
            catch
            {
                return View();
            }
        }

        // GET: Articulo/BorrarArticulo/5
        public ActionResult BorrarArticulo(int id)
        {
            ArticuloService dbArticulo = new ArticuloService();
            return View(dbArticulo.GetInfoArticulo(id));
        }

        // POST: Articulo/BorrarArticulo/5
        [HttpPost]
        public ActionResult BorrarArticulo(int id, ArticuloModel articulo)
        {
            try
            {
                // TODO: Add delete logic here
                ArticuloService dbhandle = new ArticuloService();

                if (dbhandle.BorrarArticulo(id))
                {
                    ViewBag.AlertMsg = "Articulo Deleted Successfully";
                }
                return RedirectToAction("InicioArticulos");
            }
            catch
            {
                return RedirectToAction("InicioArticulos");
            }
        }

        // GET: Articulo/Busqueda/1
        //public ContentResult Busqueda(int id)
        public ActionResult Busqueda(string hilera)
        {
            ArticuloService dbhandle = new ArticuloService();


            /* Funciones para busquedas calidad, pero no para iteracion 1
             * 
             * List<ArticuloModel> articulosList = new List<ArticuloModel>();
            char[] delimitadores = { ',', ' ' };
            string[] strList = hilera.Split(delimitadores, System.StringSplitOptions.RemoveEmptyEntries);
            //for (int i = 0; i < strList.Length; ++i)
            //{

            articulosList = dbhandle.GetArticulos();
            List<ArticuloModel> listaEncontrados = new List<ArticuloModel>();
            string[] hileras = hilera.Split();
            foreach (string palabra in hileras) { 
                listaEncontrados.AddRange(articulosList.FindAll(smodel => smodel.Topico == hilera));
                listaEncontrados.AddRange(articulosList.FindAll(smodel => smodel.Autor == hilera));
            }

            //articulosList = articulosList.Find(smodel => smodel.ArticuloId.ToString() == hileraBusqueda);
            //articulosList.Add(dbhandle.GetArticulos().Find(smodel => smodel.Topico == hileraBusqueda));
            //articulosList.Add(dbhandle.GetArticulos().Find(smodel => smodel.Autor == hileraBusqueda));
            //articulosList.Add(dbhandle.GetArticulos().Find(smodel => smodel.Titulo == hileraBusqueda));
            //}*/
            //var noDupsList = new HashSet<ArticuloModel>(listaEncontrados).ToList();

            //char[] hilera = hileraBusqueda.ToCharArray();
            ModelState.Clear();
            //return View(noDupsList);
            return View(dbhandle.GetArticulosTopico(hilera));
        }

        public JsonResult getArticulosInfo(int pageNumber, int pageSize)
        {

            ArticuloService serviceArt = new ArticuloService();
            List<ArticuloModel> articuloList = serviceArt.GetArticuloConditional(pageNumber, pageSize);
            return Json(articuloList, JsonRequestBehavior.AllowGet);

        }


    }
}
