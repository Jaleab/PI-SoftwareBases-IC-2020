using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ComunidadDePracticaMVC.Models;

namespace ComunidadDePracticaMVC.Services
{
    public class FaqService { 
        private SqlConnection con;
        private void Connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["Grupo3Conn"].ToString();
            con = new SqlConnection(constring);
        }


        // **************** ADD NEW QUESTIOn *********************
        public bool AddQuestion(FaqModel preg)
        {
            Connection();
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
        public List<FaqModel> GetQuestions()
        {
            Connection();
            List<FaqModel> studentlist = new List<FaqModel>();

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
                    new FaqModel
                    {
                        Id = Convert.ToInt32(dr["id"]),
                        Pregunta = Convert.ToString(dr["pregunta"]),
                        Respuesta = Convert.ToString(dr["respuesta"]),

                    });
            }
            return studentlist;
        }

        // ***************** UPDATE QUESTIOn *********************
        public bool UpdateQuestion(FaqModel preg)
        {
            Connection();
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
            Connection();
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