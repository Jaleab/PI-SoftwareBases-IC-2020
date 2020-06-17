using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using ComunidadDePracticaMVC.Models;
using System.IO;
using ComunidadDePracticaMVC.ViewModels;

namespace ComunidadDePracticaMVC.Services
{

    public class ArticuloService
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["Grupo3Conn75"].ToString();
            con = new SqlConnection(constring);
        }

        //**Aumentar cantidad de visitas de un artículo
        public void AumentarVisitas(int id) //OK
        {
            connection();

            SqlCommand cmd = new SqlCommand("AumentarVisitas", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IdArticulo", id);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private string ObtenerAutorDeArticulo(int articuloID)
        { //deberia estar en un modelo de usuarios o autores
            connection();
            
            SqlCommand cmd = new SqlCommand("ObtenerAutorDeArticulo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@articuloID", articuloID);



            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();

            DataRow dr = dt.Rows[0];
 
            return Convert.ToString(dr["correoMiembroFK"]);
        }

        public void AumentarMeritoPorCalificacion(int articuloID,int merito)
        {
            //esto deberia estar en el modelo de usuarios
            string correoUsuario = ObtenerAutorDeArticulo(articuloID);
            connection();

            SqlCommand cmd = new SqlCommand("ModificarMeritos", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@merito", merito);
            cmd.Parameters.AddWithValue("@correo", correoUsuario);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

        }

        public void ModificarPuntaje(int idArticulo, string correoMiembro, int puntaje) //OK
        {
            connection();

            SqlCommand cmd = new SqlCommand("ModificarPuntaje", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@articuloId", idArticulo);
            cmd.Parameters.AddWithValue("@correoMiembro", correoMiembro);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        //TODO METODO PROVISIONAL
        public void modificarLikes(int idArticulo, int puntaje) //OK
        {
            connection();

            //SqlCommand cmd = new SqlCommand("ModificarLikes", con);
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@IdArticulo", idArticulo);
            //cmd.Parameters.AddWithValue("@Puntaje", puntaje);
            //con.Open();
            //cmd.ExecuteNonQuery();
            //con.Close();

            string consulta1 = 
                "UPDATE Articulo "+
                "SET cantidadLikes=cantidadLikes+1 "+
                "WHERE articuloId=@articuloId"
                ;
            string consulta2 =
                    "UPDATE Articulo " +
                    "SET cantidadNoMeGusta=cantidadNoMeGusta+1 " +
                    "WHERE articuloId=@articuloId"
                    ;
            SqlCommand cmd1;
            if (puntaje == 1)
            {
                cmd1 = new SqlCommand(consulta1, con);
                cmd1.Parameters.AddWithValue("@articuloId", idArticulo);
                con.Open();
                cmd1.ExecuteNonQuery();
                con.Close();
            }
            else if (puntaje == -1) {
                cmd1 = new SqlCommand(consulta2, con);
                cmd1.Parameters.AddWithValue("@articuloId", idArticulo);
                con.Open();
                cmd1.ExecuteNonQuery();
                con.Close();
            }

        }


        // ******************** ADD NEW ARTICULO ********************
        public List<ArticuloModel> GetArticulos() //OK
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
                        ArticuloId = Convert.ToInt32(dr["articuloId"]),
                        Autor = Convert.ToString(dr["autor"]),
                        Titulo = Convert.ToString(dr["titulo"]),
                        Resumen = Convert.ToString(dr["resumen"]),
                        Topico = Convert.ToString(dr["topico"]),
                        Contenido = Convert.ToString(dr["contenido"])
                    });
            }
            return Articulolist;
        }

        public bool CrearArticulo(ArticuloModel articulo)
        {
            //establecer la conexion con la base de datos
            bool exito = false;
            connection();


            SqlCommand cmd = new SqlCommand("AgregarNuevoArticuloCorto", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Titulo", articulo.Titulo);
            cmd.Parameters.AddWithValue("@Topico", articulo.Topico);
            cmd.Parameters.AddWithValue("@Contenido", articulo.Contenido);
            cmd.Parameters.AddWithValue("@Resumen", articulo.Resumen);

            con.Open();
            try
            {
                exito = cmd.ExecuteNonQuery() >= 1;
            }
            catch (Exception e)
            {

            }
            con.Close();
            if (exito == false)
            {
                return exito;
            }

            DataTable dt = new DataTable();

            connection();
            SqlCommand cmd2 = new SqlCommand();
            cmd2 = new SqlCommand(
                "SELECT articuloId" + " " +
                "FROM Articulo" + " " +
                "WHERE titulo = @TituloArticulo" + " ", con);
            cmd2.Parameters.AddWithValue("@TituloArticulo", articulo.Titulo);
            cmd2.CommandType = CommandType.Text;
            SqlDataAdapter sd2 = new SqlDataAdapter(cmd2);
            con.Open();
            sd2.Fill(dt);
            con.Close();

            DataRow dr = dt.Rows[0];
            int articuloIdGuardado = Convert.ToInt32(dr["articuloId"]);
            AgregarArticuloAPublicacion(articuloIdGuardado, articulo.Correo);
            con.Close();
            return exito;
        }

        public ArticuloModel GetInfoArticulo(int id) //OK
        {
            //establecer la conexion con la base de datos
            connection();
            //Ejecutar la consulta de un articulo segun su id
            SqlCommand cmd = new SqlCommand("GetArticuloPorId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ArticuloId", id);
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
            articulo.TipoArchivo= Convert.ToString(dr["tipoArchivo"]);
            articulo.Likes= Convert.ToInt32(dr["cantidadLikes"]);
            articulo.Dislikes = Convert.ToInt32(dr["cantidadNoMeGusta"]);
            articulo.FechaPublicacion = Convert.ToString(dr["fechaPublicacion"]);


            Console.WriteLine(articulo.ArticuloId);
            return articulo;
        }
        public List<ArticuloModel> GetArticulosByAutor(int pageNumber, int pageSize, string autorCorreo) //OK
        {
            connection();
            List<ArticuloModel> articulolist = new List<ArticuloModel>();


            SqlCommand cmd = new SqlCommand("GetArticulosDeAutorByPage", con);
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
                        Resumen = Convert.ToString(dr["resumen"]),
                        Topico = Convert.ToString(dr["topico"]),
                        Contenido = Convert.ToString(dr["contenido"]),
                        NotaRevision = Convert.ToInt32(dr["notaRevision"]),
                        FechaPublicacion = Convert.ToString(dr["fechaPublicacion"]),
                        Estado = Convert.ToString(dr["estado"])
                    });
            }


            return articulolist;
        }

        public List<ArticuloModel> GetArticuloConditional(int pageNumber, int pageSize) //OK
        {
            connection();
            List<ArticuloModel> articulolist = new List<ArticuloModel>();

            SqlCommand cmd = new SqlCommand("GetArticuloByPage", con);
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
                        NotaRevision= Convert.ToInt32(dr["notaRevision"]),
                        FechaPublicacion = Convert.ToString(dr["fechaPublicacion"]),
                        Likes= Convert.ToInt32(dr["cantidadLikes"]),
                        Dislikes = Convert.ToInt32(dr["cantidadNoMeGusta"])
                    });
            }


            return articulolist;
        }

        public bool EditarArticuloCorto(int id, ArticuloModel articulo, string hilera)
        {
            connection();
            bool exito = false;
            SqlCommand cmd = new SqlCommand("UPDATE Articulo SET titulo = @Titulo, topico = @Topico, resumen = @Resumen, contenido = @Contenido  WHERE articuloId = @Id", con);
            //cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Titulo", articulo.Titulo);
            cmd.Parameters.AddWithValue("@Resumen", articulo.Resumen);
            cmd.Parameters.AddWithValue("@Topico", articulo.Topico);
            cmd.Parameters.AddWithValue("@Contenido", articulo.Contenido);
            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();
            try
            {
                exito = cmd.ExecuteNonQuery() >= 1;

            }
            catch (Exception e)
            {
            }
            con.Close();
            if (exito == false)
            {
                return exito;
            }

            connection();
            cmd = new SqlCommand(
                "UPDATE Topico" + " " +
                "SET topico = @Hilera"+ " " +
                "WHERE topico = @TopicoAnterior" + " " +
                "AND articuloIdFk = @ArticuloId", con);

            cmd.Parameters.AddWithValue("@TopicoAnterior", articulo.Topico);
            cmd.Parameters.AddWithValue("@Hilera", hilera);
            cmd.Parameters.AddWithValue("@ArticuloId", articulo.ArticuloId);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return exito;
        }

        public bool BorrarArticulo(int id)
        {
            //establecer la conexion con la base de datos
            connection();
            //Ejecutar la consulta de un articulo segun su id

            SqlCommand cmd = new SqlCommand("BorrarArticulo", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ArtId", id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        public List<ArticuloModel> GetArticulosTopico(string hilera) //OK
        {
            //establecer la conexion con la base de datos
            connection();
            //Ejecutar la consulta de un articulo segun su id
            SqlCommand cmd = new SqlCommand("BusquedaTopico", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Palabra", hilera);
            DataTable dt = new DataTable();


            con.Open();
            sd.Fill(dt);
            con.Close();

            List<ArticuloModel> articuloList = new List<ArticuloModel>();

            foreach (DataRow dr in dt.Rows)
            {
                articuloList.Add(
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
            return articuloList;
        }

        public bool GuardarArticuloLargo(ArticuloLargoViewModel articulo) {
            byte[] bytes;
            bool exito = false;
            BinaryReader br = new  BinaryReader(articulo.Archivo1.InputStream); //
            bytes = br.ReadBytes(articulo.Archivo1.ContentLength);
            connection();
            SqlCommand cmd = new SqlCommand("AgregarNuevoArticuloLargo", con); // elegir procedimiento
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Archivo", bytes); // completar resto de parametros
            cmd.Parameters.AddWithValue("@Resumen", articulo.Resumen);
            cmd.Parameters.AddWithValue("@Topico", articulo.Topico);
            cmd.Parameters.AddWithValue("@Titulo", articulo.Titulo);
            cmd.Parameters.AddWithValue("@TipoArchivo",  articulo.Archivo1.ContentType);

            con.Open();

            try
            {
                exito = cmd.ExecuteNonQuery() >= 1;
                                  
            }
            catch (Exception e)
            {
            }
            con.Close();
            if (exito == false)
            {
                return exito;
            }

            DataTable dt = new DataTable();
             
            connection();
            SqlCommand cmd2 = new SqlCommand();            
            cmd2 = new SqlCommand(
                "SELECT articuloId"+ " "+
                "FROM Articulo"+" "+
                "WHERE titulo = @TituloArticulo"+ " ", con);
            cmd2.Parameters.AddWithValue("@TituloArticulo", articulo.Titulo);
            cmd2.CommandType = CommandType.Text;
            SqlDataAdapter sd2 = new SqlDataAdapter(cmd2);
            con.Open();
            sd2.Fill(dt);
            con.Close();

            DataRow dr = dt.Rows[0];
            int articuloIdGuardado = Convert.ToInt32(dr["articuloId"]);

            connection();
            cmd2 = new SqlCommand();
            cmd2 = new SqlCommand(
                "INSERT INTO Publica" + " " +
                "VALUES (@CorreoMiembro,@ArticuloEnviado)" + " " , con);
            cmd2.Parameters.AddWithValue("@CorreoMiembro", articulo.Correo);
            cmd2.Parameters.AddWithValue("@ArticuloEnviado", articuloIdGuardado);
            cmd2.CommandType = CommandType.Text;
            //sd2 = new SqlDataAdapter(cmd2);
            con.Open();
            cmd2.ExecuteNonQuery();
            //sd2.Fill(dt);
            con.Close();
            return exito;
        }

        public Tuple<byte[], string> DescargarArticuloLargo(int id) {
            byte[] bytes;
            string contentType;
            connection();
            using (con)
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT archivo, tipoArchivo FROM Articulo WHERE articuloId=@Id";
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                        bytes = (byte[])sdr["archivo"];
                        contentType = sdr["tipoArchivo"].ToString();
                    }
                    con.Close();
                }
            }
            return new Tuple<byte[], string>(bytes, contentType);
        }

        public List< List< string > > ObtenerAutoresCorreos() {
            List< List<string> > listaNombreCorreos = new List< List<string> >();

            connection();
            string consulta =
                "SELECT nombre  + \' \' + apellido1   AS [nombre completo], correo " +
                "FROM Usuario";
            SqlCommand cmd = new SqlCommand(consulta, con);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();
                       
            foreach (DataRow dr in dt.Rows)
            {
                List<string> listaInterna = new List<string>();

                listaInterna.Add(
                 Convert.ToString(dr["nombre completo"])
                );
                listaInterna.Add(
                    Convert.ToString(dr["correo"])
                );
                listaNombreCorreos.Add(listaInterna);
            }

            return listaNombreCorreos;
        }

        public List<string> ObtenerTopicos()
        {
            List<string> listaTopicos = new List<string>();

            connection();
            string consulta =
                "SELECT DISTINCT topico " +
                "FROM Articulo";
            SqlCommand cmd = new SqlCommand(consulta, con);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();


            foreach (DataRow dr in dt.Rows)
            {
                listaTopicos.Add(Convert.ToString(dr["topico"]));
            }
            return listaTopicos;
        }

        public void PonerEnRevision(int id) {
            connection();
            string consulta = "UPDATE Articulo SET estado= 'Revision' WHERE articuloId=@articuloId";
            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.Parameters.AddWithValue("@articuloId", id);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

        }

        public bool AgregarArticuloAPublicacion(int articuloId, string correoAutor) {
            bool agregado = false;
            connection();
            string consulta = "INSERT INTO Publica VALUES(@correoAutor, @articuloId)";
            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.Parameters.AddWithValue("@articuloId", articuloId);
            cmd.Parameters.AddWithValue("@correoAutor", correoAutor);

            con.Open();
            agregado=cmd.ExecuteNonQuery() >= 1;
            con.Close();

            return agregado;
        }

    }

    public class questionService
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["Grupo3Conn"].ToString();
            con = new SqlConnection(constring);
        }


        // **************** ADD NEW QUESTIOn *********************
        public bool AddQuestion(faqModel preg)
        {
            connection();
            SqlCommand cmd = new SqlCommand("AddNewQuestion", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@pregunta", preg.Pregunta);
            cmd.Parameters.AddWithValue("@respuesta", preg.Respuesta);


            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }


        // ********** VIEW QUESTIOn ********************
        public List<faqModel> GetQuestions()
        {
            connection();
            List<faqModel> studentlist = new List<faqModel>();

            SqlCommand cmd = new SqlCommand("GetQuestions", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                studentlist.Add(
                    new faqModel
                    {
                        Id = Convert.ToInt32(dr["id"]),
                        Pregunta = Convert.ToString(dr["pregunta"]),
                        Respuesta = Convert.ToString(dr["respuesta"]),

                    });
            }
            return studentlist;
        }

        // ***************** UPDATE QUESTIOn *********************
        public bool UpdateQuestion(faqModel preg)
        {
            connection();
            SqlCommand cmd = new SqlCommand("UpdateQuestion", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", preg.Id);
            cmd.Parameters.AddWithValue("@pregunta", preg.Pregunta);
            cmd.Parameters.AddWithValue("@respuesta", preg.Respuesta);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        // ********************** DELETE QUESTIOn *******************
        public bool DeleteQuestion(int id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("DeleteQuestion", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }
    }

}
