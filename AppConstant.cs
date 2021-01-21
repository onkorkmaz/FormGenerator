using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FormGenerator
{
    public class AppConstant
    {
        /// <summary>
        /// senin lokal server adı
        /// </summary>
        public const String LocalServerName = "DESKTOP-U63HM5J";
        public const bool IsUseRemoteServer = false;
        public const String ServerName = "DESKTOP-U63HM5J";
        public const string UserId = "";
        public const string password = "";
        public static String DatabaseName = "dbTest";





        public static String TableName = "";
        public static string HtmlCode = "";
        public static string CodeBehind = "";
        public static string InUpDelSpName = "";
        public static string SelSpName = "";
        public static bool IsSp = true;
        public static string FillEntitiyCode = "";
        public static string EntityName = "";

        public static string SelectMethodName = "";
        public static string InUpDelMethodName = "";

        public static string GetConnectionString
        {
            get
            {
                if (AppConstant.IsUseRemoteServer)
                    return "Data Source=" + AppConstant.ServerName + "; User ID=" + AppConstant.UserId + "; Password=" + AppConstant.password + ";";
                else
                    return "Data Source=" + LocalServerName + "; Integrated Security=SSPI;";
            }
        }
    }
}
