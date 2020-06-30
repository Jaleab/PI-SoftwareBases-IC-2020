using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PassParameter.Controllers;
using ComunidadDePracticaMVC.Controllers;
using System.Web.Mvc;
using ComunidadDePracticaMVC.Models;
using ComunidadDePracticaMVC.Services;

namespace ComunidadDePracticaMVC.Tests.Controllers
{
    [TestClass]
    public class ArticuloTest
    {
        [TestMethod]
        public void TestInicioArticulosNotNull()
        {
            ArticuloController artController = new ArticuloController();

            ViewResult result = artController.InicioArticulos() as ViewResult;

            Assert.IsNotNull(result); 
        }

        [TestMethod]
        public void TestCrearArticulo()
        {
            ArticuloService artService = new ArticuloService();

            ArticuloController artController = new ArticuloController();

            ViewResult result = artController.Create() as ViewResult;
        }


    }
}
