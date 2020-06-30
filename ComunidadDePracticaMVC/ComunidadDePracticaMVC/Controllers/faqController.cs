using ComunidadDePracticaMVC.Models;
using ComunidadDePracticaMVC.Services;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ComunidadDePracticaMVC.Controllers
{
    public class FaqController : Controller
    {
        // GET: faq
        public ActionResult Faq()
        {
            questionService dbhandle = new questionService();
            ModelState.Clear();
            return View(dbhandle.GetQuestions());
        }

        // GET: faq/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: faq/Create
        [HttpPost]
        public ActionResult Create(string preguntaStr, string respuestaStr)
        {
            try
            {
                FaqModel pregunta = new FaqModel();
                pregunta.Pregunta = preguntaStr;
                pregunta.Respuesta = respuestaStr;
                questionService serviceFaq = new questionService();
                if (serviceFaq.AddQuestion(pregunta))
                {
                    ViewBag.Message = "Question Added Successfully";
                    ModelState.Clear();
                }
                
                return RedirectToAction("faq");

            }
            catch
            {
                return View();
            }
        }

        // GET: faq/Edit/5
        public JsonResult Edit(int id)
        {
            questionService serviceFaq = new questionService();
            List<FaqModel> articuloList = serviceFaq.GetQuestions();
            return Json(articuloList, JsonRequestBehavior.AllowGet);
        }

        // POST: faq/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, string preguntaStr, string respuestaStr)
        {
            try
            {
                FaqModel pregunta = new FaqModel();
                pregunta.Id = id;
                pregunta.Pregunta = preguntaStr;
                pregunta.Respuesta = respuestaStr;
                questionService serviceFaq = new questionService();
                serviceFaq.UpdateQuestion(pregunta);
                return RedirectToAction("faq");
            }
            catch
            {
                return View();
            }
        }

        // POST: faq/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                questionService serviceFaq = new questionService();
                if (serviceFaq.DeleteQuestion(id))
                {
                    ViewBag.AlertMsg = "Question Deleted Successfully";
                }
                return RedirectToAction("faq");
            }
            catch
            {
                return View();
            }
        }
    }
}
