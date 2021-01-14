using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Configuration;

namespace AMSDesktop.Config
{
    public class AppConfig : Application
    {
        public static string databaseName = "SQLlite_AMSBrother.sqlite";
        public static string forderPath = "C:\\Program Files (x86)\\Seiko IT Philippines\\AMS_SyncDB";//Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string databasepath = Path.Combine(forderPath, databaseName);

        public static string AMSDB
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["Brothers_AMSDB"].ConnectionString;
            }
        }

        public static string AMSDBSQLite
        {
            get
            {
                return "Data Source=" + databasepath + ";Version=3;";
            }
        }
    }

    
}
