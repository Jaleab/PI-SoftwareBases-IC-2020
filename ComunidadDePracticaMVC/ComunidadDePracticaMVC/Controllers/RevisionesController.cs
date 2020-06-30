
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ComunidadDePracticaMVC.Models;
using ComunidadDePracticaMVC.Services;
using Newtonsoft.Json;

namespace ComunidadDePracticaMVC.Controllers
{
    public class RevisionesController : Controller
    {
        public ActionResult ArticulosRequierenRevision()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            ViewBag.categoria = "";
            ViewBag.revisiones = new RevisionesModel();
            RevisionesService servicioRevisiones = new RevisionesService();
            if (User.Identity.IsAuthenticated)
            {
                string correo = User.Identity.Name.ToString();
                UsuarioService servicioUsuarios = new UsuarioService();
                var datos = servicioUsuarios.GetDatosMiembro(correo);
                ViewBag.categoria = datos[0];
                ViewBag.revisiones = servicioRevisiones.ObtenerArticulosEnRevision();
            }
            else
            {
                return RedirectToAction("~/AccessDenied");
            }

            return View();
        }

        [Authorize]
        public ActionResult MisArticulosRevision()
        {
            ViewBag.exitoAutorizacion = false;
            ViewBag.revisiones = new RevisionesModel();
            RevisionesService servicioRevisiones = new RevisionesService();
            if (CookieHandler.AuthorizeRole(Request.Cookies[FormsAuthentication.FormsCookieName], "Núcleo,Coordinador"))
            {
                if (TempData["Message"] != null)
                {
                    ViewBag.Message = TempData["Message"].ToString();
                    
                }
                ViewBag.Acceso = "Se pudo entrar";
                ViewBag.exitoAutorizacion = true;
                var auth = Request.Cookies[FormsAuthentication.FormsCookieName];
                string correo = FormsAuthentication.Decrypt(auth.Value).Name;
                ViewBag.correo = correo;
                ViewBag.revisiones = servicioRevisiones.ObtenerArticulosAsignadosRevisionAMiembro(correo);

            }
            else
            {
                return RedirectToAction("~/AccessDenied");
            }

            return View();
        }

        [Authorize]
        public ActionResult MisArticulosColaboracion() {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            ViewBag.categoria = "";
            return View();
        }

        [HttpGet]
        public JsonResult ArticulosAutorColaboracion(int pageNumber, int pageSize, string correo)
        {
            RevisionesService serviceRev = new RevisionesService();
            List<ArticuloModel> articuloList = serviceRev.GetSolicitudesDeColaboracion(pageNumber, pageSize, correo);
            return Json(articuloList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SolicitarColaboracion(int articuloId)
        {
            ViewBag.categoria = "";
            ViewBag.revisiones = new RevisionesModel();
            RevisionesService servicioRevisiones = new RevisionesService();
            if (User.Identity.IsAuthenticated)
            {
                string correo = User.Identity.Name.ToString();
                UsuarioService servicioUsuarios = new UsuarioService();
                var datos = servicioUsuarios.GetDatosMiembro(correo);
                ViewBag.categoria = datos[0];
                if (ViewBag.categoria == "Coordinador")
                {
                    //ejecuta metodo para enviar solicitud


                    bool exito = servicioRevisiones.PedirColaboracionATodos(articuloId);

                    if (exito)
                    {
                        @TempData["Message"] = "Se ha enviado la solicitud";
                    }
                    else
                    {
                        @TempData["Message"] = "Falló la operación, intente más tarde";
                    }
                }
                ViewBag.revisiones = servicioRevisiones.ObtenerArticulosEnRevision();

                return RedirectToAction("ArticulosRequierenRevision");
            }
            else
            {
                return RedirectToAction("ArticulosRequierenRevision");
            }
        }

        [Authorize]
        public ActionResult AceptarColaboracion(int articuloId)
        {

        
            if (CookieHandler.AuthorizeRole(Request.Cookies[FormsAuthentication.FormsCookieName], "Núcleo,Coordinador"))
            {
                RevisionesService servicioRevisiones = new RevisionesService();
                bool exito = servicioRevisiones.AceptarColaboracion(articuloId, User.Identity.Name);
                if (exito)
                {
                    @TempData["Message"] = "Se aceptó la colaboración.";
                }
                else
                {
                    @TempData["Message"] = "Falló la operación.";
                }
                return RedirectToAction("MisArticulosColaboracion");
            }
            else
            {
                return RedirectToAction("~/AccessDenied");
            }

            
        }

        [Authorize]
        public ActionResult RechazarColaboracion(int articuloId)
        {

            if (CookieHandler.AuthorizeRole(Request.Cookies[FormsAuthentication.FormsCookieName], "Núcleo,Coordinador"))
            {
                RevisionesService servicioRevisiones = new RevisionesService();
                bool exito = servicioRevisiones.RechazarColaboracion(articuloId, User.Identity.Name);
                if (exito)
                {
                    @TempData["Message"] = "Se rechazó la colaboración.";
                }
                else
                {
                    @TempData["Message"] = "Falló la operación.";
                }
                return RedirectToAction("MisArticulosColaboracion");
            }
            else
            {
                return RedirectToAction("~/AccessDenied");
            }
        }

        [Authorize]
        public ActionResult CalificarArticulo(int articuloId)
        {

            if (true /*TODO revisar si el aritculo le corresponde al calificador*/)
            {
                ArticuloService servicioArticulo = new ArticuloService();
                ModelState.Clear();
                if (User.Identity.IsAuthenticated)
                {
                    ViewBag.Articulo = servicioArticulo.GetInfoArticulo(articuloId);
                }
                return View();

            }
            else 
            {
                return RedirectToAction("~/AccessDenied");
            }
        }

        //Envia la revision del articulo
        [HttpPost]
        [Authorize]
        public ActionResult EnviarRevision(int articuloId, FormularioModel model)
        {

            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                CookieModel cookieInfo = JsonConvert.DeserializeObject<CookieModel>(authTicket.UserData);
                string correo = authTicket.Name;
                int calificacion = cookieInfo.Merito * (model.OpinionGeneral + model.Contribucion + model.Forma);

                RevisionesService servicioRevisiones = new RevisionesService();
                bool exito = servicioRevisiones.AsignarCalificacion(articuloId, correo, calificacion, model);
                if (exito)
                {
                    @TempData["Message"] = "Revisión enviada.";
                }
                else
                {
                    @TempData["Message"] = "Error al enviar la revisión.";
                }
                return RedirectToAction("MisArticulosRevision");
            }
            else
            {
                return RedirectToAction("~/AccessDenied");

            }

        }

        public ActionResult ArticulosRequierenRevisores()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            ViewBag.categoria = "";
            ViewBag.revisiones = new RevisionesModel();
            RevisionesService servicioRevisiones = new RevisionesService();
            if (User.Identity.IsAuthenticated)
            {
                string correo = User.Identity.Name.ToString();
                UsuarioService servicioUsuarios = new UsuarioService();
                var datos = servicioUsuarios.GetDatosMiembro(correo);
                ViewBag.categoria = datos[0];
                ViewBag.revisiones = servicioRevisiones.ObtenerArticulosRequierenRevisores();
            }
            return View();
        }

        public ActionResult DecidirRevisores(int articuloId)
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            ViewBag.categoria = "";
            ViewBag.revisiones = new RevisionesModel();
            RevisionesService servicioRevisiones = new RevisionesService();
            if (User.Identity.IsAuthenticated)
            {
                string correo = User.Identity.Name.ToString();
                UsuarioService servicioUsuarios = new UsuarioService();
                var datos = servicioUsuarios.GetDatosMiembro(correo);
                ViewBag.categoria = datos[0];
            }
            ViewBag.articuloId = articuloId;
            return View(servicioRevisiones.ObtenerPosiblesRevisores(articuloId));
        }

        public ActionResult DecisionRevisor(int articuloId, string hilera, string correo)
        {
            RevisionesService servicioRevisiones = new RevisionesService();
            ViewBag.articuloId = articuloId;
            if (hilera == "Aceptar")
            {
                bool exito = servicioRevisiones.AceptarRevisor(articuloId, correo);
                if (exito)
                {
                    @TempData["Message"] = "Se aceptó como revisor.";
                }
                else
                {
                    @TempData["Message"] = "Falló la operación.";
                }
            }
            else {
                if (hilera == "Rechazar") {
                    bool exito = servicioRevisiones.RechazarRevisor(articuloId, correo);
                    if (exito)
                    {
                        @TempData["Message"] = "Se rechazó como revisor.";
                    }
                    else
                    {
                        @TempData["Message"] = "Falló la operación.";
                    }
                }                 
            }            
            return RedirectToAction("DecidirRevisores", "Revisiones", new { articuloId });
        }

        public ActionResult ArticulosEnRevisionPorMiembros() {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            ViewBag.categoria = "";
            ViewBag.revisiones = new RevisionesModel();
            RevisionesService servicioRevisiones = new RevisionesService();
            if (User.Identity.IsAuthenticated)
            {
                string correo = User.Identity.Name.ToString();
                UsuarioService servicioUsuarios = new UsuarioService();
                var datos = servicioUsuarios.GetDatosMiembro(correo);
                ViewBag.categoria = datos[0];
                ViewBag.categoria = datos[0];
                ViewBag.revisiones = servicioRevisiones.ArticulosEnRevisionPorMiembros();
                ViewBag.servicioRevision = servicioRevisiones;
            }

            return View();
        }

        [Authorize]
        public ActionResult DecisionFinalArticulo(int articuloId) {

            RevisionesService servicioRevisiones = new RevisionesService();
            if (CookieHandler.AuthorizeRole(Request.Cookies[FormsAuthentication.FormsCookieName], "Coordinador"))
            {
                ArticuloService servicioArticulo = new ArticuloService();
                List<CalificacionesModel> listaRevisiones = servicioRevisiones.ObtenerCalificaciones(articuloId);
                //Calcula ponderado
                int cantRevisiones = listaRevisiones.Count;
                int calificacionTotal = 0;
                int ponderado = 0;
                if (listaRevisiones.Count > 0) {
                    foreach (CalificacionesModel Revision in listaRevisiones)
                    {
                        calificacionTotal += Revision.Calificacion;
                    }
                    ponderado = calificacionTotal / cantRevisiones;
                }
                ViewBag.Ponderado = ponderado;
                ViewBag.Calificaciones = listaRevisiones;
                ViewBag.Articulo = servicioArticulo.GetInfoArticulo(articuloId);
                
                ModelState.Clear();
                return View();
            }
            else {
                return RedirectToAction("~/AccessDenied");
            }
            
        }
        [HttpPost]
        public ActionResult DecisionFinalArticulo(int articuloId, int notaPonderada, DecisionFinalModel model) 
        {
            model.NotaFinal = notaPonderada;
            model.ArticuloId = articuloId;
            RevisionesService servicioRevision = new RevisionesService();
            bool exito = servicioRevision.DecisionFinal(model);
            if (exito)
            {
                @TempData["Message"] = "Se calificó el artículo con éxito";
            }
            else
            {
                @TempData["Message"] = "Algo salió mal, intente de nuevo más tarde";
            }
            return RedirectToAction("ArticulosEnRevisionPorMiembros");
        }

    }
}