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
            string constring = ConfigurationManager.ConnectionStrings["Grupo3Conn"].ToString();
            con = new SqlConnection(constring);
        }


        
        // ******************** ADD NEW ARTICULO ********************
        public List<ArticuloModel> GetArticulos(string keyword)
        {
            connection();
            List<ArticuloModel> Articulolist = new List<ArticuloModel>();

            SqlCommand cmd = new SqlCommand("GetArticulos", con);
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
                        Resumen = Convert.ToString(dr["Resumen"]),
                        Titulo = Convert.ToString(dr["titulo"])
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

            con.Open();
            sd.Fill(dt);
            con.Close();

            DataRow dr = dt.Rows[0];
            ArticuloModel articulo = new ArticuloModel();

            articulo.ArticuloId = Convert.ToInt32(dr["articuloId"]);
            articulo.Autor = Convert.ToString(dr["autor"]);
            articulo.Contenido = Convert.ToString(dr["contenido"]);
            articulo.Resumen = Convert.ToString(dr["resumen"]);
            articulo.Titulo = Convert.ToString(dr["titulo"]);
            articulo.Topico = Convert.ToString(dr["topico"]);


            Console.WriteLine(articulo.ArticuloId);
            return articulo;
        }


        public List<ArticuloModel> GetArticuloConditional(int pageNumber, int pageSize)
        {
            connection();
            List<ArticuloModel> articulolist = new List<ArticuloModel>();

            SqlCommand cmd = new SqlCommand("getArticuloByPage", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "@PageNumber",
                Value = pageNumber
            });

            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "@PageSize",
                Value = pageSize
            });
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                articulolist.Add(
                    new ArticuloModel
                    {
                        ArticuloId = Convert.ToInt32(dr["articuloId"]),
                        Autor = Convert.ToString(dr["autor"]),
                        Titulo = Convert.ToString(dr["titulo"]),
                        Resumen = Convert.ToString(dr["resumen"]),
                        Topico = Convert.ToString(dr["topico"]),
                        Contenido = Convert.ToString(dr["contenido"]),
                    });
            }


            return articulolist;
        }

        public void EditarArticulo(int id, ArticuloModel articulo)
        {
            //establecer la conexion con la base de datos
            connection();
            //Ejecutar la consulta de un articulo segun su id

            SqlCommand cmd = new SqlCommand("EditarArticulo", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Autor", articulo.Autor);
            cmd.Parameters.AddWithValue("@Pais", articulo.Pais);
            cmd.Parameters.AddWithValue("@Contenido", articulo.Contenido);
            cmd.Parameters.AddWithValue("@Resumen", articulo.Resumen); 

            con.Open();
            cmd.ExecuteNonQuery();
            
            con.Close();

            
        }

    }

}