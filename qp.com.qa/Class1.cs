using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Data;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.IO;
using static System.Windows.Forms.LinkLabel;

namespace ScrapClass
{
    public class Class1
    {
        MySqlConnection myConnection;
        MySqlDataReader myDataReader;

        string dms_entrynotice_tblstatus = "1"; // 1=no cpv alloted,2=cpv alloted  -  Default -  1 //column name = status
        string dms_entrynotice_tblquality_status = "1";// 1=no quality check, 2=quality check done //column name = quality_status // When QC Done marcked 2
        string dms_entrynotice_tbl_cqc_status = "1";// 1- Pedning, 2- Done -  Default - 1 //column name = cqc_status // for Central QC
        string dms_entrynotice_tblnotice_type = "2"; //SegFields[14] = "2"; //notice_type 2 = tenders
        string dms_entrynotice_tblcompulsary_qc = "2";//1 For compulsary_qc

        string dms_downloadfiles_tblstatus = "1"; //1=active, 2=high, 3=low, 4=reject, 5=verylow - Default 1 /////1 is to stop it in selection status-- //column name = status
        string dms_downloadfiles_tblsave_status = "1"; //1=unsaved, 2=saved - Default 1 //column name = save_status// 2 means Manual Entry done in entrynotice table / 1 that row still in dwn table and no entry in ent table
        string dms_downloadfiles_tbldatatype = "A"; // A = Automation entry, M = Manual Manual
        string dms_downloadfiles_tbluser_id = "DWN302520";//Europe=DWN2554488, Asia=DWN5046627, India=DWN00541021, Africa=DWN302520, MFA(Funding)=DWN0654200, Semi-Auto=DWN30531073, North America=DWN1011566, South America=DWN1456891, Oceania=DWN3708839, Ted=DWN2243352, India High (Defence)=DWN4127150, Asia Low=DWN0558703, Middle East=Asia

        string ncb_icb = "icb"; // ncb = national tender, icb = international tender
        string exe_no = "TA624";
        string source = "armp.cm";
        string country_code = "CM".ToUpper(); // Add MULTI = If country dynamic
        string source_doamin = "https://www.armp.cm/ ";
        string local_table_name = "africa_tenders_tbl"; // Local table name for Duplicate check
        int is_english = 1; // 0 = English, 1 = Non-English
        
        int file_upload = 1; //0=Upload by L2L, 1=Upload by EXE

        public string MySqlQuery = ""; string OnLeft = ""; string OnRight = ""; string OnRight0 = ""; string OnRight1 = ""; string OnRight2 = ""; string ReplyStrings = ""; string MyWorkBuff = ""; string MyNullString = ""; string MySingleSpace = " ";
        public int Posting_Id = 0; int MyBegin = -1; int MyEnd = -1; int MyCurr1 = -1; int MyCurr2 = -1; int MyCurr = -1; int MyReturnValue = 1; int MyReturnCode = 0;
        List<string> RequiredFields = new List<string>(); List<string> RequiredPages = new List<string>(); int rflenght = 50; int rplenght = 500;
        int errorcount = 0; int erromsg = 0;


        public int SegDoc(string HtmlDoc, string DocPath, string attach_link_withname)
        {
            MyReturnValue = 0;
            HtmlDoc = "<!DOCTYPE html><html><head><meta charset=\"utf-8\"/><title>Tender Document</title><body><Blockquote style='border:1px solid; padding:20px; margin: 10px 30px 10px 30px;'>" + HtmlDoc + "</Blockquote></body></html>";
            HtmlDoc = System.Net.WebUtility.HtmlDecode(HtmlDoc).Trim();
            ScrapData(HtmlDoc, DocPath, attach_link_withname);
            try
            {
                if (RequiredPages[240] != "")
                {
                    DateTime dt = DateTime.ParseExact(RequiredPages[240], "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime dt2 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                    TimeSpan ts = dt - dt2;
                    int days = ts.Days;
                    if (days > 0)
                    {
                        if (RequiredPages[190] == "")
                        {
                            MessageBox.Show("ShortDesc Blank !!!");
                            GlobalLevel.Global.skipped++;
                        }
                        else
                        {
                           InsertRecord_Local(RequiredFields, HtmlDoc);
                        }
                    }
                    else
                    {
                        GlobalLevel.Global.expired++;
                    }
                }
                else
                {
                    if (RequiredPages[190] == "")
                    {
                        MessageBox.Show("ShortDesc Blank !!!");
                        GlobalLevel.Global.skipped++;
                    }
                    else
                    {
                        InsertRecord_Local(RequiredFields, HtmlDoc);
                    }
                }

            }
            catch
            {
                GlobalLevel.Global.skipped++;
            }
            return MyReturnCode;
        }
        public void ScrapData(string HtmlDoc, string DocPath,string attach_link_withname)
        {
            RequiredFields.Clear();
            for (int i = 0; i < rflenght; i++)
            {
                RequiredFields.Add(MyNullString);
            }
            RequiredPages.Clear();
            for (int i = 0; i < rplenght; i++)
            {
                RequiredPages.Add(MyNullString);
            }
            OnLeft = MyNullString;
            OnRight0 = MyNullString;
            OnRight1 = MyNullString;
            OnRight2 = MyNullString;
            ReplyStrings = MyNullString;
            HtmlDoc = HtmlDoc.Replace("&amp;", "&").Trim();
            HtmlDoc = HtmlDoc.Replace("&nbsp;", " ").Trim();
            ReplyStrings = HtmlDoc;

            //****************************Address********************************20
            RequiredPages[20] = "Cameroon <br>\n[Disclaimer: For Exact Organization/Tendering Authority details, please refer to the tender notice.]";

            //****************************Country********************************70 to 80
            if (country_code == "MULTI")
            {
                OnLeft = "countryname</td>";
                OnRight0 = "</tr>";
                OnRight1 = "";
                OnRight2 = "";
                ReplyStrings = GetRqdStr(HtmlDoc, OnLeft, OnRight0, OnRight1, OnRight2);
                ReplyStrings = Regex.Replace(ReplyStrings, @"<[^>]*>", String.Empty).Trim();
                //ReplyStrings = ReplaceOthers(ReplyStrings);
                ReplyStrings = Getcountry_Code(ReplyStrings);
                RequiredPages[70] = ReplyStrings;
            }
            else
            {
                RequiredPages[70] = country_code;
            }

            //=============== [Purchaser] ========================120
            OnLeft = "Tender Document</title>";
            OnRight0 = "</DIV>";
            OnRight1 = "";
            OnRight2 = "";
            GetRqdStr(HtmlDoc, OnLeft, OnRight0, OnRight1, OnRight2);
            ReplyStrings = Regex.Replace(ReplyStrings, @"<[^>]*>", String.Empty).Trim();
            ReplyStrings = ReplyStrings.Replace(":", "").Trim();
            RequiredPages[120] = ReplyStrings.Trim();

            //=============== [TenderTitle] ========================190
            OnLeft = "";
            OnRight0 = "♥";
            OnRight1 = "";
            OnRight2 = "";
            GetRqdStr(DocPath, OnLeft, OnRight0, OnRight1, OnRight2);
            ReplyStrings = Regex.Replace(ReplyStrings, @"<[^>]*>", String.Empty).Trim();
            ReplyStrings = ReplyStrings.Replace("&nbsp;", "");
            ReplyStrings = ReplyStrings.Replace(".", "").Trim();
            RequiredPages[190] = ReplyStrings.Trim();

            //****************************doc_cost(Amount)********************************220 to 230
            OnLeft = "•";
            OnRight0 = "☺";
            OnRight1 = "";
            OnRight2 = "";
            GetRqdStr(DocPath, OnLeft, OnRight0, OnRight1, OnRight2);
            ReplyStrings = Regex.Replace(ReplyStrings, @"<[^>]*>", String.Empty).Trim();
            ReplyStrings = ReplyStrings.Replace("&nbsp;", "");
            ReplyStrings = ReplyStrings.Replace(".", "").Trim();
            RequiredPages[200] = ReplyStrings.Trim();

            if(RequiredPages[200] != "")
            {
                //****************************currency********************************210 to 220
                RequiredPages[210] = "CFA";
            }


            //=============== [Closing Date.] ========================240 to 250
            OnLeft = "♦";
            OnRight0 = "•";
            OnRight1 = "";
            OnRight2 = "";
            GetRqdStr(DocPath, OnLeft, OnRight0, OnRight1, OnRight2);
            ReplyStrings = Regex.Replace(ReplyStrings, @"<[^>]*>", String.Empty).Trim();
            ReplyStrings = ReplyStrings.Replace("&nbsp;", "");
            ReplyStrings = ReplyStrings.Replace(".", "").Trim();
            try
            {
                DateTime MyDateTime = new DateTime();
                MyDateTime = DateTime.ParseExact(ReplyStrings, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string deadline = MyDateTime.ToString("yyyy-MM-dd");
                RequiredPages[240] = deadline.ToString();
            }
            catch
            {
                try
                {
                    DateTime MyDateTime = new DateTime();
                    MyDateTime = Convert.ToDateTime(ReplyStrings);
                    string deadline = MyDateTime.ToString("yyyy-MM-dd");
                    RequiredPages[240] = deadline.ToString();
                }
                catch
                {
                    
                }
            }

            //****************************notice_type********************************140 to 150
            RequiredPages[140] = dms_entrynotice_tblnotice_type;

            //****************************ind_classification********************************150 to 160

            //****************************global********************************160 to 170
            RequiredPages[160] = "1";

            //****************************MFA********************************170 to 180
            RequiredPages[170] = "0";

            //****************************tender_doc_file********************************280 to 290
            RequiredPages[280] = DocPath.Substring(DocPath.IndexOf("☺") + "☺".Length);

            //****************************tender_Add_doc_file********************************440 to 450
            RequiredPages[440] = attach_link_withname;

            //****************************source********************************310 to 320
            RequiredPages[310] = source;

            RequiredPages[270] = "0";

            RequiredPages[420] = RequiredPages[70];

            //RequiredPages[260] = "0"; //Emd Value

            //RequiredPages[200] = "0"; //tender value

            //RequiredPages[220] = "0"; // Doc Fees

            RequiredPages[410] = ""; // Dos Selling Last Date


            if (RequiredPages[180] == "")
            {
                RequiredPages[180] = RequiredPages[190];
            }
            if (RequiredPages[190].Length > 200)
            {
                if (RequiredPages[180] != RequiredPages[190])
                {
                    RequiredPages[180] = RequiredPages[190] + "\n<br>" + RequiredPages[180];
                }
                RequiredPages[190] = RequiredPages[190].Remove(200).ToString().Trim() + "...";

            }
        }
        public string Getcountry_Code(string country_name)
        {
            string country_code = "";
            if (country_name.ToLower().Contains("congo, dem. republic") || country_name.ToLower().Contains("cote d ivoire") || country_name.ToLower().Contains("fyr macedonia") || country_name.ToLower().Contains("korea (democratic people s republic of)") || country_name.ToLower().Contains("korea (republic of)") || country_name.ToLower().Contains("palestinian territories") || country_name.ToLower().Contains("slovak republic") || country_name.ToLower().Contains("united states of america") || country_name.ToLower().Contains("viet nam") || country_name.ToLower().Contains("antarctica") || country_name.ToLower().Contains("kosovo"))
            {
                switch (country_name.ToLower().ToString())
                {
                    case "congo, dem. republic":
                        country_code = "CG";
                        break;
                    case "cote d ivoire":
                        country_code = "CI";
                        break;
                    case "fyr macedonia":
                        country_code = "MK";
                        break;
                    case "korea (democratic people s republic of)":
                        country_code = "KP";
                        break;
                    case "korea (republic of)":
                        country_code = "KR";
                        break;
                    case "palestinian territories":
                        country_code = "PS";
                        break;
                    case "slovak republic":
                        country_code = "SK";
                        break;
                    case "united states of america":
                        country_code = "US";
                        break;
                    case "viet nam":
                        country_code = "VN";
                        break;
                    case "antarctica":
                        country_code = "AQ";
                        break;
                    case "kosovo":
                        country_code = "XK";
                        break;
                }
            }
            else
            {
                if (country_name.Length > 2)
                {
                    int c = 0;
                    while (c == 0)
                    {
                        try
                        {
                            GetConnection();
                            var CommandText = "SELECT Code FROM dms_country_tbl WHERE Country LIKE '%" + country_name + "%'";
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.CommandText = CommandText;
                                cmd.Connection = myConnection;
                                cmd.CommandTimeout = 300;
                                myDataReader = cmd.ExecuteReader();
                                if (myDataReader.Read())
                                {
                                    country_code = myDataReader[0].ToString();
                                }
                                c = 1;
                            }
                            ShutConnection();
                        }
                        catch (Exception ex)
                        {
                            ShutConnection();
                            ShutDataReader();
                            CreateErrorLog(ex.Message.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), Application.ProductName.ToString(), "");
                            c = 0;
                        }
                    }
                }
                else
                {
                    country_code = "";
                }
            }
            return country_code;
        }
        public string Download_AdditionalDocs(string url_name, string fileid)
        {
            string[] urlnameArr = url_name.Split('~');
            if (urlnameArr[0].Trim() == "" || urlnameArr.Length == 1)
            {
                return "";
            }
            string filename = fileid + "-" + urlnameArr[0];
            string url = urlnameArr[1];
            int d = 0;
            while (d <= 5)
            {
                try
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    using (WebClient webClient = new WebClient())
                    {
                        if (!Directory.Exists(@"Additional_Docs"))
                        {
                            Directory.CreateDirectory(@"Additional_Docs");
                        }
                        string fullpath = @"Additional_Docs\" + filename;
                        webClient.DownloadFile(url, fullpath);
                        if (!File.Exists(fullpath))
                        {
                            filename = "";
                        }
                        else
                        {
                            FileInfo fi = new FileInfo(fullpath);
                            long size = fi.Length;
                            if (size <= 0)
                            {
                                filename = "";
                            }
                        }
                    }
                    d = 10;
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Error in FileUploadFileZila Method.\nError is: " + ex.Message, Application.ProductName.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Threading.Thread.Sleep(30000);
                    filename = "";
                    d++;
                }
            }
            return filename;
        }
        private void FileUpload_AWS(string filePath, string DirectoryInBucket)
        {
            int flage = 0;
            while (flage == 0)
            {
                try
                {
                    bool is_Uploaded = TOTS3UploadLibrary.UploadClass.UploadFile(filePath, DirectoryInBucket);
                    if (is_Uploaded == true)
                    {
                        flage = 1;//success
                    }
                    else
                    {
                        MessageBox.Show("Error in '" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "' Method.\nError while uploading file on S3 Bucket..!", Application.ProductName.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in '" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "' Method.\nError is: " + ex.Message, Application.ProductName.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public int InsertRecord_Local(List<string> SegFields, string HtmlDoc)
        {
            CombineParasInRequiredFields();
            SegFields = Fields_Validation(SegFields);
            #region Add links and filename for Download additional docs
            List<string> allLinks = new List<string>();
            try
            {
                string attach_docs = SegFields[44];
                if (attach_docs != "")
                {
                    string[] doclinks = attach_docs.Split(',');
                    foreach (string link in doclinks)
                    {
                        allLinks.Add(link);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in '" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "' Method.\nError is: " + ex.Message, Application.ProductName.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            #endregion
            if (is_english == 0) // 1 = // Comment in other than English language // title case and ToUppar after translate in translation exe.
            {
                SegFields[19] = make_title_case(SegFields[19]);
                SegFields[12] = SegFields[12].ToUpper();
            }
            myDataReader = MyReader(SegFields);
            if (myDataReader.HasRows == true)
            {
                GlobalLevel.Global.duplicate++;
                ShutDataReader();
                ShutConnection();
                return 1;
            }
            else
            {
                string HtmlDaoc_fileid = "";
                if (file_upload == 1)
                {
                    HtmlDaoc_fileid = Upload_HtmlDoc_file_AWS(HtmlDoc); // upload htmlDoc file directly on aws
                }
                else
                {
                    HtmlDaoc_fileid = Upload_HtmlDoc_file(HtmlDoc); // upload htmlDoc file in Z: Drive
                }
                #region Download and Upload additional docs and get file name
                try
                {
                    string additional_docname = "";
                    SegFields[44] = ""; // additional docs file name which are uploaded on AWS only
                    for (int i = 0; i < allLinks.Count; i++)
                    {
                        string name_n_file = allLinks[i];

                        #region limit SegFields[44]/filename
                        try
                        {
                            string[] urlnameArr = name_n_file.Split('~');
                            additional_docname += HtmlDaoc_fileid + "-" + urlnameArr[0] + ",";
                            if (additional_docname.Length > 3800)
                            {
                                save_txt("toomany_additional_docs.txt", SegFields[28]);
                                break;
                            }
                        }
                        catch
                        {

                        }
                        #endregion

                        string down_filename = Download_AdditionalDocs(name_n_file, HtmlDaoc_fileid);
                        if (down_filename != "")
                        {
                            string down_file_path = "Additional_Docs/" + down_filename;
                           FileUpload_AWS(down_file_path, "additional_docs");
                            if (SegFields[44] == "")
                            {
                                SegFields[44] = down_filename;
                            }
                            else
                            {
                                SegFields[44] += "," + down_filename;
                            }
                        }
                        else
                        {

                        }
                    }
                    if (allLinks.Count > 0)
                    {
                        clearFolder("Additional_Docs");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in '" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "' Method.\nError is: " + ex.Message, Application.ProductName.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                #endregion
                ShutDataReader();
                if (SegFields[02] == "" || SegFields[07] == "" || SegFields[12] == "" || SegFields[18] == "" || SegFields[19] == "" || SegFields[24] == "") //If true than stop this tender in compulsory QC
                {
                    dms_entrynotice_tblcompulsary_qc = "1";
                    GlobalLevel.Global.qccount++;
                }
                else
                {
                    dms_entrynotice_tblcompulsary_qc = "2";
                }

                if (SegFields[36] != "") //If CPV Available than bypass all internal process with high mark tender
                {
                    dms_entrynotice_tblstatus = "2";
                    dms_downloadfiles_tblsave_status = "2";
                    dms_downloadfiles_tblstatus = "2";
                    dms_entrynotice_tbl_cqc_status = "2";
                }
                else
                {
                    dms_entrynotice_tblstatus = "1";
                    dms_downloadfiles_tblsave_status = "1";
                    dms_downloadfiles_tblstatus = "1";
                    dms_entrynotice_tbl_cqc_status = "1";
                }

                int MyLoop = 0;
                while (MyLoop == 0)
                {
                    string date = System.DateTime.Now.ToString("yyyy-MM-dd").ToString();
                    string inquery = "INSERT INTO " + local_table_name + "( `Tender_ID`, `EMail`, `add1`, `add2`, `City`, `State`, `PinCode`, `Country`, `URL`, `Tel`, `Fax`, `Contact_Person`, `Maj_Org`, `tender_notice_no`, `notice_type`, `ind_classification`,  `global`, `MFA`, `tenders_details`, `short_desc`, `est_cost`, `currency`, `doc_cost`, `doc_start`, `doc_last`, `open_date`, `earnest_money`, `Financier`, `tender_doc_file`, `Sector`, `corregendum`, `source`, `entry_date`, `cpv`) " +
                                     "VALUES ( @Tender_ID, @EMail, @add1, @add2, @City, @State, @PinCode, @Country, @URL, @Tel, @Fax, @Contact_Person, @Maj_Org, @tender_notice_no, @notice_type, @ind_classification,  @global, @MFA, @tenders_details, @short_desc, @est_cost, @currency, @doc_cost, @doc_start, @doc_last, @open_date, @earnest_money, @Financier, @tender_doc_file, @Sector, @corregendum, @source, @entry_date, @cpv)";
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Parameters.AddWithValue("@Tender_ID", HtmlDaoc_fileid.ToString());
                        cmd.Parameters.AddWithValue("@EMail", SegFields[1].ToString());
                        cmd.Parameters.AddWithValue("@add1", SegFields[2].ToString());
                        cmd.Parameters.AddWithValue("@add2", SegFields[3].ToString());
                        cmd.Parameters.AddWithValue("@City", SegFields[4].ToString());
                        cmd.Parameters.AddWithValue("@State", SegFields[5].ToString());
                        cmd.Parameters.AddWithValue("@PinCode", SegFields[6].ToString());
                        cmd.Parameters.AddWithValue("@Country", SegFields[7].ToString());
                        cmd.Parameters.AddWithValue("@URL", SegFields[8].ToString());
                        cmd.Parameters.AddWithValue("@Tel", SegFields[9].ToString());
                        cmd.Parameters.AddWithValue("@Fax", SegFields[10].ToString());
                        cmd.Parameters.AddWithValue("@Contact_Person", SegFields[11].ToString());
                        cmd.Parameters.AddWithValue("@Maj_Org", SegFields[12].ToString());
                        cmd.Parameters.AddWithValue("@tender_notice_no", SegFields[13].ToString());
                        cmd.Parameters.AddWithValue("@notice_type", SegFields[14].ToString());
                        cmd.Parameters.AddWithValue("@ind_classification", SegFields[15].ToString());
                        cmd.Parameters.AddWithValue("@global", SegFields[16].ToString());
                        cmd.Parameters.AddWithValue("@MFA", SegFields[17].ToString());
                        cmd.Parameters.AddWithValue("@tenders_details", "");
                        cmd.Parameters.AddWithValue("@short_desc", SegFields[19].ToString());
                        cmd.Parameters.AddWithValue("@est_cost", SegFields[20].ToString());
                        cmd.Parameters.AddWithValue("@currency", SegFields[21].ToString());
                        cmd.Parameters.AddWithValue("@doc_cost", SegFields[22].ToString());
                        cmd.Parameters.AddWithValue("@doc_start", SegFields[23].ToString());
                        cmd.Parameters.AddWithValue("@doc_last", SegFields[24].ToString());
                        cmd.Parameters.AddWithValue("@open_date", SegFields[25].ToString());
                        cmd.Parameters.AddWithValue("@earnest_money", SegFields[26].ToString());
                        cmd.Parameters.AddWithValue("@Financier", SegFields[27].ToString());
                        cmd.Parameters.AddWithValue("@tender_doc_file", SegFields[28].ToString());
                        cmd.Parameters.AddWithValue("@Sector", SegFields[29].ToString());
                        cmd.Parameters.AddWithValue("@corregendum", SegFields[30].ToString());
                        cmd.Parameters.AddWithValue("@source", source.ToString());
                        cmd.Parameters.AddWithValue("@entry_date", date);
                        cmd.Parameters.AddWithValue("@cpv", SegFields[36].ToString());
                        try
                        {
                            cmd.CommandTimeout = 300;
                            cmd.CommandText = inquery;
                            cmd.Connection = myConnection;
                            cmd.ExecuteNonQuery();
                            GlobalLevel.Global.inserted++;
                            MyLoop = 1;
                        }
                        catch (Exception ex)
                        {
                            CreateErrorLog(ex.Message.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), Application.ProductName.ToString(), SegFields[28].ToString());
                            if (ex.Message.Contains("Duplicate entry"))
                            {
                                MyLoop = 1;
                            }
                            else if (ex.Message.Contains("Fatal error encountered during command execution."))
                            {
                                ShutConnection();
                                GetConnection();
                                MyLoop = 0;
                            }
                            else
                            {
                                ShutConnection();
                                GetConnection();
                                MyLoop = 0;
                            }
                        }
                    }
                }
                if (GlobalLevel.Global.flagServer == false)
                {

                }
                else
                {
                    InsertRecord_L2L(SegFields, HtmlDoc, HtmlDaoc_fileid);
                }
            }
            ShutConnection();
            return MyReturnValue;
        }
        public void save_txt(string filePath, string data)
        {
            try
            {
                System.IO.FileStream fs = null;
                if (!System.IO.File.Exists(filePath))
                {
                    using (fs = System.IO.File.Create(filePath)) { }
                }
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath, true))
                {
                    writer.WriteLine(data);
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in '" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "' Method.\nError is: " + ex.Message, Application.ProductName.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public string Upload_HtmlDoc_file(string HtmlDoc)
        {
            System.Threading.Thread.Sleep(1000);
            string id = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            Int64 posting_id = Int64.Parse(id.ToString());
            posting_id++;
            string file_id = exe_no + posting_id;
            int a = 0;
            while (a == 0)
            {
                try
                {
                    if (System.IO.Directory.Exists(@"Z:"))
                    {
                        string strOpFileName = "Z:\\" + file_id.ToString() + ".html";
                        System.IO.StreamWriter swOut = new System.IO.StreamWriter(strOpFileName, false, Encoding.UTF8);
                        swOut.Write(HtmlDoc);
                        swOut.Close();
                        a++;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Please Map DocData folder..and Create Z: Drive", "'" + source.ToString() + "'");
                    }
                }
                catch (Exception ex)
                {
                    a = 0;
                }
            }
            return file_id;
        }
        public int InsertRecord_L2L(List<string> SegFields, string HtmlDoc, string HtmlDaoc_fileid)
        {
            string search_id = "1";
            string cpv_user_id = string.Empty;
            string quality_id = "1";
            string selector_id = string.Empty;

            string col1 = source_doamin;
            string col2 = string.Empty;
            string col3 = string.Empty;
            string col4 = string.Empty;
            string col5 = string.Empty;

            if (SegFields[07] == "IN")
            {
                //col5 = SegFields[03].ToString(); //address email telfax for india product client
            }

            #region QCTenders Table
            if (dms_entrynotice_tblcompulsary_qc == "1")
            {
                int MyLoop1 = 0;
                while (MyLoop1 == 0)
                {
                    MyLoop1 = 1;
                    string inquery = "INSERT INTO qctenders_tbl (Source, tender_notice_no, short_desc, doc_last, Maj_Org, Address, doc_path, Country) VALUES ( @Source, @tender_notice_no, @short_desc, @doc_last, @Maj_Org, @Address, @doc_path, @Country)";
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Parameters.AddWithValue("@Source", source.ToString());
                        cmd.Parameters.AddWithValue("@tender_notice_no", SegFields[13].ToString());
                        cmd.Parameters.AddWithValue("@short_desc", SegFields[19].ToString());
                        cmd.Parameters.AddWithValue("@doc_last", SegFields[24].ToString());
                        cmd.Parameters.AddWithValue("@Maj_Org", SegFields[12].ToString());
                        cmd.Parameters.AddWithValue("@Address", SegFields[02].ToString());
                        cmd.Parameters.AddWithValue("@doc_path", "http://tottestupload3.s3.amazonaws.com/" + HtmlDaoc_fileid + ".html");
                        cmd.Parameters.AddWithValue("@Country", SegFields[07].ToString());
                        try
                        {
                            cmd.CommandTimeout = 300;
                            cmd.CommandText = inquery;
                            cmd.Connection = myConnection;
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            CreateErrorLog(ex.Message.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), Application.ProductName.ToString(), SegFields[28].ToString());
                            if (ex.Message.Contains("Duplicate entry"))
                            {
                                MyLoop1 = 1;
                            }
                            else if (ex.Message.Contains("Fatal error encountered during command execution."))
                            {
                                ShutConnection();
                                GetConnection();
                                MyLoop1 = 0;
                            }
                            else
                            {
                                ShutConnection();
                                GetConnection();
                                MyLoop1 = 0;
                            }
                        }
                    }
                }
            }
            #endregion

            int MyLoop = 0;
            while (MyLoop == 0)
            {
                GetConnection();
                MyLoop = 1;
                string curr_datetime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string inquery = "INSERT INTO l2l_tenders_tbl ( `notice_no`, `file_id`, `purchaser_name`, `deadline`, `country`, `description`, `purchaser_address`, `purchaser_email`, `purchaser_url`, `financier`, `deadline_two`," +
                "`tender_details`, `ncbicb`, `status`, `added_on`, `search_id`, `cpv_value`, `cpv_userid`, `quality_status`, `quality_id`, `quality_addeddate`, `source`, `tender_doc_file`, `Col1`, `Col2`, `Col3`, `Col4`, `Col5`," +
                "`file_name`, `user_id`, `status_download_id`, `save_status`, `selector_id`, `select_date`, `datatype`, `compulsary_qc`, `notice_type`,`cqc_status`,`DocCost`,`DocLastDate`,`purchaser_emd`,`purchaser_value`, `is_english`, `currency`,`sector`,`project_location`,`set_aside`,`file_name_additional`, `other_details`, `file_upload`) " +
                "VALUES ( @notice_no, @file_id, @purchaser_name, @deadline, @country, @description, @purchaser_address, @purchaser_email, @purchaser_url, @financier, @deadline_two, @tender_details, @ncbicb, @status, @added_on, @search_id, @cpv_value, @cpv_userid, @quality_status, @quality_id, @quality_addeddate," +
                "@source, @tender_doc_file, @col1, @col2, @col3, @col4, @col5, @file_name, @user_id, @status_download_id, @save_status, @selector_id, @select_date, @datatype, @compulsary_qc, @notice_type, @cqc_status, @DocCost, @DocLastDate, @purchaser_emd, @purchaser_value, @is_english, @currency, @sector, @project_location, @set_aside, @file_name_additional, @other_details, @file_upload)";

                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Parameters.AddWithValue("@notice_no", SegFields[13].ToString());
                    cmd.Parameters.AddWithValue("@file_id", HtmlDaoc_fileid.ToString());
                    cmd.Parameters.AddWithValue("@purchaser_name", SegFields[12].ToString());
                    cmd.Parameters.AddWithValue("@deadline", SegFields[24].ToString());
                    cmd.Parameters.AddWithValue("@country", SegFields[7].ToString());
                    cmd.Parameters.AddWithValue("@description", SegFields[19].ToString());
                    cmd.Parameters.AddWithValue("@purchaser_address", SegFields[2].ToString());
                    cmd.Parameters.AddWithValue("@purchaser_email", SegFields[1].ToString());
                    cmd.Parameters.AddWithValue("@purchaser_url", SegFields[8].ToString());
                    cmd.Parameters.AddWithValue("@financier", SegFields[27].ToString());
                    cmd.Parameters.AddWithValue("@deadline_two", SegFields[24].ToString());
                    cmd.Parameters.AddWithValue("@tender_details", SegFields[18].ToString());
                    cmd.Parameters.AddWithValue("@ncbicb", ncb_icb.ToString());
                    cmd.Parameters.AddWithValue("@status", dms_entrynotice_tblstatus);
                    cmd.Parameters.AddWithValue("@added_on", curr_datetime.ToString());
                    cmd.Parameters.AddWithValue("@search_id", search_id);
                    cmd.Parameters.AddWithValue("@cpv_value", SegFields[36].ToString());
                    cmd.Parameters.AddWithValue("@cpv_userid", cpv_user_id);
                    cmd.Parameters.AddWithValue("@quality_status", dms_entrynotice_tblquality_status);
                    cmd.Parameters.AddWithValue("@quality_id", quality_id);
                    cmd.Parameters.AddWithValue("@quality_addeddate", curr_datetime.ToString());
                    cmd.Parameters.AddWithValue("@source", source.ToString());
                    cmd.Parameters.AddWithValue("@tender_doc_file", SegFields[28].ToString());
                    cmd.Parameters.AddWithValue("@col1", col1.ToString());
                    cmd.Parameters.AddWithValue("@col2", col2);
                    cmd.Parameters.AddWithValue("@col3", col3);
                    cmd.Parameters.AddWithValue("@col4", col4);
                    cmd.Parameters.AddWithValue("@col5", col5);//orgaddress+ "~" + orgemail + "~" + orgtelfax + "~" + docstartdata + "~" + opendate for indiaproduct client
                    cmd.Parameters.AddWithValue("@file_name", "D:\\Tide\\DocData\\" + HtmlDaoc_fileid + ".html".ToString());
                    cmd.Parameters.AddWithValue("@user_id", dms_downloadfiles_tbluser_id);
                    cmd.Parameters.AddWithValue("@status_download_id", dms_downloadfiles_tblstatus);
                    cmd.Parameters.AddWithValue("@save_status", dms_downloadfiles_tblsave_status);
                    cmd.Parameters.AddWithValue("@selector_id", selector_id);
                    cmd.Parameters.AddWithValue("@select_date", curr_datetime.ToString());
                    cmd.Parameters.AddWithValue("@datatype", dms_downloadfiles_tbldatatype);
                    cmd.Parameters.AddWithValue("@compulsary_qc", dms_entrynotice_tblcompulsary_qc);
                    cmd.Parameters.AddWithValue("@notice_type", dms_entrynotice_tblnotice_type);
                    cmd.Parameters.AddWithValue("@cqc_status", dms_entrynotice_tbl_cqc_status);
                    cmd.Parameters.AddWithValue("@DocCost", SegFields[22].ToString());
                    cmd.Parameters.AddWithValue("@DocLastDate", SegFields[41].ToString());
                    cmd.Parameters.AddWithValue("@purchaser_emd", SegFields[26].ToString());
                    cmd.Parameters.AddWithValue("@purchaser_value", SegFields[20].ToString());
                    cmd.Parameters.AddWithValue("@is_english", is_english);
                    cmd.Parameters.AddWithValue("@currency", SegFields[21].ToString());
                    cmd.Parameters.AddWithValue("@sector", SegFields[29].ToString());
                    cmd.Parameters.AddWithValue("@project_location", SegFields[42].ToString());
                    cmd.Parameters.AddWithValue("@set_aside", SegFields[43].ToString());
                    cmd.Parameters.AddWithValue("@file_name_additional", SegFields[44].ToString());
                    cmd.Parameters.AddWithValue("@other_details", SegFields[46].ToString());
                    cmd.Parameters.AddWithValue("@file_upload", file_upload);
                    try
                    {
                        cmd.CommandTimeout = 300;
                        cmd.CommandText = inquery;
                        cmd.Connection = myConnection;
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        CreateErrorLog(ex.Message.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), Application.ProductName.ToString(), SegFields[28].ToString());
                        if (ex.Message.Contains("Duplicate entry"))
                        {
                            MyLoop = 1;
                        }
                        else if (ex.Message.Contains("Fatal error encountered during command execution."))
                        {
                            ShutConnection();
                            MyLoop = 0;
                        }
                        else
                        {
                            ShutConnection();
                            MyLoop = 0;
                        }
                    }
                }
            }
            return MyReturnValue;
        }
        public void CreateErrorLog(string msg, string method, string source, string docpath)
        {
            int b = 0;
            while (b == 0)
            {
                if (errorcount > 100)
                {
                    MessageBox.Show("Error occurred more than 200 times.\n\nERROR MSG : " + msg, source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                try
                {
                    GetConnection();
                    string CommandText = "INSERT INTO errorlog_tbl (Error_Message,Function_Name,Exe_Name,doc_path) VALUES (@Error_Message,@Function_Name,@Exe_Name,@doc_path)";
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = CommandText;
                        cmd.Connection = myConnection;
                        cmd.Parameters.AddWithValue("@Error_Message", msg.ToString());
                        cmd.Parameters.AddWithValue("@Function_Name", method.ToString());
                        cmd.Parameters.AddWithValue("@Exe_Name", source.ToString());
                        cmd.Parameters.AddWithValue("@doc_path", docpath.ToString());
                        cmd.ExecuteNonQuery();
                        b++;
                        errorcount++;
                    }
                }
                catch (Exception ex)
                {
                    ShutConnection();
                    b = 0;
                }
            }
        }
        private MySqlDataReader MyReader(List<string> SegFields)
        {
            int a = 0;
            while (a == 0)
            {
                GetConnection();
                try
                {
                    string CommandText = "";
                    if (SegFields[13].ToString() != "" && SegFields[24].ToString() != "")//notice_no and doclast and country
                    {
                        CommandText = "SELECT Posting_Id FROM " + local_table_name + " WHERE Tender_notice_no = @noticeno and doc_last = @doclast and country = @country";
                    }
                    else if (SegFields[13].ToString() != "" && SegFields[07].ToString() != "")//notice_no and country
                    {
                        CommandText = "SELECT Posting_Id FROM " + local_table_name + " WHERE Tender_notice_no = @noticeno and country = @country";
                    }
                    else if (SegFields[19].ToString() != "" && SegFields[24].ToString() != "")//shortdesc and doclast and country
                    {
                        CommandText = "SELECT Posting_Id FROM " + local_table_name + " WHERE short_desc = @shortdesc and doc_last = @doclast and country = @country";
                    }
                    else
                    {
                        CommandText = "SELECT Posting_Id FROM " + local_table_name + " WHERE short_desc = @shortdesc and country = @country";
                    }
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = CommandText;
                        cmd.Connection = myConnection;
                        cmd.Parameters.AddWithValue("@noticeno", SegFields[13].ToString());
                        cmd.Parameters.AddWithValue("@doclast", SegFields[24].ToString());
                        cmd.Parameters.AddWithValue("@country", SegFields[07].ToString());
                        cmd.Parameters.AddWithValue("@shortdesc", SegFields[19].ToString());
                        myDataReader = cmd.ExecuteReader();
                        a = 1;
                    }
                }
                catch (Exception ex)
                {
                    a = 0;
                    ShutDataReader();
                    ShutConnection();
                    CreateErrorLog(ex.Message.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), Application.ProductName.ToString(), SegFields[28].ToString());
                }
            }
            return myDataReader;
        }
        public void GetConnection()
        {
            int a = 0;
            while (a == 0)
            {
                try
                {
                    myConnection = new MySqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                    myConnection.Open();
                    a++;
                }
                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(10000);
                    a = 0;
                    ShutConnection();
                }
            }
        }
        public void ShutConnection()
        {
            try
            {
                MySqlConnection.ClearAllPools();
                myConnection.Dispose();
                myConnection.Close();
            }
            catch
            {

            }
        }
        private void ShutDataReader()
        {
            try
            {
                myDataReader.Dispose();
                myDataReader.Close();
            }
            catch (Exception ex)
            {

            }
        }       
        public string Upload_HtmlDoc_file_AWS(string HtmlDoc)
        {
            System.Threading.Thread.Sleep(1000);
            string id = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            Int64 posting_id = Int64.Parse(id.ToString());
            posting_id++;
            string file_id = exe_no + posting_id + Guid.NewGuid().ToString("N").Substring(0, 8); // Add Extra 8 alphanumeric value using (Global Unique Identifier)
            int a = 0;
            while (a == 0)
            {
                try
                {
                    string path = "Docs";
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    string strOpFileName = path + "\\" + file_id.ToString() + ".html";
                    System.IO.StreamWriter swOut = new System.IO.StreamWriter(strOpFileName, false, Encoding.UTF8);
                    swOut.Write(HtmlDoc);
                    swOut.Close();

                    FileUpload_AWS(strOpFileName, "");
                    clearFolder(path);
                    a++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in '" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "' Method.\nError is: " + ex.Message, Application.ProductName.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return file_id;
        }
        public void clearFolder(string FolderName)
        {
            try
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(FolderName);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in '" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "' Method.\nError is: " + ex.Message, Application.ProductName.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public List<string> Fields_Validation(List<string> SegFields)
        {
            if (SegFields[19].Length > 200)
            {
                SegFields[19] = SegFields[19].Remove(200).ToString().Trim() + "...";
            }
            if (SegFields[02].Length > 500)
            {
                SegFields[02] = SegFields[02].Remove(500).ToString().Trim() + "...";
            }
            for (int i = 0; i < SegFields.Count; i++)
            {
                SegFields[i] = System.Net.WebUtility.HtmlDecode(SegFields[i]);
                //SegFields[i] = SegFields[i].Replace("'", "''");
                SegFields[i] = SegFields[i].Trim();
            }
            for (int i = 0; i < SegFields.Count; i++)
            {
                if (SegFields[i].Length > 2000)
                {
                    SegFields[i] = SegFields[i].Substring(0, 2000);
                    SegFields[i] = SegFields[i] + "...";
                }
                if (SegFields[i].ToString() == "")
                { SegFields[i] = ""; }
            }
            return SegFields;
        }
        public string make_title_case(string title)
        {
            title = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());
            title = title.Replace(" I ", " i ");
            title = title.Replace(" Me ", " me ");
            title = title.Replace(" My ", " my ");
            title = title.Replace(" Myself ", " myself ");
            title = title.Replace(" We ", " we ");
            title = title.Replace(" Our ", " our ");
            title = title.Replace(" Ours ", " ours ");
            title = title.Replace(" Ourselves ", " ourselves ");
            title = title.Replace(" You ", " you ");
            title = title.Replace(" Your ", " your ");
            title = title.Replace(" Yours ", " yours ");
            title = title.Replace(" Yourself ", " yourself ");
            title = title.Replace(" Yourselves ", " yourselves ");
            title = title.Replace(" He ", " he ");
            title = title.Replace(" Him ", " him ");
            title = title.Replace(" His ", " his ");
            title = title.Replace(" Himself ", " himself ");
            title = title.Replace(" She ", " she ");
            title = title.Replace(" Her ", " her ");
            title = title.Replace(" Hers ", " hers ");
            title = title.Replace(" Herself ", " herself ");
            title = title.Replace(" It ", " it ");
            title = title.Replace(" Its ", " its ");
            title = title.Replace(" Itself ", " itself ");
            title = title.Replace(" They ", " they ");
            title = title.Replace(" Them ", " them ");
            title = title.Replace(" Their ", " their ");
            title = title.Replace(" Theirs ", " theirs ");
            title = title.Replace(" Themselves ", " themselves ");
            title = title.Replace(" What ", " what ");
            title = title.Replace(" Which ", " which ");
            title = title.Replace(" Who ", " who ");
            title = title.Replace(" Whom ", " whom ");
            title = title.Replace(" This ", " this ");
            title = title.Replace(" That ", " that ");
            title = title.Replace(" These ", " these ");
            title = title.Replace(" Those ", " those ");
            title = title.Replace(" Am ", " am ");
            title = title.Replace(" Is ", " is ");
            title = title.Replace(" Are ", " are ");
            title = title.Replace(" Was ", " was ");
            title = title.Replace(" Were ", " were ");
            title = title.Replace(" Be ", " be ");
            title = title.Replace(" Been ", " been ");
            title = title.Replace(" Being ", " being ");
            title = title.Replace(" Have ", " have ");
            title = title.Replace(" Has ", " has ");
            title = title.Replace(" Had ", " had ");
            title = title.Replace(" Having ", " having ");
            title = title.Replace(" Do ", " do ");
            title = title.Replace(" Does ", " does ");
            title = title.Replace(" Did ", " did ");
            title = title.Replace(" Doing ", " doing ");
            title = title.Replace(" A ", " a ");
            title = title.Replace(" An ", " an ");
            title = title.Replace(" The ", " the ");
            title = title.Replace(" And ", " and ");
            title = title.Replace(" But ", " but ");
            title = title.Replace(" If ", " if ");
            title = title.Replace(" Or ", " or ");
            title = title.Replace(" Because ", " because ");
            title = title.Replace(" As ", " as ");
            title = title.Replace(" Until ", " until ");
            title = title.Replace(" While ", " while ");
            title = title.Replace(" Of ", " of ");
            title = title.Replace(" At ", " at ");
            title = title.Replace(" By ", " by ");
            title = title.Replace(" For ", " for ");
            title = title.Replace(" With ", " with ");
            title = title.Replace(" About ", " about ");
            title = title.Replace(" Against ", " against ");
            title = title.Replace(" Between ", " between ");
            title = title.Replace(" Into ", " into ");
            title = title.Replace(" Through ", " through ");
            title = title.Replace(" During ", " during ");
            title = title.Replace(" Before ", " before ");
            title = title.Replace(" After ", " after ");
            title = title.Replace(" Above ", " above ");
            title = title.Replace(" Below ", " below ");
            title = title.Replace(" To ", " to ");
            title = title.Replace(" From ", " from ");
            title = title.Replace(" Up ", " up ");
            title = title.Replace(" Down ", " down ");
            title = title.Replace(" In ", " in ");
            title = title.Replace(" Out ", " out ");
            title = title.Replace(" On ", " on ");
            title = title.Replace(" Off ", " off ");
            title = title.Replace(" Over ", " over ");
            title = title.Replace(" Under ", " under ");
            title = title.Replace(" Again ", " again ");
            title = title.Replace(" Further ", " further ");
            title = title.Replace(" Then ", " then ");
            title = title.Replace(" Once ", " once ");
            title = title.Replace(" Here ", " here ");
            title = title.Replace(" There ", " there ");
            title = title.Replace(" When ", " when ");
            title = title.Replace(" Where ", " where ");
            title = title.Replace(" Why ", " why ");
            title = title.Replace(" How ", " how ");
            title = title.Replace(" All ", " all ");
            title = title.Replace(" Any ", " any ");
            title = title.Replace(" Both ", " both ");
            title = title.Replace(" Each ", " each ");
            title = title.Replace(" Few ", " few ");
            title = title.Replace(" More ", " more ");
            title = title.Replace(" Most ", " most ");
            title = title.Replace(" Other ", " other ");
            title = title.Replace(" Some ", " some ");
            title = title.Replace(" Such ", " such ");
            title = title.Replace(" No ", " no ");
            title = title.Replace(" Nor ", " nor ");
            title = title.Replace(" Not ", " not ");
            title = title.Replace(" Only ", " only ");
            title = title.Replace(" Own ", " own ");
            title = title.Replace(" Same ", " same ");
            title = title.Replace(" So ", " so ");
            title = title.Replace(" Than ", " than ");
            title = title.Replace(" Too ", " too ");
            title = title.Replace(" Very ", " very ");
            title = title.Replace(" S ", " s ");
            title = title.Replace(" T ", " t ");
            title = title.Replace(" Can ", " can ");
            title = title.Replace(" Will ", " will ");
            title = title.Replace(" Just ", " just ");
            title = title.Replace(" Don ", " don ");
            title = title.Replace(" Should ", " should ");
            title = title.Replace(" Now", " now");
            return title;
        }      
        private void CombineParasInRequiredFields()
        {
            string MyNewLine = Environment.NewLine;
            int MyNewLineIn = 0;
            for (int i = 0; i < RequiredFields.Count; i++)
            {
                int PrevJ = -1;
                for (int j = (i * 10); j < ((i * 10) + 10); j++)
                {
                    if ((RequiredPages[j].Trim()) != null && (RequiredPages[j].Trim()) != "")
                    {
                        if (PrevJ == -1) // This field has freshly started
                        {
                            RequiredFields[i] = RequiredPages[j];
                            PrevJ = j;
                        }
                        else
                        {
                            if (PrevJ == j)
                            {
                                RequiredFields[i] = RequiredFields[i] + MySingleSpace + RequiredPages[j];
                                MyNewLineIn = 1;
                            }
                            else
                            {
                                RequiredFields[i] = RequiredFields[i] + "\n<BR>" + RequiredPages[j];
                                PrevJ = j;
                            }
                        }
                    }
                }
                if (RequiredFields[i] != null && RequiredFields[i] != "")
                {
                    if (MyNewLineIn != 0)
                    {
                        RequiredFields[i] = RequiredFields[i] + MyNewLine + "\n<BR>";
                        MyNewLineIn = 0;
                    }
                }
            }

        }
        public string GetRqdStr(string TheString, string OnLeft, string OnRight0, string OnRight1, string OnRight2)
        {

            if (TheString != null && TheString.Trim() != null)
            {
                MyBegin = TheString.IndexOf(OnLeft, 0);
                if (MyBegin == -1)
                {
                    ReplyStrings = "";
                }
                else
                {
                    if (OnRight0 == null || OnRight0 == "")
                    {
                        MyCurr = -1;
                    }
                    else
                    {
                        MyCurr = TheString.IndexOf(OnRight0, (MyBegin + OnLeft.Length));
                    }
                    if (OnRight1 == null || OnRight1 == "")
                    {
                        MyCurr1 = -1;
                    }
                    else
                    {
                        MyCurr1 = TheString.IndexOf(OnRight1, (MyBegin + OnLeft.Length));
                    }
                    if (OnRight2 == null || OnRight2 == "")
                    {
                        MyCurr2 = -1;
                    }
                    else
                    {
                        MyCurr2 = TheString.IndexOf(OnRight2, (MyBegin + OnLeft.Length));
                    }
                    if (MyCurr == -1 && MyCurr1 == -1 && MyCurr2 == -1)
                    {
                        MyEnd = TheString.Length;
                    }
                    else
                    {
                        if (MyCurr == -1)
                        {
                            if (MyCurr1 == -1)
                            {
                                MyEnd = MyCurr2;
                                OnRight = OnRight2;
                            }
                            else
                            {
                                if (MyCurr2 == -1)
                                {
                                    MyEnd = MyCurr1;
                                    OnRight = OnRight1;
                                }
                                else
                                {
                                    if (MyCurr1 < MyCurr2)
                                    {
                                        MyEnd = MyCurr1;
                                        OnRight = OnRight1;
                                    }
                                    else
                                    {
                                        MyEnd = MyCurr2;
                                        OnRight = OnRight2;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (MyCurr1 == -1)
                            {
                                if (MyCurr2 == -1)
                                {
                                    MyEnd = MyCurr;
                                    OnRight = OnRight0;
                                }
                                else
                                {
                                    if (MyCurr < MyCurr2)
                                    {
                                        MyEnd = MyCurr;
                                        OnRight = OnRight0;
                                    }
                                    else
                                    {
                                        MyEnd = MyCurr2;
                                        OnRight = OnRight2;
                                    }
                                }
                            }
                            else
                            {
                                if (MyCurr2 == -1)
                                {
                                    if (MyCurr < MyCurr1)
                                    {
                                        MyEnd = MyCurr;
                                        OnRight = OnRight0;
                                    }
                                    else
                                    {
                                        MyEnd = MyCurr1;
                                        OnRight = OnRight1;
                                    }
                                }
                                else
                                {
                                    if (MyCurr < MyCurr1)
                                    {
                                        if (MyCurr < MyCurr2)
                                        {
                                            MyEnd = MyCurr;
                                            OnRight = OnRight0;
                                        }
                                        else
                                        {
                                            MyEnd = MyCurr2;
                                            OnRight = OnRight2;
                                        }
                                    }
                                    else
                                    {
                                        if (MyCurr2 < MyCurr1)
                                        {
                                            MyEnd = MyCurr2;
                                            OnRight = OnRight2;
                                        }
                                        else
                                        {
                                            MyEnd = MyCurr1;
                                            OnRight = OnRight1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    MyWorkBuff = TheString.Substring((MyBegin + OnLeft.Length),
                                                 (MyEnd - MyBegin - OnLeft.Length));
                    ReplyStrings = MyWorkBuff.Trim();
                    //ReplyStrings = MyWorkBuff.Replace(Environment.NewLine, "");                   
                }
            }
            else
            {
                ReplyStrings = "";
            }
            return ReplyStrings.Trim();
        }

        public string ReplaceOthers(string rString)
        {
            rString = Regex.Replace(rString, "\n", "", RegexOptions.IgnoreCase);
            rString = Regex.Replace(rString, "\r", "", RegexOptions.IgnoreCase);
            rString = Regex.Replace(rString, "\t", "", RegexOptions.IgnoreCase);
            rString = rString.Trim();
            return rString;
        }
    }
}
