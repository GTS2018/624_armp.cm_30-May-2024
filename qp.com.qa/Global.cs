using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Net;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.SqlTypes;

namespace GlobalLevel
{
    internal static class Global
    {
        internal static bool flagServer = true;
        internal static bool nextpage = true;
        internal static bool NonEnglish = true;
        
        internal static int duplicate = 0;
        internal static int inserted = 0;
        internal static int qccount = 0;
        internal static int skipped = 0;
        internal static int expired = 0;


        internal static string server = "";
        internal static string Fromdate = "";
        internal static string Todate = "";
        internal static string DocPath = "";
        internal static string DataHtmlDoc = "";
        internal static string attach_link_withname = "";

        internal static List<string> Stage = new List<string>();
        internal static List<string> Doc_Links = new List<string>(); 
        internal static List<string> Doc_Data = new List<string>();
        
    }
}
