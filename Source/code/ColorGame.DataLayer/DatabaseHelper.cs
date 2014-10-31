using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Configuration;

namespace ColorGame.DataLayer
{
    public class DatabaseHelper
    {
        private static string _connectionString = string.Empty;
        public static bool IsDbCreated { get; set; }

        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = ConfigurationManager.ConnectionStrings["ColorGameConnectionString"].ConnectionString;
                }

                return _connectionString;
            }
        }
    }
}