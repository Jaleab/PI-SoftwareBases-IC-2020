using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ComunidadDePracticaMVC.Services
{
    public class ChartService
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["Grupo3Conn75"].ToString();
            con = new SqlConnection(constring);
        }

        public List<string> ObtenerPaises()
        {
            List<string> listaPaises = new List<string>();
            connection();   
            string consulta =
               "SELECT DISTINCT pais " +
               "FROM Usuario";

            SqlCommand cmd = new SqlCommand(consulta, con);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();


            foreach (DataRow dr in dt.Rows)
            {
                listaPaises.Add(Convert.ToString(dr["pais"]));
            }

            return listaPaises;
        }

        public List<string> ObtenerHabilidades()
        {
            List<string> listaPaises = new List<string>();
            connection();
            string consulta =
               "SELECT DISTINCT habilidadUsuario " +
               "FROM Habilidad, Usuario " +
               "WHERE Habilidad.correoUsuarioFK = Usuario.correo " +
               "AND habilidadUsuario = NULLIF(habilidadUsuario,'')";

            SqlCommand cmd = new SqlCommand(consulta, con);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();


            foreach (DataRow dr in dt.Rows)
            {
                listaPaises.Add(Convert.ToString(dr["habilidadUsuario"]));
            }

            return listaPaises;
        }

        public List<string> ObtenerHobbies()
        {
            List<string> listaHobbies = new List<string>();
            connection();
            string consulta =
               "SELECT DISTINCT hobbieUsuario " +
               "FROM Hobbie, Usuario " +
               "WHERE Hobbie.correoUsuarioFK = Usuario.correo " +
               "AND hobbieUsuario = NULLIF(hobbieUsuario,'')";

            SqlCommand cmd = new SqlCommand(consulta, con);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();


            foreach (DataRow dr in dt.Rows)
            {
                listaHobbies.Add(Convert.ToString(dr["hobbieUsuario"]));
            }

            return listaHobbies;
        }

        public List<string> ObtenerIdiomas()
        {
            List<string> listaIdiomas = new List<string>();
            connection();
            string consulta =
               "SELECT DISTINCT idiomaUsuario " +
               "FROM Idioma, Usuario " +
               "WHERE Idioma.correoUsuarioFK = Usuario.correo " +
               "AND idiomaUsuario = NULLIF(idiomaUsuario,'')";

            SqlCommand cmd = new SqlCommand(consulta, con);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();


            foreach (DataRow dr in dt.Rows)
            {
                listaIdiomas.Add(Convert.ToString(dr["idiomaUsuario"]));
            }

            return listaIdiomas;
        }


        public List<int> ObtenerCuentaPaises()
        {
            List<int> listaValores = new List<int>();
            connection();

            string consulta = "SELECT COUNT(DISTINCT U.correo) AS 'VALOR' FROM Usuario U GROUP BY U.pais;";

            SqlCommand cmd = new SqlCommand(consulta, con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows) {
                listaValores.Add(Convert.ToInt32(dr["VALOR"]));
            }
            return listaValores;
        }

        public List<int> ObtenerCuentaHabilidades()
        {
            List<int> listaValores = new List<int>();
            connection();

            string consulta = "SELECT COUNT(DISTINCT H.correoUsuarioFK) AS 'VALOR' FROM habilidad H GROUP BY H.habilidadUsuario HAVING H.habilidadUsuario != '';";

            SqlCommand cmd = new SqlCommand(consulta, con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                listaValores.Add(Convert.ToInt32(dr["VALOR"]));
            }
            return listaValores;
        }

        public List<int> ObtenerCuentaHobbies()
        {
            List<int> listaValores = new List<int>();
            connection();

            string consulta = "SELECT COUNT(DISTINCT Ho.correoUsuarioFK) AS 'VALOR' FROM Hobbie Ho GROUP BY Ho.hobbieUsuario HAVING Ho.hobbieUsuario != '';";

            SqlCommand cmd = new SqlCommand(consulta, con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                listaValores.Add(Convert.ToInt32(dr["VALOR"]));
            }
            return listaValores;
        }

        public List<int> ObtenerCuentaIdiomas()
        {
            List<int> listaValores = new List<int>();
            connection();

            string consulta = "SELECT COUNT(DISTINCT I.correoUsuarioFK) AS 'VALOR' FROM Idioma I GROUP BY I.idiomaUsuario HAVING I.idiomaUsuario != '';";

            SqlCommand cmd = new SqlCommand(consulta, con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                listaValores.Add(Convert.ToInt32(dr["VALOR"]));
            }
            return listaValores;
        }
    }
}

