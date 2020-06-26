using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ComunidadDePracticaMVC.Services
{
    public class ChartService
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["Grupo3Conn75"].ToString();
            con = new SqlConnection(constring);
        }

        public List<string> ObtenerPaises()
        {
            List<string> listaPaises = new List<string>();
            connection();   
            string consulta =
               "SELECT DISTINCT pais " +
               "FROM Usuario";

            SqlCommand cmd = new SqlCommand(consulta, con);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();


            foreach (DataRow dr in dt.Rows)
            {
                listaPaises.Add(Convert.ToString(dr["pais"]));
            }

            return listaPaises;
        }

        public List<int> ObtenerCuentaPaises()
        {
            List<String> paises = this.ObtenerPaises(); 
            List<int> listaValores = new List<int>();
            connection();


            string consulta =
               "SELECT COUNT(pais) AS 'VALOR' " +
               "FROM Usuario " + 
               "WHERE pais = @paisParam";

            SqlCommand cmd = new SqlCommand(consulta, con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            for (int i = 0; i < paises.Count(); ++i)
            {
                cmd.Parameters.Clear(); 
                cmd.Parameters.AddWithValue("@paisParam", paises[i]);
                dt.Clear();  
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                con.Open();
                sd.Fill(dt);
                con.Close();

                DataRow dr = dt.Rows[0];
                listaValores.Add(Convert.ToInt32(dr["VALOR"])); 
            }
            return listaValores;
        }
    }
}