using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComunidadDePracticaMVC.Models;
using ComunidadDePracticaMVC.Services;

namespace ComunidadDePracticaMVC.Controllers
{
    public class ChartDataController : Controller
    {
        // GET: ChartData
        public ActionResult Index(ChartDataModel data)
        {
            ChartService chartData = new ChartService();
            var labelPais = chartData.ObtenerPaises();
            var numbers = chartData.ObtenerCuentaPaises();
            
            ViewBag.LabelPais = Newtonsoft.Json.JsonConvert.SerializeObject(labelPais);
            ViewBag.DataPais = Newtonsoft.Json.JsonConvert.SerializeObject(numbers);


            return View(data);
        }        
    }
}
