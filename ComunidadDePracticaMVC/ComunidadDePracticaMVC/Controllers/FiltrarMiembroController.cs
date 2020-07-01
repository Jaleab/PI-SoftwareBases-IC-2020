using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ComunidadDePracticaMVC.Models;
using ComunidadDePracticaMVC.Services;

namespace ComunidadDePracticaMVC.Controllers
{
    public class FiltrarMiembroController : Controller
    {
        // GET: FiltrarMiembro
        //, List<List<string>> etiquetas, List<int> counts )
        public ActionResult FiltrarMiembro(FiltroMiembroModel filtro, int? graficado)
        {
            FiltrarMiembroService filtrarServicio = new FiltrarMiembroService();
            ViewBag.listaPaises = filtrarServicio.getPaises();
            ViewBag.listaIdiomas = filtrarServicio.getIdiomas();
            ViewBag.listaHabilidades = filtrarServicio.getHabilidades();
            ViewBag.listaHobbies = filtrarServicio.getHobbies();

            ViewBag.graficado = graficado;
            if (graficado == 1)
            {
                ViewBag.etiquetas = TempData["etiquetas"];
                ViewBag.counts = TempData["counts"];
                List<DataModel> modeloDatos = new List<DataModel>();
                List<string> categorias = ViewBag.etiquetas[0];
                List<int> counts = ViewBag.counts;
                List<string> etiquetas = new List<string>();
                for (var i = 1; i < ViewBag.etiquetas.Count; ++i) {
                    foreach (var columna in ViewBag.etiquetas[i]) {
                        etiquetas.Add(columna);
                    }
                }
                modeloDatos.Add(new DataModel { categorias = categorias, etiquetas = etiquetas, counts = counts});
                ViewBag.jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(modeloDatos);
            }
            return View(filtro);
        }

        [HttpPost]
        public ActionResult Consultar(FiltroMiembroModel filtro)
        {
            FiltrarMiembroService filtrarServicio = new FiltrarMiembroService();
            List<MiembroModel> usuarios = filtrarServicio.getUsuarios();

            //int n = 0;
            //int r = 0;
            //// operadores ternarios intente, pero no me salio como en c++ con el +=
            //if (filtro.paisesseleccionados.count > 0) {
            //    n += 1;
            //    r += filtro.paisesseleccionados.count;
            //}
            //if (filtro.idiomasseleccionados.count > 0)
            //{
            //    n += 1;
            //    r += filtro.idiomasseleccionados.count;
            //}
            //if (filtro.hobbiesseleccionados.count > 0)
            //{
            //    n += 1;
            //    r += filtro.hobbiesseleccionados.count;
            //}
            //if (filtro.habilidadesseleccionados.count > 0)
            //{
            //    n += 1;
            //    r += filtro.habilidadesseleccionados.count;
            //}

            int r = 0; // nCr. n es el count de data y r diferentes tipos de filtros. Ejemplo r = 2 si filtro es idioma y pais
            List<List<MiembroModel>> resultadosFiltros = new List<List<MiembroModel>>();
            List<int> dataCounts = new List<int>();
            List<List<string>> etiquetasData = new List<List<string>>();
            List<string> correosDistintos = new List<string>();

            if (filtro.paisesSeleccionados != null)
            {
                List<string> etiquetasDataTemp = new List<string>();
                foreach (var pais in filtro.paisesSeleccionados)
                {
                    List<MiembroModel> resultadoBusqueda = usuarios.FindAll(model => model.pais == pais);
                    resultadosFiltros.Add(resultadoBusqueda);
                    correosDistintos = resultadoBusqueda.Select(o => o.correo).Distinct().ToList();

                    dataCounts.Add(correosDistintos.Count);
                    etiquetasDataTemp.Add(pais);                    
                }
                r += 1;
                etiquetasData.Add(etiquetasDataTemp);
            }

            if (filtro.habilidadesSeleccionados != null)
            {
                List<string> etiquetasDataTemp = new List<string>();
                List<int> countsDataTemp = new List<int>();
                if (resultadosFiltros.Count != 0) {
                    List<List<MiembroModel>> listaNueva = new List<List<MiembroModel>>();
                    foreach (var habilidad in filtro.habilidadesSeleccionados)
                    {
                        foreach (var resultado in resultadosFiltros) {
                            List<MiembroModel> resultadoBusqueda = resultado.FindAll(model => model.habilidad == habilidad);
                            listaNueva.Add(resultadoBusqueda);
                            correosDistintos = resultadoBusqueda.Select(o => o.correo).Distinct().ToList();

                            countsDataTemp.Add(correosDistintos.Count);
                        }
                        etiquetasDataTemp.Add(habilidad);
                    }
                    resultadosFiltros = listaNueva;
                }
                else {
                    foreach (var habilidad in filtro.habilidadesSeleccionados)
                    {
                        List<MiembroModel> resultadoBusqueda = usuarios.FindAll(model => model.habilidad == habilidad);
                        resultadosFiltros.Add(resultadoBusqueda);
                        correosDistintos = resultadoBusqueda.Select(o => o.correo).Distinct().ToList();

                        dataCounts.Add(correosDistintos.Count);
                        etiquetasDataTemp.Add(habilidad);
                    }
                }
                r += 1;
                dataCounts = countsDataTemp;
                etiquetasData.Add(etiquetasDataTemp);
            }

            if (resultadosFiltros.Count == 0) {
                return RedirectToAction("FiltrarMiembro", "FiltrarMiembro", new {graficado = 0});
            }
            else
            {
                @TempData["etiquetas"] = etiquetasData;
                @TempData["counts"] = dataCounts;
                return RedirectToAction("FiltrarMiembro", "FiltrarMiembro", new { graficado = 1});
            }



            //if (filtro.paisSeleccionado != null)
            //    foreach (var pais in filtro.paisesSeleccionados) {
            //        filterResults.AddRange(usuarios.FindAll(model => model.pais == pais));
            //    }
            //if (filtro.habilidadSeleccionado != null)
            //    filterResults.AddRange(usuarios.FindAll(model => model.habilidad == filtro.habilidadSeleccionado));
            //if (filtro.hobbieSeleccionado != null)
            //    filterResults.AddRange(usuarios.FindAll(model => model.hobbie == filtro.hobbieSeleccionado));
            //if (filtro.idiomaSeleccionado != null)
            //    filterResults.AddRange(usuarios.FindAll(model => model.idioma == filtro.idiomaSeleccionado));

            //var noDupsList = new HashSet<MiembroModel>(resultadosFiltros).ToList();  

            return View(resultadosFiltros[0]); 
            /*FiltrarMiembroService filtrarServicio = new FiltrarMiembroService();
            List<MiembroModel> usuarios = filtrarServicio.GetDatosPersonalesByFiltro(filtro);

            return View(usuarios);*/

        }

    }
}