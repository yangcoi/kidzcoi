using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace FantasyAdventures
{
    internal class Database
    {
        static string conn_string =
            @"Data Source=yangcoi;Initial Catalog=FANTASYADVENTURES;Integrated Security=True";
        static SqlConnection conn;
        static SqlCommand command;

        public static SqlConnection CreateConnection()
        {
            try
            {
                conn = new SqlConnection(conn_string);
                conn.Open();
            }
            catch (Exception err)
            {
                conn = null;
            }
            return conn;
        }

        public static SqlCommand CreateCommand(string strCommand)
        {
            try
            {
                command = new SqlCommand(strCommand, conn);
            }
            catch (Exception err)
            {
                command = null;
            }
            return command;
        }

        public static DataTable SelectQuery(string sql)
        {
            command = CreateCommand(sql);
            SqlDataAdapter adt = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            adt.Fill(dt);
            command.Dispose();
            adt.Dispose();
            return dt;
        }
    }
}
