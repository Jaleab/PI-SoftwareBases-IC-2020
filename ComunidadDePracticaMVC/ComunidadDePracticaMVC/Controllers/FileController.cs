   using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ComunidadDePracticaMVC.Models;
using ComunidadDePracticaMVC.Services;

namespace ComunidadDePracticaMVC.Controllers
{
    public class FileController : Controller
    {
        FileService archivoService = new FileService();
        // GET: File
        public ActionResult Index()
        {
            return View(archivoService.GetFiles());
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            archivoService.SubirArchivo(postedFile);
            return View(archivoService.GetFiles());
        }

        
    }
}