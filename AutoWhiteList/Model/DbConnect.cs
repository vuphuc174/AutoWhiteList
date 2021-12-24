using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoWhiteList.Model
{
    class DbConnect
    {
        OleDbConnection strcon = new OleDbConnection();
        public string conn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:/Users/admin/source/repos/AutoWhiteList/AutoWhiteList/Database/whitelist.accdb;Persist Security Info=False;";


        public void OpenConnection()

        {
            strcon.ConnectionString = conn;
            if (strcon.State == ConnectionState.Closed)
            {
                strcon.Open();
            }
        }
        public void CloseConnection()

        {

            if (strcon.State == ConnectionState.Open)
            {
                strcon.Close();
            }
        }
        public Boolean exedata(string cmd)
        {
            OpenConnection();
            Boolean check = false;
            try
            {
                OleDbCommand sc = new OleDbCommand(cmd, strcon);
                sc.ExecuteNonQuery();
                check = true;
            }
            catch (Exception)
            {
                check = false;
            }
            CloseConnection();
            return check;
        }
        public DataTable readdata(string cmd)
        {
            OpenConnection();
            DataTable da = new DataTable();
            try
            {
                OleDbCommand sc = new OleDbCommand(cmd, strcon);
                OleDbDataAdapter sda = new OleDbDataAdapter(sc);
                sda.Fill(da);
            }
            catch (Exception)
            {
                da = null;
            }
            CloseConnection();
            return da;
        }
    }
}
