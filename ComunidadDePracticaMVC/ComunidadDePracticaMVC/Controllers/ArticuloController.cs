using ComunidadDePracticaMVC.ViewModels;
using ComunidadDePracticaMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComunidadDePracticaMVC.Services;

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
            ViewBag.Message = "Usted está visitando un artículo";
            ArticuloService servicioParaverResumen = new ArticuloService();
            servicioParaverResumen.AumentarVisitas(id);
            ModelState.Clear();
            return View(servicioParaverResumen.GetInfoArticulo(id));  
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
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(ArticuloModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }
            else
            {
                ArticuloService servicioArt = new ArticuloService();
                @TempData["Message"] = "Artículo enviado a revisión";
                servicioArt.CrearArticulo(model);
                return RedirectToAction("Create");
            }
        }

        public ActionResult EditarArticuloCorto(int id)
        {
            ArticuloService dbArticulo = new ArticuloService();
            return View(dbArticulo.GetInfoArticulo(id));
        }

        [HttpPost]
        public ActionResult Editar(int id, ArticuloModel model)
        {
            ArticuloService servicioArticulo = new ArticuloService();
            servicioArticulo.EditarArticuloCorto(id, model);

            int articuloId = id;
            ViewBag.mensaje = "Se cargaron los archivos";
            return RedirectToAction("EditarArticuloCorto", "Articulo", new { id = articuloId});
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

        public JsonResult puntuar(int id, int puntaje)
        {
            ArticuloService artServ = new ArticuloService();
            artServ.modificarLikes(id, puntaje);

            JsonResult result = Json(new
            {
                message = "Se modifico los likes de articulo " + id.ToString()
            },  JsonRequestBehavior.AllowGet);;
            return result;
        }

        public JsonResult AsignarMeritos(int id, int merito)
        {
            ArticuloService artServ = new ArticuloService();
            artServ.AumentarMeritoPorCalificacion(id, merito);

            JsonResult result = Json(new
            {
                message = "Usted a enviado un merito de " + merito.ToString() + " al articulo"
            }, JsonRequestBehavior.AllowGet); ;
            return result;
        }

        public ActionResult ArticulosCalificados() {
            return View();
        }

    }
}
