using ComunidadDePracticaMVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComunidadDePracticaMVC.ArticuloService;

namespace ComunidadDePracticaMVC.ArticuloService
{
    public class UsuarioService
    {
        private SqlConnection con;
        private void Connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["Grupo3Conn75"].ToString();
            con = new SqlConnection(constring);
        }


        public UsuarioModel GetProfile(string correo)
        {
            Connection();

            SqlCommand cmd = new SqlCommand("GetProfile", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "@Correo",
                Value = correo
            });

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            UsuarioModel usuario = new UsuarioModel();


            con.Open();
            sd.Fill(dt);
            con.Close();

            DataRow dr = dt.Rows[0];
            usuario.Correo = Convert.ToString(dr["correo"]);
            usuario.Nombre = Convert.ToString(dr["nombre"]);
            usuario.Apellido1 = Convert.ToString(dr["apellido1"]);
            usuario.Apellido2 = Convert.ToString(dr["apellido2"]);
            usuario.Ciudad = Convert.ToString(dr["ciudad"]);
            usuario.Pais = Convert.ToString(dr["pais"]);            
            usuario.Merito = Convert.ToString(dr["merito"]);
            usuario.Peso = Convert.ToString(dr["peso"]);
            usuario.Categoria = Convert.ToString(dr["categoriaMiembro"]);
            usuario.Habilidad = Convert.ToString(dr["habilidadUsuario"]);
            usuario.Hobbie = Convert.ToString(dr["hobbieUsuario"]);

            return usuario;
        }

        
        public void EditarUsuario(UsuarioModel usuario)
        {
            
            Connection();

            SqlCommand cmd = new SqlCommand("EditarUsuario", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
            cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
            cmd.Parameters.AddWithValue("@Apellido1", usuario.Apellido1);
            cmd.Parameters.AddWithValue("@Apellido2", usuario.Apellido2);
            cmd.Parameters.AddWithValue("@Ciudad", usuario.Ciudad);
            cmd.Parameters.AddWithValue("@Pais", usuario.Pais);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

         
        }


        public UsuarioModelCompleto GetUsuarioCompleto(string correo)
        {
            UsuarioModelCompleto modelo = new UsuarioModelCompleto();
            Connection();

            SqlCommand cmd = new SqlCommand("GetIdiomas", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Correo", correo);
            DataTable dt = new DataTable();
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            con.Open();
            sd.Fill(dt);
            con.Close();

            List<string> idiomas = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                idiomas.Add(Convert.ToString(dr["idiomaUsuario"]));
            };

             cmd = new SqlCommand("GetHobbies", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Correo", correo);
             dt = new DataTable();
             sd = new SqlDataAdapter(cmd);
            con.Open();
            sd.Fill(dt);
            con.Close();
            List<string> hobbies = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                hobbies.Add(Convert.ToString(dr["hobbieUsuario"]));
            };

             cmd = new SqlCommand("GetHabilidades", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Correo", correo);
             dt = new DataTable();
             sd = new SqlDataAdapter(cmd);
            con.Open();
            sd.Fill(dt);
            con.Close();
            List<string> habilidades = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                habilidades.Add(Convert.ToString(dr["habilidadUsuario"]));
            };

            List<UsuarioModel> usuario = new List<UsuarioModel>();
            usuario.Add(GetProfile(correo));

            List<ArticuloModel> articulo = new List<ArticuloModel>();
            ArticuloService service = new ArticuloService();
            articulo=service.GetArticulosUsuario(correo);


            modelo.Idioma = idiomas;
            modelo.Usuario = usuario;
            modelo.Hobbie = hobbies;
            modelo.Habilidad = habilidades;
            modelo.Articulo = articulo;

            return modelo;
        }
    }
}
