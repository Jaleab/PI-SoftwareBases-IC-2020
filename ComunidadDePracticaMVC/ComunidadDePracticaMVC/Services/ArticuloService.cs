using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using ComunidadDePracticaMVC.Models;
using System.Web.Mvc;

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

        public int ArticuloIdInDB(ArticuloModel  articulo)
        {
            int Id = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Grupo3Conn75"].ToString()))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from Articulo", con);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string titulo = row["Titulo"].ToString(); 
                        if(titulo == articulo.Titulo)
                        {
                            Id = (int)row["ArticuloId"]; 
                        }
                    }
                }
                else
                { 
                    con.Close();
                }
            }
            return Id;
        }

        public List<SelectListItem> FillList()
        {
            var list = new List<SelectListItem>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Grupo3Conn75"].ToString()))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("select * from Usuario", con);
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    dataAdapter.Fill(dt);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            list.Add(new SelectListItem { Text = row["nombre"].ToString() + " " + row["apellido1"].ToString(), Value = row["correo"].ToString() });
                        }
                    }
                    else
                    {
                        list.Add(new SelectListItem { Text = "No records found", Value = "0" });
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                list.Add(new SelectListItem { Text = ex.Message.ToString(), Value = "0" });
            }

            return list;
        }
 

    // ******************** ADD NEW ARTICULO ********************
    public List<ArticuloModel> GetArticulos()
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
                        ArticuloId = Convert.ToInt32(dr["ArticuloId"]),
                        Autor = Convert.ToString(dr["Autor"]),
                        Titulo = Convert.ToString(dr["Titulo"]),
                        Resumen = Convert.ToString(dr["Resumen"]),
                        Topico = Convert.ToString(dr["Topico"]),
                        Contenido = Convert.ToString(dr["Contenido"])                        
            });
            }
            return Articulolist;
        }

        public void CrearArticulo(ArticuloModel articulo)
        {
            //establecer la conexion con la base de datos
            connection();  
            
            SqlCommand cmd = new SqlCommand("AgregarNuevoArticulo", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Titulo", articulo.Titulo);
            cmd.Parameters.AddWithValue("@Topico", articulo.Topico); 
            cmd.Parameters.AddWithValue("@Contenido", articulo.Contenido);
            cmd.Parameters.AddWithValue("@Resumen", articulo.Resumen);
            cmd.Parameters.AddWithValue("@TipoArchivo", "corto");
            cmd.Parameters.AddWithValue("@FechaPublicacion", articulo.FechaPublicacion);
         

            int Id = ArticuloIdInDB(articulo);


            SqlCommand cmd1 = new SqlCommand("AgregarNuevoArticuloPublica", con);
            cmd.Parameters.AddWithValue("@ArticuloId", Id);
            cmd.Parameters.AddWithValue("@Autor", articulo.Autor);

            con.Open();
            cmd.ExecuteNonQuery();

            con.Close();
            
        }

        public ArticuloModel GetInfoArticulo(int id)
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
            articulo.TipoArchivo = Convert.ToString(dr["tipoArchivo"]);
            articulo.FechaPublicacion = Convert.ToDateTime(dr["fechaPublicacion"]);


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

        public List<ArticuloModel> GetArticulosTopico(string hilera)
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
