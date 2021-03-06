﻿using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;
using ComunidadDePracticaMVC.Models;
namespace ComunidadDePracticaMVC.Services
{
    public class AccountService
    {
        private SqlConnection con;
        private void Connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["Grupo3Conn75"].ToString();
            con = new SqlConnection(constring);

        }
        public int RegisterUser(RegisterViewModel model)
        {
            Connection();
            SqlCommand cmd = new SqlCommand("InsertUser", con);
            cmd.CommandType = CommandType.StoredProcedure;

            string encryp = GetHash(model.Password, "SHA1");

            cmd.Parameters.AddWithValue("@Email", model.Email);
            cmd.Parameters.AddWithValue("@Name", model.Name);
            cmd.Parameters.AddWithValue("@LastName", model.LastName);
            cmd.Parameters.AddWithValue("@Country", model.Country);
            cmd.Parameters.AddWithValue("@Password", encryp);

            con.Open();
            int codereturn = (int)cmd.ExecuteScalar();
            return codereturn;
        }

        public int LoginUser(LoginViewModel model)
        {
            int codereturn = 0;
            if (Authenticate(model.Email, model.Password))
            {
                codereturn = 1;
            }
            else
            {
                codereturn = -1;
            }
            return codereturn;
        }

        private bool Authenticate(string Username, string Password)
        {

            Connection();
            SqlCommand cmd = new SqlCommand("SelectUser", con);
            cmd.CommandType = CommandType.StoredProcedure;

            string encryp = GetHash(Password, "SHA1");

            cmd.Parameters.AddWithValue("@Email", Username);
            cmd.Parameters.AddWithValue("@Password", encryp);
            con.Open();
            int codereturn = (int)cmd.ExecuteScalar();
            return codereturn == 1;

        }

        private string GetHash(string stringToHash, string hashAlgorithm)
        {
            //Ej: MD5, SHA1, SHA256, SHA384, SHA512
            var algorithm = System.Security.Cryptography.HashAlgorithm.Create(hashAlgorithm);
            byte[] hash = algorithm.ComputeHash(System.Text.Encoding.UTF8.GetBytes(stringToHash));

            // ToString("x2")  converts byte in hexadecimal value
            string encryptedVal = string.Concat(hash.Select(b => b.ToString("x2"))).ToUpperInvariant();
            return encryptedVal;
        }
    }

    public class CookieHandler {
        public static bool AuthorizeRole(HttpCookie authCookie, string rolesString) {

            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            CookieModel cookieInfo = JsonConvert.DeserializeObject<CookieModel>(authTicket.UserData);

            bool isInRole = false;

            string[] roles = rolesString.Split(',');
            foreach (string role in roles)
            {
                if (cookieInfo.Categoria.Equals(role))
                {
                    isInRole = true;
                }
            }

            return isInRole;

        }
    }
}
