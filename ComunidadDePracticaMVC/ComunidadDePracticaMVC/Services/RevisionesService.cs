using ComunidadDePracticaMVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ComunidadDePracticaMVC.Services
{
    public class RevisionesService
    {
        private SqlConnection con;
        private void Connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["Grupo3Conn75"].ToString();
            con = new SqlConnection(constring);
        }

        public RevisionesModel ObtenerArticulosEnRevision() {
            Connection();
            string consulta =
                "SELECT articuloId, titulo, fechaPublicacion FROM Articulo WHERE estado='Revision' ORDER BY fechaPublicacion "
                ;
            SqlCommand cmd = new SqlCommand(consulta, con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            RevisionesModel revisiones=new RevisionesModel();
            List<ArticuloModel> listaArticulos = new List<ArticuloModel>();

            foreach (DataRow dr in dt.Rows)
            {
                revisiones.ArticulosEnRevision.Add(
                    new ArticuloModel
                    {
                        ArticuloId = Convert.ToInt32(dr["articuloId"]),
                        Titulo = Convert.ToString(dr["titulo"]),
                        FechaPublicacion = Convert.ToString(dr["fechaPublicacion"])
                    });
            }

            return revisiones;
        }
    }
}