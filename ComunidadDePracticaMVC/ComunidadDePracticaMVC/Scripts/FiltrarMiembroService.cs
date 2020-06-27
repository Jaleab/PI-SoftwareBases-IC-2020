using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ComunidadDePracticaMVC.Services
{
    public class FiltrarMiembroService
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["Grupo3Conn75"].ToString();
            con = new SqlConnection(constring);
        }
        public List<String> getPaises()
        {
            List<String> paises = new List<string>();
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
                paises.Add(Convert.ToString(dr["pais"]));
            }

            return paises;
        }

        public List<String> getHabilidades()
        {
            List<String> hablidades = new List<string>();
            connection();
            string consulta =
                "SELECT DISTINCT habilidadUsuario " +
                "FROM Habilidad";
            SqlCommand cmd = new SqlCommand(consulta, con);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();


            foreach (DataRow dr in dt.Rows)
            {
                hablidades.Add(Convert.ToString(dr["habilidadUsuario"]));
            }

            return hablidades;
        }
        public List<String> getIdiomas()
        {
            List<String> idiomas = new List<string>();
            connection();
            string consulta =
                "SELECT DISTINCT idiomaUsuario " +
                "FROM Idioma";
            SqlCommand cmd = new SqlCommand(consulta, con);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();


            foreach (DataRow dr in dt.Rows)
            {
                idiomas.Add(Convert.ToString(dr["idiomaUsuario"]));
            }

            return idiomas;
        }
        public List<String> getHobbies()
        {
            List<String> hobbies = new List<string>();
            connection();
            string consulta =
                "SELECT DISTINCT hobbieUsuario " +
                "FROM Hobbie";
            SqlCommand cmd = new SqlCommand(consulta, con);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();


            foreach (DataRow dr in dt.Rows)
            {
                hobbies.Add(Convert.ToString(dr["hobbieUsuario"]));
            }

            return hobbies;
        }
    }
}