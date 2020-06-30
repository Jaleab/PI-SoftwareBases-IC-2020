using ComunidadDePracticaMVC.Controllers;
using ComunidadDePracticaMVC.Models;
using ComunidadDePracticaMVC.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComunidadDePracticaMVC.Tests.Controllers
{
    [TestClass]
    public class LoginTest
    {
        [TestMethod]
        public void VerificarLogin_DatosCorrectos() {
            //Arrange
            AccountService service = new AccountService();
            LoginViewModel model = new LoginViewModel
            {
                Email = "edwin.brenes.c@gmail.com",
                Password = "delfin"
            };
            //Act
            int resultado = service.LoginUser (model);

            //Assert 
            Assert.AreEqual(1, resultado);
        }
        [TestMethod]
        public void VerificarLogin_DatosCorrectosAllCaps() {

            //Arrange
            AccountService service = new AccountService();
            LoginViewModel model = new LoginViewModel
            {
                Email = "EDWIN.BRENES.C@GMAIL.COM",
                Password = "DELFIN"
            };
            //Act
            int resultado = service.LoginUser(model);

            //Assert 
            Assert.AreEqual(-1, resultado);

        }
        [TestMethod]
        public void VerificarLogin_DatosCorrectosMinuscula() {
            //Arrange
            AccountService service = new AccountService();
            LoginViewModel model = new LoginViewModel
            {
                Email = "allcapsDELFIN@gmail.com",
                Password = "delfin"
            };
            //Act
            int resultado = service.LoginUser(model);

            //Assert 
            Assert.AreEqual(-1, resultado);

        }
        [TestMethod]
        public void VerificarLogin_DatosIncorrectos() {
            //Arrange
            AccountService service = new AccountService();
            LoginViewModel model = new LoginViewModel
            {
                Email = "aasdasd@cmas.com",
                Password = "de1212n"
            };
            //Act
            int resultado = service.LoginUser(model);

            //Assert 
            Assert.AreEqual(-1, resultado);
        }
        [TestMethod]
        public void VerificarLogin_NombreIncorrecto() {
            //Arrange
            AccountService service = new AccountService();
            LoginViewModel model = new LoginViewModel
            {
                Email = "aasdasd@cmas.com",
                Password = "delfin"
            };
            //Act
            int resultado = service.LoginUser(model);

            //Assert 
            Assert.AreEqual(-1, resultado);

        }
        [TestMethod]
        public void VerificarLogin_ContraseñaIncorrecta() {
            //Arrange
            AccountService service = new AccountService();
            LoginViewModel model = new LoginViewModel
            {
                Email = "edwin.brenes.c@gmail.com",
                Password = "asdasd"
            };
            //Act
            int resultado = service.LoginUser(model);

            //Assert 
            Assert.AreEqual(-1, resultado);
        }


    }
}
