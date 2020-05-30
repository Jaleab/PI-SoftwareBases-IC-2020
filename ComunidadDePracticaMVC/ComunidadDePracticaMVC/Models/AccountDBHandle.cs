using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ComunidadDePracticaMVC.Models
{
    public class AccountDBHandle
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["Grupo3Conn"].ToString();
            con = new SqlConnection(constring);

        }
        public int registerUser(RegisterViewModel model)
        {
            connection();
            SqlCommand cmd = new SqlCommand("InsertUser", con);
            cmd.CommandType = CommandType.StoredProcedure;

            string encryp = GetHash(model.Password, "SHA1");

            cmd.Parameters.AddWithValue("@Username", model.Username);
            cmd.Parameters.AddWithValue("@Password", encryp);
            cmd.Parameters.AddWithValue("@Email", model.Email);

            con.Open();
            int codereturn = (int)cmd.ExecuteScalar();
            return codereturn;
        }

        public int loginUser(LoginViewModel model)
        {
            int codereturn = 0;
            if (authenticate(model.Username, model.Password))
            {
                codereturn = 1;
            }
            else
            {
                codereturn = -1;
            }
            return codereturn;
        }

        private bool authenticate(string Username, string Password)
        {

            connection();
            SqlCommand cmd = new SqlCommand("SelectUser", con);
            cmd.CommandType = CommandType.StoredProcedure;

            string encryp = GetHash(Password, "SHA1");

            cmd.Parameters.AddWithValue("@Username",  Username);
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
}