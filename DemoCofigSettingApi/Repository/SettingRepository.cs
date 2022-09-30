using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DemoCofigSettingApi.Models;

namespace DemoCofigSettingApi.Repository
{
    public class SettingRepository
    {
        private SqlConnection con;
        //To Handle connection related activities    
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["getconn"].ToString();
            con = new SqlConnection(constr);

        }
        //To Add Employee details    
        public bool AddSetting(Setting obj)
        {

            connection();
            SqlCommand com = new SqlCommand("AddSetting", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Url", obj.Url);
            com.Parameters.AddWithValue("@Token", obj.Token);
            com.Parameters.AddWithValue("@NameUnit", obj.NameUnit);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {

                return true;

            }
            else
            {

                return false;
            }


        }
        //To view employee details with generic list     
        public List<Setting> GetAllSetting()
        {
            connection();
            List<Setting> List = new List<Setting>();


            SqlCommand com = new SqlCommand("GetSetting", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            con.Open();
            da.Fill(dt);
            con.Close();
            //Bind Setting generic list using dataRow     
            foreach (DataRow dr in dt.Rows)
            {

                List.Add(

                    new Setting
                    {

                        Id = Convert.ToInt32(dr["Id"]),
                        Url = Convert.ToString(dr["Url"]),
                        Token = Convert.ToString(dr["Token"]),
                        NameUnit = Convert.ToString(dr["NameUnit"])

                    }
                    );
            }

            return List;
        }
        //To Update Employee details    
        public bool UpdateSetting(Setting obj)
        {

            connection();
            SqlCommand com = new SqlCommand("UpdateSetting", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@SettingId", obj.Id);
            com.Parameters.AddWithValue("@Url", obj.Url);
            com.Parameters.AddWithValue("@Token", obj.Token);
            com.Parameters.AddWithValue("@NameUnit", obj.NameUnit);
            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {

                return true;
            }
            else
            {
                return false;
            }
        }
        //To delete Employee details    
        public bool DeleteSetting(int Id)
        {

            connection();
            SqlCommand com = new SqlCommand("DeleteSetting", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@SettingId", Id);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                return true;
            }
            else
            {

                return false;
            }
        }
    }
}