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
                                   ConfigurationManager.ConnectionStrings["Grupo3Conn"].ToString();
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

            SqlCommand cmd = new SqlCommand("GetArticulos   ", con);
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
                        Contenido = Convert.ToString(dr["Contenid"]),
                        Resumen = Convert.ToString(dr["Resumen"])
                    });
            }
            return Articulolist;
        }


    }
}