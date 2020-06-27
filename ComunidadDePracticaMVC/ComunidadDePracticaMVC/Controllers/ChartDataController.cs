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
            var numbersPais = chartData.ObtenerCuentaPaises();
            
            ViewBag.LabelPais = Newtonsoft.Json.JsonConvert.SerializeObject(labelPais);
            ViewBag.DataPais = Newtonsoft.Json.JsonConvert.SerializeObject(numbersPais);

            var labelHabilidad = chartData.ObtenerHabilidades();
            var numbersHabilidad = chartData.ObtenerCuentaHabilidades();

            ViewBag.LabelHabilidad = Newtonsoft.Json.JsonConvert.SerializeObject(labelHabilidad);
            ViewBag.DataHabilidad = Newtonsoft.Json.JsonConvert.SerializeObject(numbersHabilidad);

            var labelHobbie = chartData.ObtenerHobbies();
            var numbersHobbie = chartData.ObtenerCuentaHobbies();

            ViewBag.LabelHobbie = Newtonsoft.Json.JsonConvert.SerializeObject(labelHobbie);
            ViewBag.DataHobbie = Newtonsoft.Json.JsonConvert.SerializeObject(numbersHobbie);

            var labelIdioma = chartData.ObtenerIdiomas();
            var numbersIdioma = chartData.ObtenerCuentaIdiomas();

            ViewBag.LabelIdioma = Newtonsoft.Json.JsonConvert.SerializeObject(labelIdioma);
            ViewBag.DataIdioma = Newtonsoft.Json.JsonConvert.SerializeObject(numbersIdioma);


            return View(data);
        }        
    }
}
