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
                ViewBag.pivotes = TempData["pivotes"];
                if (ViewBag.etiquetas != null & ViewBag.counts != null && ViewBag.pivotes != null) {
                    List<DataModel> modeloDatos = new List<DataModel>();
                    List<string> categorias = ViewBag.etiquetas[0];
                    List<int> counts = ViewBag.counts;
                    List<string> etiquetas = new List<string>();
                    for (var i = 1; i < ViewBag.etiquetas.Count; ++i)
                    {
                        foreach (var columna in ViewBag.etiquetas[i])
                        {
                            etiquetas.Add(columna);
                        }
                    }
                    modeloDatos.Add(new DataModel { categorias = categorias, etiquetas = etiquetas, counts = counts });
                    ViewBag.jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(modeloDatos);
                }
            }
            else {
                return View(filtro);
            }
            return View(filtro);
        }

        [HttpPost]
        public ActionResult Consultar(FiltroMiembroModel filtro)
        {
            FiltrarMiembroService filtrarServicio = new FiltrarMiembroService();
            List<MiembroModel> usuarios = filtrarServicio.getUsuarios();
            List<List<MiembroModel>> resultadosFiltros = new List<List<MiembroModel>>();
            List<int> dataCounts = new List<int>();
            List<List<string>> etiquetasData = new List<List<string>>();
            List<string> correosDistintos = new List<string>();

            int[] pivotes = {0,0,0,0};
            int contador = 0;

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
                    contador += 1;
                }
                etiquetasData.Add(etiquetasDataTemp);
                pivotes[0] = contador;
            }

            contador = 0;

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
                            //listaNueva.Add(resultadoBusqueda);
                            correosDistintos = resultadoBusqueda.Select(o => o.correo).Distinct().ToList();
                            if (correosDistintos.Count > 0)
                            {
                                countsDataTemp.Add(correosDistintos.Count);
                            }
                            else {
                                countsDataTemp.Add(0);
                            }
                        }
                        etiquetasDataTemp.Add(habilidad);
                        contador += 1;
                    }
                    dataCounts = countsDataTemp;
                    //resultadosFiltros = listaNueva;
                }
                else {
                    foreach (var habilidad in filtro.habilidadesSeleccionados)
                    {
                        List<MiembroModel> resultadoBusqueda = usuarios.FindAll(model => model.habilidad == habilidad);
                        resultadosFiltros.Add(resultadoBusqueda);
                        correosDistintos = resultadoBusqueda.Select(o => o.correo).Distinct().ToList();

                        dataCounts.Add(correosDistintos.Count);
                        etiquetasDataTemp.Add(habilidad);
                        contador += 1;
                    }
                }
                etiquetasData.Add(etiquetasDataTemp);
                pivotes[1] = contador;
            }

            contador = 0;

            if (filtro.hobbiesSeleccionados != null)
            {
                List<string> etiquetasDataTemp = new List<string>();
                List<int> countsDataTemp = new List<int>();
                if (resultadosFiltros.Count != 0)
                {
                    //List<List<MiembroModel>> listaNueva = new List<List<MiembroModel>>();
                    foreach (var hobby in filtro.hobbiesSeleccionados)
                    {
                        foreach (var resultado in resultadosFiltros)
                        {
                            List<MiembroModel> resultadoBusqueda = resultado.FindAll(model => model.hobbie == hobby);
                            //listaNueva.Add(resultadoBusqueda);
                            correosDistintos = resultadoBusqueda.Select(o => o.correo).Distinct().ToList();
                            if (correosDistintos.Count > 0)
                            {
                                countsDataTemp.Add(correosDistintos.Count);
                            }
                            else
                            {
                                countsDataTemp.Add(0);
                            }
                        }
                        etiquetasDataTemp.Add(hobby);
                        contador += 1;
                    }
                    foreach (int valor in countsDataTemp) {
                        dataCounts.Add(valor);
                    }
                    //resultadosFiltros = listaNueva;
                }
                else
                {
                    foreach (var hobby in filtro.hobbiesSeleccionados)
                    {
                        List<MiembroModel> resultadoBusqueda = usuarios.FindAll(model => model.hobbie == hobby);
                        resultadosFiltros.Add(resultadoBusqueda);
                        correosDistintos = resultadoBusqueda.Select(o => o.correo).Distinct().ToList();

                        dataCounts.Add(correosDistintos.Count);
                        etiquetasDataTemp.Add(hobby);
                        contador += 1;
                    }
                }
                //dataCounts = countsDataTemp;
                etiquetasData.Add(etiquetasDataTemp);
                pivotes[2] = contador;
            }

            if (filtro.idiomasSeleccionados != null)
            {
                List<string> etiquetasDataTemp = new List<string>();
                List<int> countsDataTemp = new List<int>();
                if (resultadosFiltros.Count != 0)
                {
                    //List<List<MiembroModel>> listaNueva = new List<List<MiembroModel>>();
                    foreach (var idioma in filtro.idiomasSeleccionados)
                    {
                        foreach (var resultado in resultadosFiltros)
                        {
                            List<MiembroModel> resultadoBusqueda = resultado.FindAll(model => model.idioma == idioma);
                            //listaNueva.Add(resultadoBusqueda);
                            correosDistintos = resultadoBusqueda.Select(o => o.correo).Distinct().ToList();
                            if (correosDistintos.Count > 0)
                            {
                                countsDataTemp.Add(correosDistintos.Count);
                            }
                            else
                            {
                                countsDataTemp.Add(0);
                            }
                        }
                        etiquetasDataTemp.Add(idioma);
                        contador += 1;
                    }
                    foreach (int valor in countsDataTemp)
                    {
                        dataCounts.Add(valor);
                    }
                    //resultadosFiltros = listaNueva;
                }
                else
                {
                    foreach (var idioma in filtro.idiomasSeleccionados)
                    {
                        List<MiembroModel> resultadoBusqueda = usuarios.FindAll(model => model.idioma == idioma);
                        resultadosFiltros.Add(resultadoBusqueda);
                        correosDistintos = resultadoBusqueda.Select(o => o.correo).Distinct().ToList();

                        dataCounts.Add(correosDistintos.Count);
                        etiquetasDataTemp.Add(idioma);
                        contador += 1;
                    }
                }
                //dataCounts = countsDataTemp;
                etiquetasData.Add(etiquetasDataTemp);
                pivotes[2] = contador;
            }

            if (resultadosFiltros.Count == 0) {
                return RedirectToAction("FiltrarMiembro", "FiltrarMiembro", new {graficado = 0});
            }
            else
            {
                @TempData["etiquetas"] = etiquetasData;
                @TempData["counts"] = dataCounts;
                @TempData["pivotes"] = pivotes;
                return RedirectToAction("FiltrarMiembro", "FiltrarMiembro", new { graficado = 1});
            }
        }

    }
}