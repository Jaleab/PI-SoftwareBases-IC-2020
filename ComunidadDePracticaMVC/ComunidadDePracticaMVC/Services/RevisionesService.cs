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

        public RevisionesModel ObtenerArticulosRequierenRevisores()
        {
            Connection();
            string consulta = "SELECT A.articuloId, A.titulo, A.fechaPublicacion FROM Articulo A WHERE A.estado = 'Revision' AND EXISTS(SELECT* FROM Revisa R WHERE R.articuloIdFK= A.articuloID) ORDER BY fechaPublicacion ";
            SqlCommand cmd = new SqlCommand(consulta, con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            RevisionesModel revisiones = new RevisionesModel();
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

        public bool ColaboracionDeUnMiembro(string correo,int articuloId) {
            bool exito = true;
            string operacion = "INSERT INTO Revisa (correoMiembroFK,articuloIdFK,estadoRevision,calificacion) VALUES(@correo,@articuloId,'Colaboracion',0)";
            Connection();
            SqlCommand cmd = new SqlCommand(operacion, con);

            cmd.Parameters.AddWithValue("@correo", correo);
            cmd.Parameters.AddWithValue("@articuloId", articuloId);

            con.Open();
            exito = cmd.ExecuteNonQuery() > 0;
            con.Close();
            return exito;
        }

        public bool PedirColaboracionATodos(int articuloId) {
            bool exito = true;
            RevisionesModel revision = new RevisionesModel();
            revision.Colaboradores = ObtenerMiembrosNucleo();
        
            foreach (var miembro in revision.Colaboradores) {
               exito=exito && ColaboracionDeUnMiembro(miembro.Correo, articuloId);
            }
            return exito;
        }

        //obtiene los articulos que fueron solicitados por el coordinador para un miembro del nucleo
        public List<ArticuloModel> GetSolicitudesDeColaboracion(int pageNumber, int pageSize, string autorCorreo) //OK
        {
            Connection();
            List<ArticuloModel> articulolist = new List<ArticuloModel>();


            SqlCommand cmd = new SqlCommand("GetSolicitudesDeColaboracion", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@correo", autorCorreo);
            cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);

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
                        Topico = Convert.ToString(dr["topico"]),
                        NotaRevision = Convert.ToInt32(dr["notaRevision"]),
                        FechaPublicacion = Convert.ToString(dr["fechaPublicacion"]),
                        Likes = Convert.ToInt32(dr["cantidadLikes"]),
                        Dislikes = Convert.ToInt32(dr["cantidadNoMeGusta"]),
                        Estado = Convert.ToString(dr["estado"])
                    });
            }


            return articulolist;
        }

        //Actualiza la colaboracion de la revision a aceptada
        public bool AceptarColaboracion(int articuloId, string correoNucleo) {
            bool exito = true;
            string operacion = "UPDATE Revisa SET Revisa.estadoRevision = 'Acepta colaborar' WHERE Revisa.correoMiembroFK = @correo AND articuloIdFK = @articuloId;";
            Connection();
            SqlCommand cmd = new SqlCommand(operacion, con);
            cmd.Parameters.AddWithValue("@correo", correoNucleo);
            cmd.Parameters.AddWithValue("@articuloId", articuloId);

            con.Open();
            exito = cmd.ExecuteNonQuery() > 0;
            con.Close();
            return exito;
        }

        //Actualiza la colaboracion de la revision a rechazada
        public bool RechazarColaboracion(int articuloId, string correoNucleo) {

            bool exito = true;
            string operacion = "UPDATE Revisa SET Revisa.estadoRevision = 'Rechaza colaborar' WHERE Revisa.correoMiembroFK = @correo AND articuloIdFK = @articuloId;";
            Connection();
            SqlCommand cmd = new SqlCommand(operacion, con);
            cmd.Parameters.AddWithValue("@correo", correoNucleo);
            cmd.Parameters.AddWithValue("@articuloId", articuloId);

            con.Open();
            exito = cmd.ExecuteNonQuery() > 0;
            con.Close();
            return exito;
        }

        public RevisionesModel ObtenerArticulosAsignadosRevisionAMiembro(string correoMiembro)
        {
            Connection();
            string consulta =
                "SELECT A.articuloId, A.titulo, A.fechaPublicacion FROM Articulo A JOIN Revisa R ON R.articuloIdFK=A.articuloId  WHERE R.estadoRevision='Pendiente revisar' AND R.correoMiembroFK=@correo ORDER BY fechaPublicacion "
                ;
            SqlCommand cmd = new SqlCommand(consulta, con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            cmd.Parameters.AddWithValue("@correo", correoMiembro);

            con.Open();
            sd.Fill(dt);
            con.Close();

            RevisionesModel revisiones = new RevisionesModel();
            List<ArticuloModel> listaArticulos = new List<ArticuloModel>();

            foreach (DataRow dr in dt.Rows)
            {
                revisiones.ArticulosEnRevision.Add(
                    new ArticuloModel
                    {
                        ArticuloId = Convert.ToInt32(dr["articuloId"]),
                        Autor= Convert.ToString(dr["titulo"]),
                        Titulo = Convert.ToString(dr["titulo"]),
                        FechaPublicacion = Convert.ToString(dr["fechaPublicacion"])
                    });
            }

            return revisiones;
        }

        //Actualiza la colaboracion de la revision a aceptada
        public bool AsignarCalificacion(int articuloId, string correoNucleo, int merito, FormularioModel model)
        {
            bool exito = true;
            int puntaje = merito * (model.OpinionGeneral + model.Contribucion + model.Forma);
            /*string operacion = "UPDATE Revisa " +
                               "SET Revisa.estadoRevision = @estado, Revisa.puntaje = @puntaje, Revisa.comentario = @comentario " +
                               "WHERE Revisa.correoMiembroFK = @correo AND articuloIdFK = @articuloId;";*/

            string operacion = "INSERT INTO CalificacionProvisional (email, articuloId, puntaje, estadoRevision, comentario) " +
                                            "VALUES(@correo, @articuloId, @puntaje, @estado, @comentario)";
            Connection();
            SqlCommand cmd = new SqlCommand(operacion, con);
            cmd.Parameters.AddWithValue("@correo", correoNucleo);
            cmd.Parameters.AddWithValue("@articuloId", articuloId);
            cmd.Parameters.AddWithValue("@estado", model.Estado);
            cmd.Parameters.AddWithValue("@comentario", model.Comentario);
            cmd.Parameters.AddWithValue("@puntaje", puntaje);

            con.Open();
            exito = cmd.ExecuteNonQuery() > 0;
            con.Close();
            return exito;
        }


        public List<UsuarioModel> ObtenerPosiblesRevisores(int articuloId)
        {
            Connection();
            string consulta = "SELECT U.nombre, U.apellido1, U.apellido2, U.correo FROM Usuario U WHERE U.correo = ANY( SELECT R.correoMiembroFK FROM Revisa R WHERE R.articuloIdFK = @articuloId AND R.estadoRevision = 'Acepta colaborar' OR R.estadoRevision = 'Rechaza colaborar' OR R.estadoRevision = 'Colaboracion' AND U.correo = R.correoMiembroFK)";
            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.Parameters.AddWithValue("@articuloId", articuloId);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            List<UsuarioModel> posiblesRevisores = new List<UsuarioModel>();
            foreach (DataRow dr in dt.Rows)
            {
                posiblesRevisores.Add(
                    new UsuarioModel
                    {
                        //aqui pueden ir mas atributos en caso de ser necesario
                        Correo = Convert.ToString(dr["correo"]),
                        Nombre = Convert.ToString(dr["nombre"]),
                        Apellido1 = Convert.ToString(dr["apellido1"]),
                        Apellido2 = Convert.ToString(dr["apellido2"])
                    });
            }
            return posiblesRevisores;
        }
    }
}