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

        //Obtener todos los articulos que han sido enviados a revision, pero aun no tienen revisores asignados
        public RevisionesModel ObtenerArticulosEnRevision() {
            Connection();
            string consulta =
                "SELECT A.articuloId, A.titulo, STRING_AGG(U.nombre + ' ' + U.apellido1, ', ') AS autores, A.fechaPublicacion " +
                "FROM Articulo A " +
                "JOIN Publica P ON P.articuloIdFK = A.articuloId " +
                "JOIN Usuario U ON P.correoMiembroFK = U.correo " +
                "GROUP BY A.articuloId, A.titulo, A.fechaPublicacion, A.estado " +
                "HAVING A.estado='Revision' " +
                "AND NOT EXISTS (SELECT * FROM Revisa R WHERE R.articuloIdFK=A.articuloID ) " +
                "ORDER BY fechaPublicacion "
                ;
            SqlCommand cmd = new SqlCommand(consulta, con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            RevisionesModel revisiones=new RevisionesModel();

            foreach (DataRow dr in dt.Rows)
            {
                revisiones.ArticulosEnRevision.Add(
                    new ArticuloModel
                    {
                        ArticuloId = Convert.ToInt32(dr["articuloId"]),
                        Autor = Convert.ToString(dr["autores"]),
                        Titulo = Convert.ToString(dr["titulo"]),
                        FechaPublicacion = Convert.ToString(dr["fechaPublicacion"])
                    });
            }

            return revisiones;
        }
        //Se obtienen potenciales calificadores de articulos
        public List<UsuarioModel> ObtenerMiembrosNucleo() {
            Connection();
            List<UsuarioModel> listaUsuarios = new List<UsuarioModel>();
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
        //Agregar a la BD una peticion de revisar el articulo del parametro al autor con correo pasado por parametro
        public bool ColaboracionDeUnMiembro(string correo,int articuloId) {
            Connection();
            bool exito;
            string operacion = "INSERT INTO Revisa (correoMiembroFK,articuloIdFK,estadoRevision,calificacion,comentario) VALUES(@correo,@articuloId,'Colaboracion',0,' ')";
            SqlCommand cmd = new SqlCommand(operacion, con);

            cmd.Parameters.AddWithValue("@correo", correo);
            cmd.Parameters.AddWithValue("@articuloId", articuloId);

            con.Open();
            exito = cmd.ExecuteNonQuery() > 0;
            con.Close();
            return exito;
        }
        //invoca ek metodo anterior para hacer un envio masivo de solicitudes
        public bool PedirColaboracionATodos(int articuloId) {
            bool exito = true;
            RevisionesModel revision = new RevisionesModel();
            revision.Colaboradores = ObtenerMiembrosNucleo();
        
            foreach (var miembro in revision.Colaboradores) {
               exito=exito && ColaboracionDeUnMiembro(miembro.Correo, articuloId);
            }
            return exito;
        }

        //Para el autor con correo autorCorreo, recuperar todos los articulos para los cuales se el coordinador pidio colaborar
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
                        Autor = Convert.ToString(dr["autores"]),
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
            Connection();
            bool exito;
            string operacion = "UPDATE Revisa SET Revisa.estadoRevision = 'Acepta colaborar' WHERE Revisa.correoMiembroFK = @correo AND articuloIdFK = @articuloId;";
            SqlCommand cmd = new SqlCommand(operacion, con);
            cmd.Parameters.AddWithValue("@correo", correoNucleo);
            cmd.Parameters.AddWithValue("@articuloId", articuloId);

            con.Open();
            exito = cmd.ExecuteNonQuery() > 0;
            con.Close();
            return exito;
        }

        //Actualiza la colaboracion de la revision a rechazada
        public bool RechazarColaboracion(int articuloId, string correoNucleo) 
        {

            Connection();
            bool exito;
            string operacion = "UPDATE Revisa SET Revisa.estadoRevision = 'Rechaza colaborar' WHERE Revisa.correoMiembroFK = @correo AND articuloIdFK = @articuloId;";
            SqlCommand cmd = new SqlCommand(operacion, con);
            cmd.Parameters.AddWithValue("@correo", correoNucleo);
            cmd.Parameters.AddWithValue("@articuloId", articuloId);

            con.Open();
            exito = cmd.ExecuteNonQuery() > 0;
            con.Close();
            return exito;
        }
        //Obtener datos de artículos que ya tienen asignados al menos un revisor
        public RevisionesModel ObtenerArticulosAsignadosRevisionAMiembro(string correoMiembro)
        {
            Connection();
            string consulta =
                "SELECT A.articuloId, A.titulo, STRING_AGG(U.nombre + ' ' + U.apellido1, ', ') AS autores, A.fechaPublicacion " +
                "FROM Articulo A " +
                "JOIN Publica P ON P.articuloIdFK = A.articuloId " +
                "JOIN Usuario U ON P.correoMiembroFK = U.correo " +
                "GROUP BY A.articuloId, A.titulo, A.fechaPublicacion " +
                "HAVING A.articuloId IN(SELECT R.articuloIdFK " +
                                        "FROM Revisa R " +
                                        "JOIN Articulo A2 ON A2.articuloId = R.articuloIdFK " +
                                        "WHERE R.estadoRevision= 'Pendiente revisar' " +
                                        "AND A2.estado = 'Revision' " +
                                        "AND R.correoMiembroFK = @correo) " +
                "ORDER BY fechaPublicacion;"
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
                        Autor = Convert.ToString(dr["autores"]),
                        Titulo = Convert.ToString(dr["titulo"]),
                        FechaPublicacion = Convert.ToString(dr["fechaPublicacion"])
                    });
            }

            return revisiones;
        }

        //Modifica los datos de un articulo en revision para el autor que ya ha revisado
        public bool AsignarCalificacion(int articuloId, string correoNucleo, int calificacion, FormularioModel model)
        {
            Connection();
            bool exito;
            string operacion = "UPDATE Revisa " +
                               "SET Revisa.estadoRevision = @estado, Revisa.calificacion = @puntaje, Revisa.comentario = @comentario " +
                               "WHERE Revisa.correoMiembroFK = @correo AND articuloIdFK = @articuloId";
            SqlCommand cmd = new SqlCommand(operacion, con);
            cmd.Parameters.AddWithValue("@correo", correoNucleo);
            cmd.Parameters.AddWithValue("@articuloId", articuloId);
            cmd.Parameters.AddWithValue("@estado", model.Estado);
            cmd.Parameters.AddWithValue("@comentario", model.Comentario);
            cmd.Parameters.AddWithValue("@puntaje", calificacion);

            con.Open();
            exito = cmd.ExecuteNonQuery() > 0;
            con.Close();
            return exito;
        }
        //Obtener datos de un articulo
        public RevisionesModel ObtenerArticulosRequierenRevisores()
        {
            Connection();
            string consulta = "SELECT A.articuloId, A.titulo, A.fechaPublicacion, STRING_AGG(U.nombre + ' ' + U.apellido1, ', ') AS autores " +
                                "FROM Articulo A " +
                                "JOIN Publica P ON P.articuloIdFK = A.articuloId " +
                                "JOIN Usuario U ON U.correo = P.correoMiembroFK " +
                                "GROUP BY A.articuloId, A.titulo, A.fechaPublicacion, A.estado " +
                                "HAVING A.estado = 'Revision' " +
                                "AND EXISTS(SELECT* FROM Revisa R " +
                                            "WHERE R.articuloIdFK= A.articuloID " +
                                            "AND A.estado = 'Revision') " +
                                "ORDER BY fechaPublicacion;";
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
                        Autor = Convert.ToString(dr["autores"]),
                        Titulo = Convert.ToString(dr["titulo"]),
                        FechaPublicacion = Convert.ToString(dr["fechaPublicacion"])
                    });
            }
            return revisiones;
        }

        public List<UsuarioModel> ObtenerPosiblesRevisores(int articuloId)
        {
            Connection();
            string consulta = "SELECT DISTINCT U.nombre, U.apellido1, U.apellido2, U.correo FROM Usuario U JOIN Revisa R ON U.correo = R.correoMiembroFK WHERE R.articuloIdFK = @articuloId AND (R.estadoRevision = 'Acepta colaborar' OR R.estadoRevision = 'Rechaza colaborar' OR R.estadoRevision = 'Colaboracion') AND (R.estadoRevision != 'Pendiente revisar' OR R.estadoRevision != 'No asignado')";
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

        public bool AceptarRevisor(int articuloId, string correoRevisor)
        {
            Connection();
            bool exito;
            string operacion = "UPDATE Revisa SET estadoRevision = 'Pendiente revisar' WHERE correoMiembroFK = @correo AND articuloIdFK = @articuloId;";
            SqlCommand cmd = new SqlCommand(operacion, con);
            cmd.Parameters.AddWithValue("@correo", correoRevisor);
            cmd.Parameters.AddWithValue("@articuloId", articuloId);

            con.Open();
            exito = cmd.ExecuteNonQuery() > 0;
            con.Close();
            return exito;
        }

        //Actualiza la colaboracion de la revision a rechazada
        public bool RechazarRevisor(int articuloId, string correoRevisor)
        {
            Connection();
            bool exito;
            string operacion = "UPDATE Revisa SET estadoRevision = 'No asignado' WHERE correoMiembroFK = @correo AND articuloIdFK = @articuloId;";
            SqlCommand cmd = new SqlCommand(operacion, con);
            cmd.Parameters.AddWithValue("@correo", correoRevisor);
            cmd.Parameters.AddWithValue("@articuloId", articuloId);

            con.Open();
            exito = cmd.ExecuteNonQuery() > 0;
            con.Close();
            return exito;
        }


        //Este metodo retorna un modelo de revisiones cuya lista de articulos en revision corresponde a todos aquellos articulos que tienen asignado un revisor
        public RevisionesModel ArticulosEnRevisionPorMiembros() {
            Connection();
            string consulta =
            "SELECT A.articuloId, A.titulo, A.fechaPublicacion, STRING_AGG(U.nombre + ' ' + U.apellido1, ', ') AS autores " +
            "FROM Articulo A " +
            "JOIN Publica P ON P.articuloIdFK = A.articuloId " +
            "JOIN Usuario U ON U.correo = P.correoMiembroFK " +
            "GROUP BY A.articuloId, A.titulo, A.fechaPublicacion, A.estado " +
            "HAVING A.estado = 'Revision' " +
            "AND EXISTS ( SELECT * " +
                        "FROM Revisa R " +
                        "WHERE R.articuloIdFK= A.articuloId " +
                        "AND ( R.estadoRevision= 'Pendiente revisar' " +
                            "OR R.estadoRevision= 'Aprobado' " +
                            "OR R.estadoRevision= 'Aceptado con modificaciones' " +
                            "OR R.estadoRevision= 'Rechazado' ) )";
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
                        Autor = Convert.ToString(dr["autores"]),
                        Titulo = Convert.ToString(dr["titulo"]),
                        FechaPublicacion = Convert.ToString(dr["fechaPublicacion"])
                    });
            }
            return revisiones;
        }

        public List< Tuple<string,int> > CantidadesPorEstadoArticulosRevision(int articuloId) {
            
            Connection();
            string consulta = "DECLARE @numPendiente INT, @numAprob INT , @numRech INT, @numAcepMod INT "+
                               "SELECT @numPendiente = COUNT(*) FROM Revisa WHERE estadoRevision = 'Pendiente revisar' AND articuloIdFK = @articuloId "+
                               "SELECT @numAprob = COUNT(*) FROM Revisa WHERE estadoRevision = 'Aprobado' AND articuloIdFK = @articuloId "+
                               "SELECT @numAcepMod = COUNT(*) FROM Revisa WHERE estadoRevision = 'Aceptado con modificaciones' AND articuloIdFK = @articuloId "+
                               "SELECT @numRech = COUNT(*) FROM Revisa WHERE estadoRevision = 'Rechazado' AND articuloIdFK = @articuloId "+
                               "SELECT @numAcepMod AS[Aceptado con modificaciones] , @numAprob AS[Aprobado], @numRech AS[Rechazado], @numPendiente AS[Pendiente revisar] ";
            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.Parameters.AddWithValue("@articuloId", articuloId);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            DataRow dr = dt.Rows[0];
            Tuple<string, int> par1 = new Tuple<string, int> ("Aceptado con modificaciones", Convert.ToInt32(dr["Aceptado con modificaciones"]));
            Tuple<string, int> par2 = new Tuple<string, int>("Aprobado", Convert.ToInt32(dr["Aprobado"]));
            Tuple<string, int> par3 = new Tuple<string, int>("Rechazado", Convert.ToInt32(dr["Rechazado"]));
            Tuple<string, int> par4 = new Tuple<string, int>("Pendiente revisar", Convert.ToInt32(dr["Pendiente revisar"]));

            List<Tuple<string, int>> valores = new List<Tuple<string, int>> () ;
            
            valores.Add(par1);
            valores.Add(par2);
            valores.Add(par3);
            valores.Add(par4);

            return valores;
        }

        public List<CalificacionesModel> ObtenerCalificaciones(int articuloId) {
            Connection();
            string consulta = "SELECT R.articuloIdFK, R.correoMiembroFK, M.merito, " +
                                        "R.estadoRevision, R.calificacion, R.comentario " +
                              "FROM Revisa R JOIN Miembro M ON M.correoUsuarioFK = R.correoMiembroFK " +
                              "WHERE R.articuloIdFK = @articuloId " +
                              "AND (R.estadoRevision = 'Aprobado' " +
                                   "OR R.estadoRevision = 'Rechazado' " +
                                   "OR R.estadoRevision = 'Aceptado con modificaciones');";
            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.Parameters.AddWithValue("@articuloId", articuloId);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            List<CalificacionesModel> calificaciones = new List<CalificacionesModel>();
            foreach (DataRow dr in dt.Rows)
            {
                calificaciones.Add(
                    new CalificacionesModel
                    {
                        //aqui pueden ir mas atributos en caso de ser necesario
                        ArticuloId = Convert.ToInt32(dr["articuloIdFK"]),
                        Revisor = Convert.ToString(dr["correoMiembroFK"]),
                        MeritoRevisor = Convert.ToInt32(dr["merito"]),
                        Estado = Convert.ToString(dr["estadoRevision"]),
                        Calificacion = Convert.ToInt32(dr["calificacion"]),
                        Comentario = Convert.ToString(dr["comentario"])
                        
                    });
            }
            return calificaciones;
        }

        public bool DecisionFinal(DecisionFinalModel modelo) {
            bool exito;
            string operacion = "UPDATE Articulo SET estado = @estado, notaRevision=@nota, comentario=@comentario WHERE articuloId = @articuloId ";
            Connection();
            SqlCommand cmd = new SqlCommand(operacion, con);
            cmd.Parameters.AddWithValue("@articuloId", modelo.ArticuloId);
            cmd.Parameters.AddWithValue("@estado", modelo.Decision);
            cmd.Parameters.AddWithValue("@nota", modelo.NotaFinal);
            cmd.Parameters.AddWithValue("@comentario", modelo.Comentario);

            con.Open();
            exito = cmd.ExecuteNonQuery() > 0;
            con.Close();

            return exito;
        }
    }
}