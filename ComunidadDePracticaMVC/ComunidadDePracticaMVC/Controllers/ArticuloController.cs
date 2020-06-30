using ComunidadDePracticaMVC.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using ComunidadDePracticaMVC.Services;

namespace ComunidadDePracticaMVC.Controllers

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
            ViewBag.Reaccion = 2;
            ArticuloService servicioParaverResumen = new ArticuloService();
            UsuarioService servicioUsuarios = new UsuarioService();
            servicioParaverResumen.AumentarVisitas(id);
            ModelState.Clear();
            var articuloModel = servicioParaverResumen.GetInfoArticulo(id);
            if (User.Identity.IsAuthenticated) {
                string correo = User.Identity.Name.ToString();
                ViewBag.Reaccion = servicioUsuarios.ReaccionDeUsuario(correo,id);
            }
            return View(articuloModel);
        }

        // GET: Buscar por id Articulo
        public ActionResult BuscarArticuloID(int id)
        {
            ArticuloService dbhandle = new ArticuloService();
            ModelState.Clear();
            return View(dbhandle.GetInfoArticulo(id));
        }


        // GET: Articulo/Details/5
        public FileResult AccederArticuloLargo(int idArt)
        {
            ArticuloService hdb = new ArticuloService();
            var tupla=hdb.DescargarArticuloLargo(idArt);
            return File(tupla.Item1,tupla.Item2);
        }

        // GET: Articulo/Create
        public ActionResult Create()
        {
            ArticuloService servicioArt = new ArticuloService();
            ViewBag.listaAutoresCorreos = servicioArt.ObtenerAutoresCorreos();
            ViewBag.listaTopicos = servicioArt.ObtenerTopicos();                       
            return View();
        }

        [HttpPost]
        public ActionResult Create(ArticuloModel model)
        {
            if (model.Titulo == null || model.Topico == null || model.Correos == null || model.Resumen == null || model.Contenido == null)
            {
                ArticuloService servicioArt = new ArticuloService();
                ViewBag.listaAutoresCorreos = servicioArt.ObtenerAutoresCorreos();
                ViewBag.listaTopicos = servicioArt.ObtenerTopicos();
                return View();
            }
            else
            {
                ArticuloService servicioArt = new ArticuloService();
                bool exito = servicioArt.CrearArticulo(model);
                if (exito)
                {
                    ViewBag.mensaje = "Artículo ha sido guardado";
                }
                else {
                    ViewBag.mensaje = "Articulo no ha sido guardado por titulo duplicado";
                }
                ViewBag.listaAutoresCorreos = servicioArt.ObtenerAutoresCorreos();
                ViewBag.listaTopicos = servicioArt.ObtenerTopicos();
                return View();
            }
        }

        public ActionResult EditarArticulos(int id)
        {
            ViewBag.Message = "Usted está editando un artículo";
            ArticuloService articuloService = new ArticuloService();
            ModelState.Clear();
            ArticuloModel articuloModel = articuloService.GetInfoArticulo(id);
            if (articuloModel.TipoArchivo == "corto")
            {
                int articuloId = id;
                return RedirectToAction("EditarArticuloCorto", "Articulo", new { id = articuloModel.ArticuloId });
            }
            else
            {
                return RedirectToAction("EditarArticuloLargo", "ArticuloLargo", new { id = articuloModel.ArticuloId });                
            }
        }

        public ActionResult EditarArticuloCorto(int id, string mensaje)
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
            bool exito = servicioArticulo.EditarArticuloCorto(id, model, hilera);
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
            return RedirectToAction("EditarArticuloCorto", "Articulo", new { id = articuloId, mensaje = ViewBag.mensaje});
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
            ViewBag.busqueda = hilera;
            //return View(noDupsList);
            return View(dbhandle.GetArticulosTopico(hilera));
        }

        public JsonResult getArticulosInfo(int pageNumber, int pageSize)
        {

            ArticuloService serviceArt = new ArticuloService();
            List<ArticuloModel> articuloList = serviceArt.GetArticuloConditional(pageNumber, pageSize);
            return Json(articuloList, JsonRequestBehavior.AllowGet);

        }

        public JsonResult ArticulosAutor(int pageNumber, int pageSize, string correo)
        {

            ArticuloService serviceArt = new ArticuloService();
            List<ArticuloModel> articuloList = serviceArt.GetArticulosByAutor(pageNumber, pageSize,correo);
            return Json(articuloList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Puntuar(int id, int puntaje, string correo, int reaccionVieja)
        {
            UsuarioService usuarioServ = new UsuarioService();
            usuarioServ.UsuarioReacciona(correo, id, puntaje);
            //reaccionaVieja solo se estaba enviando para debuggear
            JsonResult result = Json(new
            {
                message = "Gracias por brindar su opinión "
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

        public JsonResult EnviarRevision(int id) {
            JsonResult result;
            try
            {
                ArticuloService artServ = new ArticuloService();
                artServ.PonerEnRevision(id);
                result = Json(new
                {
                    message = "Se envio a revisar."
                }, JsonRequestBehavior.AllowGet);
            }
            catch {
                result = Json(new
                {
                    message  = "No se pudo enviar a revisar."
                }, JsonRequestBehavior.AllowGet);
            }
            return result;
        }

    }
}
