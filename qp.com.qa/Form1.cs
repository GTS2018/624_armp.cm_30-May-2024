using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.Globalization;
using System.Text.RegularExpressions;
using GlobalLevel;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Net.Mime;
using System.Net.Http;

namespace qp.com.qa
{
    public partial class Form1 : Form
    {
        //CancelEventHandler webBrowser1NewWindow = null;
        ScrapClass.Class1 MainClass = new ScrapClass.Class1();
        HtmlDocument theDoc = null;
        string Step = "Step_1"; string MyUrl = ""; string lastpage = string.Empty; string maindata = string.Empty;
        int MyReturnCode = 0; int pno = 2;
        int a = 0; int totaldata = 0; int timer = 0; int totalten = 0;
        bool nxtpage = true; int navilink = 0; int t = 0;

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            System.Diagnostics.Process[] procss = System.Diagnostics.Process.GetProcessesByName(Application.ProductName);
            if (procss.Count() > 1)
            {
                MessageBox.Show("Same Product Name Exe is Already Running....!\n\n\n\n\n", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Application.Exit();
                Environment.Exit(0);
            }
            string dayname = DateTime.Now.DayOfWeek.ToString();
            if (dayname == "Monday")
            {
                dateTimePicker1From.Text = DateTime.Today.AddDays(-2).ToString();
            }
            else
            {
                dateTimePicker1From.Text = DateTime.Today.AddDays(-1).ToString();
            }
            dateTimePicker2To.Text = DateTime.Today.AddDays(0).ToString();
            radioButton1live.Checked = true;
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.StatusTextChanged += new EventHandler(webBrowser1_StatusTextChanged);
        }
        private void button1exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void radioButton1live_CheckedChanged(object sender, EventArgs e)
        {
            GlobalLevel.Global.flagServer = true;
            GlobalLevel.Global.server = "Live";
            this.Text = "armp.cm [Live Server Selected]";
            label5server.Text = "Live Server Selected";
            label5server.Refresh();
        }
        private void radioButton2test_CheckedChanged(object sender, EventArgs e)
        {
            GlobalLevel.Global.flagServer = false;
            GlobalLevel.Global.server = "Test";
            this.Text = "armp.cm [Test Server Selected]";
            label5server.Text = "Test Server Selected";
            label5server.Refresh();
        }
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.ReadyState == WebBrowserReadyState.Complete)
            {
                theDoc = webBrowser1.Document;
                string MyDocType = webBrowser1.DocumentType.ToString();
                if (theDoc.Body.InnerText.Contains("Navigation to the webpage was canceled") || theDoc.Body.InnerText.Contains("This program cannot display the webpage"))
                {
                    //MessageBox.Show(new Form() { TopMost = true }, "Network Connection Failed.\r\n    OR\r\nNavigation to the webpage was canceled!!", "Network Problem!!", MessageBoxButtons.OK, MessageBoxIcon.None);
                    //webBrowser1.Refresh();
                }
                else
                {
                    switch (Step)
                    {
                        case "Step_1":
                            CollectLinks();
                            NextPage();
                            break;

                        case "Step_2":
                            GetData();
                            NavigateLinks();
                            break;

                        case "Step_3":

                            break;
                    }
                }
            }
            else
            {
                Application.DoEvents();
            }
        }
        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {

        }
        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            txtUrlName.Text = e.Url.ToString();
            MyUrl = txtUrlName.Text;
        }
        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }
        private void webBrowser1_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            try
            {
                toolStripProgressBar1.Maximum = (int)e.MaximumProgress;
                toolStripProgressBar1.Value = (int)e.CurrentProgress;
            }
            catch (Exception ex)
            { }
        }
        private void webBrowser1_StatusTextChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = webBrowser1.StatusText;
        }
        private void statusStrip1_TextChanged(object sender, EventArgs e)
        {

        }
        public void NextPage()
        {
            try
            {
                if(Global.nextpage == true)
                {
                    if (theDoc.Body.InnerHtml.Contains(">" + pno + "</A>"))
                    {
                        label1pno.Visible = true;
                        label1pno.Text = "NextPage : " + pno;
                        label1pno.Refresh();

                        HtmlElementCollection tdcall = theDoc.GetElementsByTagName("a");
                        foreach (HtmlElement tag in tdcall)
                        {
                            if (tag.OuterHtml.Contains(">" + pno + "</A>"))
                            {
                                tag.InvokeMember("click");
                                Application.DoEvents();
                                pno++;
                                Step = "Step_1";
                                break;
                            }
                        }
                    }
                    else
                    {
                        NavigateLinks();
                    }
                }
                else
                {
                    NavigateLinks();
                }
            }
            catch
            {
                MessageBox.Show("Error in NextPage() Method..\n\nPlease Give it for Maintenance!!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void NavigateLinks()
        {
            if (a < GlobalLevel.Global.Doc_Links.Count)
            {
                string link = GlobalLevel.Global.Doc_Links[a];
                link = link.Substring(link.IndexOf("☺") + "☺".Length);
                webBrowser1.Navigate(link);
                Step = "Step_2";
            }
            else
            {
                MessageBox.Show("All Data Inserted Successfully....!!\n\nTotal Data : " + GlobalLevel.Global.Doc_Links.Count + "\n\nTotal Inserted : " + GlobalLevel.Global.inserted + "\n\nDuplicate Data : " + GlobalLevel.Global.duplicate + "\n\nExpired Data : " + GlobalLevel.Global.expired + "\n\nSkipped Data : " + GlobalLevel.Global.skipped + "\n\nQC Data : " + GlobalLevel.Global.qccount, "Completed - " + Application.ProductName.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
                Environment.Exit(0);
            }
        }
        private void GetData()
        {
            try
            {
                string data1 = theDoc.Body.InnerHtml;
                int checkcomeinif = 0;
                HtmlElementCollection divcall = theDoc.GetElementsByTagName("DIV");
                foreach (HtmlElement tag in divcall)
                {
                    if (tag.GetAttribute("classname") == " inner-body mx-auto px-5 ")
                    {
                        checkcomeinif = 1;
                        string data = tag.OuterHtml;

                        #region Get additional attach_docs --METHOD 1
                        string attach_link_withname = string.Empty;
                        try
                        {
                            string tdcol = data;
                            while (tdcol.Contains("href=\""))
                            {
                                string doclink = tdcol.Substring(tdcol.IndexOf("href=\"") + ("href=\"").Length);
                                doclink = doclink.Substring(0, doclink.IndexOf("\"")).Trim();
                                doclink = doclink.Replace("&amp;", "&").Trim();

                                string htmldata = GetSource(doclink);

                                if(htmldata.Contains("src=\""))
                                {
                                    string newdoclink = htmldata.Substring(htmldata.IndexOf("src=\"") + ("src=\"").Length);
                                    newdoclink = newdoclink.Substring(0, newdoclink.IndexOf("\"")).Trim();
                                    newdoclink = newdoclink.Replace("&amp;", "&").Trim();

                                    string docname = string.Empty;
                                    docname = getFilename1(newdoclink);
                                    if (docname != "")
                                    {
                                        if (attach_link_withname == "")
                                        {
                                            attach_link_withname = docname + "~" + newdoclink;
                                        }
                                        else
                                        {
                                            string fullurldocnam = docname + "~" + newdoclink;
                                            attach_link_withname += "," + fullurldocnam;
                                        }
                                    }
                                }

                                doclink = doclink.Replace("&", "&amp;").Trim();
                                doclink = "href=\"" + doclink + "";
                                tdcol = tdcol.Replace(doclink, "");
                            }
                        }
                        catch (Exception exx)
                        {
                            MessageBox.Show("Error while crawling attachments links..\nError in '" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "' Method..\n\nPlease Give it for Maintenance!!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        #endregion

                        GlobalLevel.Global.DocPath = GlobalLevel.Global.Doc_Links[a];
                        GlobalLevel.Global.DataHtmlDoc = data;

                        if (data != "")
                        {
                            int MyReturnCode = MainClass.SegDoc(GlobalLevel.Global.DataHtmlDoc, GlobalLevel.Global.DocPath, attach_link_withname);
                            a++;

                            label4datainserted.Visible = true;
                            label4datainserted.Text = "Inserted : " + GlobalLevel.Global.inserted + "\nDuplicate : " + GlobalLevel.Global.duplicate + "\nExpired : " + GlobalLevel.Global.expired;
                            label4datainserted.Refresh();
                            label3datacoll.Visible = true;
                           // label3datacoll.Text = "Completed : " + (a + Global.Doc_Links.Count);
                            label3datacoll.Text = "Completed : " + (a + 1) + " / " + GlobalLevel.Global.Doc_Links.Count();
                            
                            label3datacoll.Refresh();
                            this.Text = Application.ProductName.ToString() + " [" + (a + 1) + " / " + Global.Doc_Links.Count + "]";
                        }
                    }
                }
                if (checkcomeinif == 0)
                {
                    GlobalLevel.Global.skipped++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in GetData() method\n\nError are : " + ex, "armp.cm");
            }
        }
        private void CollectLinks()
        {
            try
            {
                
                HtmlElementCollection divcol = theDoc.GetElementsByTagName("Div");
                foreach (HtmlElement table in divcol)
                {
                    if (table.GetAttribute("classname") == "container-fluid armp-body  p-1")
                    {
                        HtmlElementCollection licol = table.GetElementsByTagName("li");
                        foreach (HtmlElement ele2 in licol)
                        {
                            if (ele2.InnerHtml.Contains("Details"))
                            {
                                string Title = string.Empty;
                                string Publisheddate = string.Empty;
                                string deadline = string.Empty;
                                string Amount = string.Empty;
                                string link = string.Empty;

                                maindata = ele2.InnerHtml;

                                Publisheddate = maindata.Substring(maindata.IndexOf("Published on the : </DIV>") + "Published on the : </DIV>".Length).Trim();
                                Publisheddate = Publisheddate.Remove(Publisheddate.IndexOf("</DIV>")).Trim();
                                Publisheddate = Regex.Replace(Publisheddate, @"<[^>]*>", String.Empty).Trim();
                                Publisheddate = Publisheddate.Remove(Publisheddate.IndexOf(" ")).Trim();

                                DateTime dt = DateTime.ParseExact(Publisheddate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                DateTime dt2 = DateTime.ParseExact(Global.Fromdate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                TimeSpan ts = dt - dt2;
                                int days = ts.Days;
                                if (days >= 0)
                                {
                                    Title = maindata.Substring(maindata.IndexOf(">") + ">".Length).Trim();
                                    Title = Title.Remove(Title.IndexOf("</STRONG></DIV>")).Trim();
                                    Title = Regex.Replace(Title, @"<[^>]*>", String.Empty).Trim();

                                    deadline = maindata.Substring(maindata.IndexOf("Closing date : </DIV>") + "Closing date : </DIV>".Length).Trim();
                                    deadline = deadline.Remove(deadline.IndexOf("</DIV>")).Trim();
                                    deadline = Regex.Replace(deadline, @"<[^>]*>", String.Empty).Trim();

                                    Amount = maindata.Substring(maindata.IndexOf("Amount : </DIV>") + "Amount : </DIV>".Length).Trim();
                                    Amount = Amount.Remove(Amount.IndexOf("</DIV>")).Trim();
                                    Amount = Regex.Replace(Amount, @"<[^>]*>", String.Empty).Trim();
                                    Amount = Regex.Replace(Amount, @"[^0-9.]", String.Empty).Trim();

                                    link = maindata.Substring(maindata.IndexOf("href=\"/") + "href=\"/".Length).Trim();
                                    link = link.Remove(link.IndexOf("\"")).Trim();
                                    link = Regex.Replace(link, @"<[^>]*>", String.Empty).Trim();
                                    link = "https://www.armp.cm/" + link;
                                    link = link.Replace("&amp;", "&").Trim();

                                    Global.Doc_Links.Add(Title + "♥" + Publisheddate + "♦" + deadline + "•" + Amount + "☺" + link);
                                    label2linkcoll.Visible = true;
                                    label2linkcoll.Text = "Links Collected : " + Global.Doc_Links.Count.ToString();
                                    label2linkcoll.Refresh();
                                }
                                else
                                {
                                    Global.nextpage = false;
                                    break;
                                }
                            }
                            
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("Error in CollectLinks() method...Give it for maintenance \n\n" + ee.Message, "iom.int");
                Application.Exit();
            }
        }
        private string GetSource(string url)
        {
            System.Net.WebRequest WReq = null;
            System.Net.WebResponse WRes = null;
            System.IO.StreamReader SReader = null;
            WRes = null;
            string searchData = "";
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                WReq = System.Net.WebRequest.Create(url);
                WReq.Timeout = 10 * 60 * 1000;
                WRes = WReq.GetResponse();
                SReader = new System.IO.StreamReader(WRes.GetResponseStream());
                searchData = SReader.ReadToEnd();
                WRes.Close();
                WReq = null;
                WRes = null;
            }
            catch (Exception e)
            {
                
            }
            return searchData;
        }
        private string getFilename1(string url)
        {
            Uri uri = new Uri(url);
            string filename = System.IO.Path.GetFileName(uri.LocalPath);
            filename = filenameReplace(filename);
            if (!filename.Contains("."))
            {
                filename = "";
            }
            return filename;
        }
        public string filenameReplace(string filename)
        {
            filename = Regex.Replace(filename, @"[^0-9a-zA-Z .\-_]+", String.Empty).Trim();
            filename = Regex.Replace(filename, @"\s+", " ");
            filename = filename.Replace(" ", "-");
            filename = filename.Replace("--", "-");
            return filename;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            GlobalLevel.Global.Fromdate = dateTimePicker1From.Text;
            panel1.Enabled = false;
            webBrowser1.Navigate("https://www.armp.cm/");
            Step = "Step_1";
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timer >= 50)
            {
                theDoc = webBrowser1.Document;
                timer = 0;
                timer1.Enabled = false;
                if (theDoc != null)
                {
                    if (theDoc.Body != null)
                    {
                        if (theDoc.Body.InnerHtml.Contains("<DIV class=\"tableCell resultTitle\">"))
                        {

                        }
                        else
                        {
                            timer1.Enabled = true;
                        }
                    }
                    else
                    {
                        timer1.Enabled = true;
                    }
                }
                else
                {
                    timer1.Enabled = true;
                }
            }
            else
            {
                timer++;
                label1timer.Visible = true;
                label1timer.Text = (timer * 2) + "% Loading...";
                label1timer.Refresh();
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (timer >= 50)
            {
                theDoc = webBrowser1.Document;
                timer = 0;
                timer2.Enabled = false;
                if (theDoc != null)
                {
                    if (theDoc.Body != null)
                    {
                        if (theDoc.Body.InnerHtml.Contains("<DIV style=\"DISPLAY: none\" id=ctl00_CPH1_ctl05>"))
                        {

                        }
                        else
                        {
                            timer2.Enabled = true;
                        }
                    }
                    else
                    {
                        timer2.Enabled = true;
                    }
                }
                else
                {
                    timer2.Enabled = true;
                }
            }
            else
            {
                timer++;
                label1timer.Visible = true;
                label1timer.Text = (timer * 2) + "% Loading...";
                label1timer.Refresh();
            }
        }
    }
}
