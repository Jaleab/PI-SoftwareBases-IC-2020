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
            List<String> paises = this.ObtenerPaises(); 
            List<int> listaValores = new List<int>();
            connection();


            string consulta =
               "SELECT COUNT(pais) AS 'VALOR' " +
               "FROM Usuario " + 
               "WHERE pais = @paisParam";

            SqlCommand cmd = new SqlCommand(consulta, con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            for (int i = 0; i < paises.Count(); ++i)
            {
                cmd.Parameters.Clear(); 
                cmd.Parameters.AddWithValue("@paisParam", paises[i]);
                dt.Clear();  
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                con.Open();
                sd.Fill(dt);
                con.Close();

                DataRow dr = dt.Rows[0];
                listaValores.Add(Convert.ToInt32(dr["VALOR"])); 
            }
            return listaValores;
        }

        public List<int> ObtenerCuentaHabilidades()
        {
            List<String> habilidades = this.ObtenerHabilidades(); 
            List<int> listaValores = new List<int>();
            connection();


            string consulta =
               "SELECT COUNT(habilidadUsuario) AS 'VALOR' " +
               "FROM Habilidad, Usuario " +
               "WHERE Habilidad.correoUsuarioFK = Usuario.correo " +
               "AND habilidadUsuario = @habilidadParam";

            SqlCommand cmd = new SqlCommand(consulta, con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            for (int i = 0; i < habilidades.Count(); ++i)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@habilidadParam", habilidades[i]);
                dt.Clear();
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                con.Open();
                sd.Fill(dt);
                con.Close();

                DataRow dr = dt.Rows[0];
                listaValores.Add(Convert.ToInt32(dr["VALOR"]));
            }
            return listaValores;
        }

        public List<int> ObtenerCuentaHobbies()
        {
            List<String> hobbies = this.ObtenerHobbies();
            List<int> listaValores = new List<int>();
            connection();


            string consulta =
               "SELECT COUNT(hobbieUsuario) AS 'VALOR' " +
               "FROM Hobbie, Usuario " +
               "WHERE Hobbie.correoUsuarioFK = Usuario.correo " +
               "AND hobbieUsuario = @hobbieParam";

            SqlCommand cmd = new SqlCommand(consulta, con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            for (int i = 0; i < hobbies.Count(); ++i)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@hobbieParam", hobbies[i]);
                dt.Clear();
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                con.Open();
                sd.Fill(dt);
                con.Close();

                DataRow dr = dt.Rows[0];
                listaValores.Add(Convert.ToInt32(dr["VALOR"]));
            }
            return listaValores;
        }

        public List<int> ObtenerCuentaIdiomas()
        {
            List<String> idomas = this.ObtenerIdiomas();
            List<int> listaValores = new List<int>();
            connection();


            string consulta =
               "SELECT COUNT(idiomaUsuario) AS 'VALOR' " +
               "FROM Idioma, Usuario " +
               "WHERE Idioma.correoUsuarioFK = Usuario.correo " +
               "AND idiomaUsuario = @idiomaUsuario";

            SqlCommand cmd = new SqlCommand(consulta, con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            for (int i = 0; i < idomas.Count(); ++i)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@idiomaUsuario", idomas[i]);
                dt.Clear();
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                con.Open();
                sd.Fill(dt);
                con.Close();

                DataRow dr = dt.Rows[0];
                listaValores.Add(Convert.ToInt32(dr["VALOR"]));
            }
            return listaValores;
        }
    }
}

