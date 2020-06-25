using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ComunidadDePracticaMVC.Models;
using ComunidadDePracticaMVC.Services;

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
            return View();
        }

        [Authorize]
        public ActionResult MisArticulosRevision()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            ViewBag.categoria = "";
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

        
            if (AuthorizeClass.AuthorizeRole(Request.Cookies[FormsAuthentication.FormsCookieName], "Núcleo,Coordinador"))
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
                return RedirectToAction("MisArticulosRevision");
            }
            else
            {
                return new RedirectResult("~/AccessDenied");
            }

            
        }

        [Authorize]
        public ActionResult RechazarColaboracion(int articuloId)
        {

            if (AuthorizeClass.AuthorizeRole(Request.Cookies[FormsAuthentication.FormsCookieName], "Núcleo,Coordinador"))
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
                return RedirectToAction("MisArticulosRevision");
            }
            else
            {
                return new RedirectResult("~/AccessDenied");
            }
        }

        [Authorize]
        public ActionResult CalificarArticulo(int id)
        {

            if (true /*TODO revisar si el aritculo le corresponde al calificador*/)
            {
                ViewBag.Message = "Usted está visitando un artículo";
                ViewBag.Reaccion = 2;
                ArticuloService servicioArticulo = new ArticuloService();
                UsuarioService servicioUsuarios = new UsuarioService();
                servicioArticulo.AumentarVisitas(id);
                ModelState.Clear();
                var articuloModel = servicioArticulo.GetInfoArticulo(id);
                if (User.Identity.IsAuthenticated)
                {
                    string correo = User.Identity.Name.ToString();
                    ViewBag.Reaccion = servicioUsuarios.ReaccionDeUsuario(correo, id);
                }
                return View(articuloModel);

            }
            else 
            {
                return new RedirectResult("~/AccessDenied");
            }

            
        }

    }
}