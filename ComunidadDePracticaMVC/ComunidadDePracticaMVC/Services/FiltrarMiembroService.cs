using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using ComunidadDePracticaMVC.Models; 


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

        public List<MiembroModel> getUsuarios()
        {
            List<MiembroModel> usuarios = new List<MiembroModel>();
            
            connection();
            string consulta =
                "SELECT DISTINCT correo, nombre,apellido1, pais,hobbieUsuario, idiomaUsuario, habilidadUsuario " +
                "FROM Usuario U , Hobbie H, Idioma I, Habilidad HA " +
                "WHERE U.correo = H.correoUsuarioFK " +
                "AND U.correo = I.correoUsuarioFK " +
                "AND U.correo = HA.correoUsuarioFK";
            SqlCommand cmd = new SqlCommand(consulta, con);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();


            foreach (DataRow dr in dt.Rows)
            {
                usuarios.Add(new MiembroModel
                            {
                                nombre = Convert.ToString(dr["nombre"]),
                                apellido = Convert.ToString(dr["apellido1"]),
                                pais = Convert.ToString(dr["pais"]),
                                habilidad = Convert.ToString(dr["habilidadUsuario"]),
                                hobbie = Convert.ToString(dr["hobbieUsuario"]), 
                                idioma = Convert.ToString(dr["idiomaUsuario"]),
                                correo = Convert.ToString(dr["correo"])
                }
                );
            }

            return usuarios; 
        }

        public List<MiembroModel> GetDatosPersonalesByFiltro(FiltroMiembroModel filtro)
        {
            List<MiembroModel> usuarios = new List<MiembroModel>();

            connection();
            SqlCommand cmd = new SqlCommand("filtrarMiembroPorDatosPersonales", con);
            cmd.CommandType = CommandType.StoredProcedure;

            if (filtro.paisSeleccionado == null) {
                filtro.paisSeleccionado = "";
            }
            if (filtro.hobbieSeleccionado == null) {
                filtro.hobbieSeleccionado = "";
            }
            if (filtro.idiomaSeleccionado == null)
            {
                filtro.idiomaSeleccionado = "";
            }
            if (filtro.habilidadSeleccionado == null)
            {
                filtro.habilidadSeleccionado = "";
            }

            cmd.Parameters.AddWithValue("@pais", filtro.paisSeleccionado);
            cmd.Parameters.AddWithValue("@hobbie", filtro.hobbieSeleccionado);
            cmd.Parameters.AddWithValue("@idioma", filtro.idiomaSeleccionado);
            cmd.Parameters.AddWithValue("@habilidad", filtro.habilidadSeleccionado);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();


            foreach (DataRow dr in dt.Rows)
            {
                usuarios.Add(new MiembroModel
                {
                    correo = Convert.ToString(dr["correo"]),
                    nombre = Convert.ToString(dr["nombre"]),
                    apellido = Convert.ToString(dr["apellido1"]),
                    pais = Convert.ToString(dr["pais"]),
                    hobbie = Convert.ToString(dr["hobbies"]),
                    idioma = Convert.ToString(dr["idiomas"]),
                    habilidad = Convert.ToString(dr["habilidades"])

                }
                );
            }

            return usuarios;

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

        public List<MiembroModel> GetMiembrosPorFiltros(string pais, string habilidad, string hobby, string idioma)
        {
            List<MiembroModel> usuarios = new List<MiembroModel>();

            connection();
            SqlCommand cmd = new SqlCommand("filtrarMiembroPorDatosPersonales", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@pais", pais);
            cmd.Parameters.AddWithValue("@hobbie", hobby);
            cmd.Parameters.AddWithValue("@idioma", idioma);
            cmd.Parameters.AddWithValue("@habilidad", habilidad);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();


            foreach (DataRow dr in dt.Rows)
            {
                usuarios.Add(new MiembroModel
                {
                    correo = Convert.ToString(dr["correo"]),
                    nombre = Convert.ToString(dr["nombre"]),
                    apellido = Convert.ToString(dr["apellido1"]),
                    pais = Convert.ToString(dr["pais"]),
                    hobbie = Convert.ToString(dr["hobbies"]),
                    idioma = Convert.ToString(dr["idiomas"]),
                    habilidad = Convert.ToString(dr["habilidades"])

                }
                );
            }

            return usuarios;

        }
    }


}