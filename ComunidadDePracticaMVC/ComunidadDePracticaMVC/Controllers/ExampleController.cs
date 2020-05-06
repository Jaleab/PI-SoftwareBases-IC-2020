using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ComunidadDePracticaMVC.Models;

namespace ComunidadDePracticaMVC.Controllers
{
    public class ExampleController : Controller
    {
        // GET: Example
        public ActionResult Index()
        {

            StudentDBHandle dbhandle = new StudentDBHandle();
            ModelState.Clear();
            return View(dbhandle.GetStudent());
        }

        public JsonResult getStudentInfo(int pageNumber, int pageSize)
        {

            StudentDBHandle dbhandle = new StudentDBHandle();
            List<StudentExample> studentList = dbhandle.GetStudentConditional(pageNumber, pageSize);
            
            return Json(studentList, JsonRequestBehavior.AllowGet); 

        }


        // GET: Example/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Example/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Example/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Example/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Example/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Example/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Example/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
