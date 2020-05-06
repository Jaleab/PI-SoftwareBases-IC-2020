using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using ComunidadDePracticaMVC.Models;

namespace ComunidadDePracticaMVC.Services
{
    public class ArticuloService
    {
        private SqlConnection con;
        private void connection()
        {
            string constring =
                                   ConfigurationManager.ConnectionStrings["articuloconn"].ToString();
            con = new SqlConnection(constring);
        }

        // ******************** ADD NEW ARTICULO ********************
        public bool AddArticulo(ArticuloModel artmodel)
        {
            connection();
            SqlCommand cmd = new SqlCommand("AgregarNuevoArticulo", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Autor", artmodel.Autor);
            cmd.Parameters.AddWithValue("@Pais", artmodel.Pais);
            cmd.Parameters.AddWithValue("Contenido", artmodel.Contenido);
            cmd.Parameters.AddWithValue("Resumen", artmodel.Resumen);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return i >= 1;
            
        }

        public List<ArticuloModel> GetArticulos()
        {
            connection();
            List<ArticuloModel> Articulolist = new List<ArticuloModel>();

            SqlCommand cmd = new SqlCommand("GetArticuloDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                Articulolist.Add(
                    new ArticuloModel
                    {
                        ArticuloId = Convert.ToInt32(dr["Id"]),
                        Autor = Convert.ToString(dr["Autor"]),
                        Pais = Convert.ToString(dr["Pais"]),
                        Contenido = Convert.ToString(dr["Contenido"]),
                        Resumen = Convert.ToString(dr["Resumen"])
                    });
            }
            return Articulolist;
        }

        public ArticuloModel GetInfoArticulo(int id)
        {
            //establecer la conexion con la base de datos
            connection();
            //Ejecutar la consulta de un articulo segun su id
            SqlCommand cmd = new SqlCommand("ConsultaUnitaria", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ArtId", id);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            //Llenar un tabla de datos c# con los valores de la consulta
            con.Open();
            sd.Fill(dt);
            con.Close();
            //Crear un articulo con los datos obtenidos de la consulta
            ArticuloModel articulo = new ArticuloModel();
            foreach (DataRow dr in dt.Rows){
                articulo.ArticuloId = Convert.ToInt32(dr["Id"]);
                articulo.Autor = Convert.ToString(dr["Autor"]);
                articulo.Pais = Convert.ToString(dr["Pais"]);
                articulo.Contenido = Convert.ToString(dr["Contenido"]);
                articulo.Resumen = Convert.ToString(dr["Resumen"]);
            }

            return articulo;
        }

        public ArticuloModel Prueba()
        {
            //establecer la conexion con la base de datos
            connection();
            //Ejecutar la consulta de un articulo segun su id
            SqlCommand cmd = new SqlCommand("ConsultaUnitaria", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ArtId", 1);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            //Llenar un tabla de datos c# con los valores de la consulta
            con.Open();
            sd.Fill(dt);
            con.Close();
            //Crear un articulo con los datos obtenidos de la consulta
            ArticuloModel articulo = new ArticuloModel();
            foreach (DataRow dr in dt.Rows)
            {
                articulo.ArticuloId = Convert.ToInt32(dr["Id"]);
                articulo.Autor = Convert.ToString(dr["Autor"]);
                articulo.Pais = Convert.ToString(dr["Pais"]);
                articulo.Contenido = Convert.ToString(dr["Contenido"]);
                articulo.Resumen = Convert.ToString(dr["Resumen"]);
            }

            return articulo;
        }

    }


}