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

        //Obtener todos los articulos que han sido enviados a revision
        public RevisionesModel ObtenerArticulosEnRevision() {
            Connection();
            string consulta =
                "SELECT A.articuloId, A.titulo, A.fechaPublicacion FROM Articulo A WHERE A.estado='Revision' AND NOT EXISTS (SELECT * FROM Revisa R WHERE R.articuloIdFK=A.articuloID ) ORDER BY fechaPublicacion "
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

        public List<UsuarioModel> ObtenerMiembrosNucleo() {
            List<UsuarioModel> listaUsuarios = new List<UsuarioModel>();
            Connection();
            string consulta = "SELECT correoUsuarioFK FROM Miembro WHERE categoriaMiembro='Núcleo'";
            SqlCommand cmd = new SqlCommand(consulta, con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                listaUsuarios.Add(
                    new UsuarioModel
                    {
                        //aqui pueden ir mas atributos en caso de ser necesario
                        Correo = Convert.ToString(dr["correoUsuarioFK"])
                    });
            }
            return listaUsuarios;
        }

        public bool ColaboracionDeunMiembro(string correo,int articuloId) {
            bool exito = true;
            string operacion = "INSERT INTO Revisa (correoMiembroFK,articuloIdFK,estadoRevision,calificacion) VALUES(@correo,@articuloId,'Colaboracion',0)";
            Connection();
            SqlCommand cmd = new SqlCommand(operacion, con);

            cmd.Parameters.AddWithValue("@correo", correo);
            cmd.Parameters.AddWithValue("@articuloId", articuloId);

            con.Open();
            exito = exito & cmd.ExecuteNonQuery() > 0;
            con.Close();
            return exito;
        }

        public bool PedirColaboracionATodos(int articuloId) {
            bool exito = true;
            RevisionesModel revision = new RevisionesModel();
            revision.Colaboradores = ObtenerMiembrosNucleo();
        
            foreach (var miembro in revision.Colaboradores) {
               exito=exito && ColaboracionDeunMiembro(miembro.Correo, articuloId);
            }
            return exito;
        }
    }
}