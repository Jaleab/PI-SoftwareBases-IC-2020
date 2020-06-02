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

            SqlCommand cmd = new SqlCommand("ModificarLikes", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IdArticulo", idArticulo);
            cmd.Parameters.AddWithValue("@Puntaje", puntaje);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
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

        public void CrearArticulo(ArticuloModel articulo)
        {
            //establecer la conexion con la base de datos
            connection();


            SqlCommand cmd = new SqlCommand("AgregarNuevoArticuloCorto", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Titulo", articulo.Titulo);
            cmd.Parameters.AddWithValue("@Topico", articulo.Topico);
            cmd.Parameters.AddWithValue("@Contenido", articulo.Contenido);
            cmd.Parameters.AddWithValue("@Resumen", articulo.Resumen);

            con.Open();
            cmd.ExecuteNonQuery();

            con.Close();

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


            Console.WriteLine(articulo.ArticuloId);
            return articulo;
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
            cmd.Parameters.AddWithValue("@Titulo", articulo.Titulo);
            cmd.Parameters.AddWithValue("@Topico", articulo.Topico);
            cmd.Parameters.AddWithValue("@Contenido", articulo.Contenido);
            cmd.Parameters.AddWithValue("@Resumen", articulo.Resumen);

            con.Open();
            cmd.ExecuteNonQuery();

            con.Close();


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

        public void GuardarArticuloLargo(ArticuloLargoViewModel articulo) {
            byte[] bytes;
            BinaryReader br = new  BinaryReader(articulo.Archivo1.InputStream); //
            bytes = br.ReadBytes(articulo.Archivo1.ContentLength);
            connection();
            SqlCommand cmd = new SqlCommand("AgregarNuevoArticuloLargo", con); // elegir procedimiento
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Archivo", bytes); // completar resto de parametros
            cmd.Parameters.AddWithValue("@Resumen", articulo.Resumen);
            cmd.Parameters.AddWithValue("@Topico", "Matemática Largo");
            cmd.Parameters.AddWithValue("@Titulo", articulo.Titulo);
            cmd.Parameters.AddWithValue("@TipoArchivo",  articulo.Archivo1.ContentType);


            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

        }

        public void DescargarArticuloLargo() {

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

            cmd.Parameters.AddWithValue("@pregunta", preg.pregunta);
            cmd.Parameters.AddWithValue("@respuesta", preg.respuesta);


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
                        id = Convert.ToInt32(dr["id"]),
                        pregunta = Convert.ToString(dr["pregunta"]),
                        respuesta = Convert.ToString(dr["respuesta"]),

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

            cmd.Parameters.AddWithValue("@id", preg.id);
            cmd.Parameters.AddWithValue("@pregunta", preg.pregunta);
            cmd.Parameters.AddWithValue("@respuesta", preg.respuesta);

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
