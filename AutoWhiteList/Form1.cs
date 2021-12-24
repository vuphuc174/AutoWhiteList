using AutoWhiteList.Common;
using AutoWhiteList.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tulpep.NotificationWindow;

namespace AutoWhiteList
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            TimeUpdate();


        }
        DbConnect connect = new DbConnect();
        private bool checkexist(string str)
        {
            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                if (row.Index != -1)
                {
                    if (row.Cells["ipaddress"].Value.ToString() == str)
                    {
                        return true;
                    }

                }

            }
            return false;
        }
        private void LoadData()
        {
            DataTable dtIps = connect.readdata("select * from TrackingIP order by ReleaseTime desc");
            dataGridView3.DataSource = dtIps;
        }
        async void TimeUpdate()
        {
            while (true)
            {


                if (GetIP.getIpInternet() != txtCurrentIP.Text)
                {
                    if (txtCurrentIP.Text == "")
                    {
                        txtCurrentIP.Text = GetIP.getIpInternet();
                    }
                    else
                    {
                        connect.exedata("insert into TrackingIP(IP,ReleaseTime) values('" + txtCurrentIP.Text + "','" + DateTime.Now + "')");
                        //dataGridView3.Rows.Insert(0, txtCurrentIP.Text, DateTime.Now);
                        LoadData();
                        txtCurrentIP.Text = GetIP.getIpInternet();
                        if (checkexist(txtCurrentIP.Text))
                        {
                            txtCurrentIP.BackColor = Color.Red;
                            txtCurrentIP.ForeColor = Color.White;
                            PopupNotifier popup = new PopupNotifier();
                            popup.TitleText = "Caution";
                            popup.ContentText = "IP already used";
                            popup.Popup();
                        }
                        else
                        {
                            txtCurrentIP.BackColor = Color.White;
                            txtCurrentIP.ForeColor = Color.Black;
                        }
                    }

                }
                await Task.Delay(1000);

            }
        }
        private void btnchangeip_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(changeIP));

            thread.IsBackground = true;

            thread.Start();
        }

        private void changeIP()
        {
            //ChromeDriver chromeDriver = new ChromeDriver();
            //chromeDriver.Url = "http://192.168.1.1/html/home.html";
            //try
            //{
            //    chromeDriver.Manage().Window.Maximize();
            //    chromeDriver.Navigate();

            //    //wait for all element must be loaded
            //    chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            //    //button setting
            //    IWebElement sett = chromeDriver.FindElement(By.XPath("//div[@id=\"menu_connection_settings\"]/a"));
            //    sett.Click();
            //    //username input #username
            //    IWebElement username = chromeDriver.FindElement(By.XPath("//input[@id=\"username\"]"));

            //    //password input #password
            //    IWebElement password = chromeDriver.FindElement(By.XPath("//input[@id=\"password\"]"));

            //    //button login 
            //    IWebElement login = chromeDriver.FindElement(By.XPath("//span[@class=\"button_center\"]/a"));

            //    username.SendKeys("admin");
            //    password.SendKeys("admin");
            //    login.Click();
            //    Task.Delay(2000).Wait();
            //    chromeDriver.Navigate().GoToUrl("http://192.168.1.1/html/reboot.html");
            //    //reboot button
            //    Task.Delay(2000).Wait();
            //    chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            //    IWebElement reboot = chromeDriver.FindElement(By.XPath("/html/body/div[1]/div/div[1]/div[2]/div[2]/div[2]/div/label/span/span/span/span"));
            //    reboot.Click();
            //    IWebElement confirm = chromeDriver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[3]/div/span[1]/span/span/span/a"));
            //    confirm.Click();
            //    Task.Delay(35000).Wait();
            //    chromeDriver.Quit();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            disconnectDcom();
            Task.Delay(1000).Wait();
            connectDcom();
        }
        private void disconnectDcom()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C rasdial /disconnect";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

        }
        private void connectDcom()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C rasdial viettel";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }
        private void Follow_Twitter(string url, ChromeDriver chromeDriver)
        {
            //firststep:
            chromeDriver.Navigate().GoToUrl(url);
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            //try
            //{
            IWebElement fl = chromeDriver.FindElement(By.CssSelector(".css-1dbjc4n[data-testid=\"placementTracking\"]"));
            fl.Click();
            //}
            //catch(Exception ex)
            //{

            //    changeIP();
            //    goto firststep;
            //}

        }
        string profile;
        private string get_Twitter_Account(ChromeDriver chromeDriver)
        {

            chromeDriver.SwitchTo().Window(chromeDriver.WindowHandles.Last());
            //firststep:
            chromeDriver.Navigate().GoToUrl("https://twitter.com/home");
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            //try
            //{
            IWebElement _user = chromeDriver.FindElement(By.CssSelector(".r-eqz5dr[data-testid=\"AppTabBar_Profile_Link\"]"));
            profile = _user.GetAttribute("href");

            //}
            //catch
            //{
            //    changeIP();
            //    goto firststep;
            //}
            return "@" + profile.Substring(profile.LastIndexOf("/") + 1, profile.Length - profile.LastIndexOf("/") - 1);
        }

        private string get_Twitter_Profile(ChromeDriver chromeDriver)
        {

            chromeDriver.SwitchTo().Window(chromeDriver.WindowHandles.Last());
            chromeDriver.Navigate().GoToUrl("https://twitter.com/home");
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            IWebElement _user = chromeDriver.FindElement(By.CssSelector(".r-eqz5dr[data-testid=\"AppTabBar_Profile_Link\"]"));
            return _user.GetAttribute("href");


        }
        private void Twitter_LR(ChromeDriver chromeDriver, string url)
        {
            //firststep:
            chromeDriver.Navigate().GoToUrl(url);
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            //try
            //{
            IWebElement likeBtn = chromeDriver.FindElement(By.CssSelector(".r-bt1l66[data-testid =\"like\"]"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)chromeDriver;
            js.ExecuteScript("window.scrollBy(0,250)", "");
            IWebElement commentbtn = chromeDriver.FindElement(By.CssSelector(".r-bt1l66[data-testid =\"reply\"]"));

            likeBtn.Click();
            Task.Delay(1000).Wait();
            IWebElement retweetBtn = chromeDriver.FindElement(By.CssSelector(".r-bt1l66[data-testid =\"retweet\"]"));
            retweetBtn.Click();
            Task.Delay(1000).Wait();
            IWebElement retweetConfirm = chromeDriver.FindElement(By.CssSelector(".r-1ny4l3l[data-testid =\"retweetConfirm\"]"));
            retweetConfirm.Click();
            //}
            //catch
            //{
            //    changeIP();
            //    goto firststep;
            //}


        }
        string[] comments = new string[] {
        "Hope be lucky one ",
"When you list to kucoin and other exchange ",
"awesome concept on using an existing eco system to provide revenue for the treasury ",
"Is this real life? ",
"Wow Cool project ",
"Wow this is insane! ",
"Great Project",
"Best project ever! ",
"This will be very useful in the future ",
"It's a great pleasure to be part of this wonderful opportunity ",
"I love this project so much ",
"Great to be here so early. ",
"Nice project and congratulations to the team ",
"Wonderful project ",
"This is a Fantastic project with great potential ",
"Very good projects, i think in the near future i will see an unprecedentet growth of this project ",
"This project is looks so innovative and impactful ",
"Good project and strong team ",
"I think good project. I hope have good luck in project ",
"The project is implemented professionally and has a clear development plan.",
"this is a amizing and Excellent project,best project in the airdrop history. ",
"The project is implemented very professionally and has a clear development plan. ",
"Your project is very good",
 "I hope your project can be successful and successful in the future ",
"Hopefully this project is a success and then it becomes one of the best cryptos",
"Very good projects, i think in the near future i will see an unprecedentet growth of this project.",
"Excellent project, i hope it will be one of the best project in the airdrop history.",
            " I am really happy that i have participated to this project",




        };
        private static Random random = new Random((int)DateTime.Now.Ticks);
        private string getTw()
        {
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\admin\source\repos\AutoWhiteList\AutoWhiteList\Files\tw_user.txt");
            string p1 = lines[new Random().Next(0, lines.Length)];
            Task.Delay(100).Wait();
            string p2 = lines[new Random().Next(0, lines.Length)];
            Task.Delay(100).Wait();
            string p3 = lines[new Random().Next(0, lines.Length)];

            string fri = " @" + p1 + " @" + p2 + " @" + p3;
            return comments[random.Next(0, comments.Length)] + " " + fri;
        }
        private void Twitter_Comment(ChromeDriver chromeDriver, string url, string tag)
        {

            //string path = Path.Combine(Environment.CurrentDirectory, @"Files\", "tw_user.txt");
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\admin\source\repos\AutoWhiteList\AutoWhiteList\Files\tw_user.txt");
            string p1 = lines[new Random().Next(0, lines.Length)];
            Task.Delay(100).Wait();
            string p2 = lines[new Random().Next(0, lines.Length)];
            Task.Delay(100).Wait();
            string p3 = lines[new Random().Next(0, lines.Length)];

            string fri = " @" + p1 + " @" + p2 + " @" + p3;
            chromeDriver.Navigate().GoToUrl(url);
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            //firststep:



            //try
            //{
            Task.Delay(1000).Wait();
            IWebElement commentbtn = chromeDriver.FindElement(By.CssSelector(".r-bt1l66[data-testid =\"reply\"]"));
            Task.Delay(1000).Wait();
            IJavaScriptExecutor js = (IJavaScriptExecutor)chromeDriver;
            js.ExecuteScript("window.scrollBy(0,280)", "");

            commentbtn.Click();
            Task.Delay(1000).Wait();

            chromeDriver.FindElement(By.CssSelector("div[role=\"textbox\"][data-testid=\"tweetTextarea_0\"]")).SendKeys(comments[random.Next(0, comments.Length)] + fri + " " + tag + "  ");
            Task.Delay(1000).Wait();
            IWebElement commentConfirm = chromeDriver.FindElement(By.CssSelector("div[role=\"button\"][data-testid=\"tweetButton\"]"));
            commentConfirm.Click();

            //quote tw
            //chromeDriver.Navigate().Refresh();

            Task.Delay(1000).Wait();
            try
            {
                IWebElement retweetBtn = chromeDriver.FindElement(By.CssSelector(".r-bt1l66[data-testid =\"unretweet\"]"));
                retweetBtn.Click();
            }
            catch
            {

                IWebElement retweetBtn = chromeDriver.FindElement(By.CssSelector(".r-bt1l66[data-testid =\"retweet\"]"));
                retweetBtn.Click();
            }

            IWebElement quote = chromeDriver.FindElement(By.CssSelector("a[role=\"menuitem\"][href =\"/compose/tweet\"]"));
            quote.Click();
            Task.Delay(1000).Wait();
            chromeDriver.FindElement(By.CssSelector("div[role=\"textbox\"][data-testid=\"tweetTextarea_0\"]")).SendKeys(comments[random.Next(0, comments.Length)] + fri + " " + tag + "  ");
            Task.Delay(1000).Wait();
            //string javascript = "document.querySelector("div[role ='button'][data-testid='tweetButton']").click()";
            chromeDriver.ExecuteScript("document.querySelector(\"div[role = 'button'][data-testid = 'tweetButton']\").click()");


            //IWebElement confirmQuote = chromeDriver.FindElement(By.CssSelector("div[role=\"button\"][data-testid=\"tweetButton\"]"));
            //confirmQuote.Click();
            Task.Delay(1000).Wait();
            //IWebElement emoj = chromeDriver.FindElement(By.CssSelector(".r-1qi8awa[aria-haspopup=\"menu\"]"));
            //emoj.Click();
            //Task.Delay(1000).Wait();

            //IWebElement icon = chromeDriver.FindElement(By.CssSelector(".r-1qi8awa[aria-label=\"Waving hand\"]"));
            //icon.Click();
            //Task.Delay(1000).Wait();

            //chromeDriver.ExecuteScript("document.querySelectorAll('span[data-text=\"true\"]')[0].innerText=\"aaaaa\";");
            //Task.Delay(1000).Wait();
            //IWebElement retweetConfirm = chromeDriver.FindElement(By.CssSelector(".r-1ny4l3l[data-testid =\"retweetConfirm\"]"));
            //retweetConfirm.Click();
            //}
            //catch
            //{
            //    changeIP();
            //    goto firststep;
            //}

        }
        private void close_Driver(ChromeDriver chromeDriver)
        {
            chromeDriver.Quit();
        }
        private void button2_Click_1(object sender, EventArgs e)
        {

        }
        private void DO_Click(object sender, EventArgs e)
        {

            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions = new ChromeOptions();
            //chromeOptions.AddArgument(@"--user-data-dir=G:\Account\Chrome\1-connorarthurmoss8@gmail.com-84583126062\Data\profile");
            chromeOptions.AddArgument(@"--user-data-dir=" + txtPath.Text);
            chromeOptions.AddArgument("--disable-extensions");
            ChromeDriver chrome = new ChromeDriver(chromeOptions);
            chrome.Manage().Window.Maximize();
            //MessageBox.Show(get_Twitter_Account(chrome));
            try
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    switch (row.Cells["No"].Value.ToString())
                    {
                        case "1":
                            Twitter_LR(chrome, row.Cells["Value"].Value.ToString());
                            break;
                        case "2":
                            Twitter_Comment(chrome, row.Cells["Value"].Value.ToString(), row.Cells["Value2"].Value.ToString());
                            break;
                        case "3":
                            Follow_Twitter(row.Cells["Value"].Value.ToString(), chrome);
                            break;
                    }
                }
                //Follow_Twitter("https://twitter.com/HunnyFinance", chrome);
                //Twitter_Comment(chrome, "https://twitter.com/HunnyFinance/status/1468582365454757888", "a");
                //Twitter_LR(chrome, "https://twitter.com/HunnyFinance/status/1468582365454757888");
                //chrome.Url = "https://twitter.com/HunnyFinance/status/1468582365454757888";
                //chrome.Navigate();

                ////chrome.ExecuteScript("alert(document.getElementsByClassName('r-4qtqp9 r-yyyyoo r-50lct3 r-dnmrzs r-bnwqim r-1plcrui r-lrvibr r-1srniue')[2])");
                ////Task.Delay(5000);
                //chrome.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                //IWebElement likeBtn = chrome.FindElement(By.CssSelector(".r-bt1l66[data-testid =\"like\"]"));
                //IWebElement retweetBtn = chrome.FindElement(By.CssSelector(".r-bt1l66[data-testid =\"retweet\"]"));
                //likeBtn.Click();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Task.Delay(5000).Wait();
            chrome.Quit();

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Dictionary<int, string> job = new Dictionary<int, string>();
            job.Add(1, "Like & Retweet");
            job.Add(2, "Comment & Quote");
            job.Add(3, "Follow");
            cbbJob.DataSource = new BindingSource(job, null);
            cbbJob.ValueMember = "Key";
            cbbJob.DisplayMember = "Value";
            if (!string.IsNullOrEmpty(AutoWhiteList.Properties.Settings.Default.mainFolder))
            {
                txtMainPath.Text = AutoWhiteList.Properties.Settings.Default.mainFolder;
            }
            LoadData();
            LoadJob();

        }
        private void LoadJob()
        {
            DataTable dtJob = connect.readdata("select CategoryID,JobTitle,Value1,Value2 from Job");
            dataGridView1.DataSource = dtJob;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtValue.Text))
            {
                try
                {
                    connect.exedata("insert into Job(CategoryID,JobTitle,Value1,Value2) values(" + cbbJob.SelectedValue.ToString() + ",'" + cbbJob.Text + "','" + txtValue.Text + "','" + txtValue2.Text + "')");
                    LoadJob();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                //dataGridView1.Rows.Add(cbbJob.SelectedValue.ToString(), cbbJob.Text, txtValue.Text, txtValue2.Text);

            }
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Delete)
            {
                Int32 selectedRowCount = dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (selectedRowCount > 0)
                {
                    for (int i = 0; i < selectedRowCount; i++)
                    {
                        dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = getTw() + " " + txtValue2.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //select profile
            using (selectprofile frm = new selectprofile())
            {
                frm.ShowDialog();
            }
        }

        private void btnSelectMainFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtMainPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMainPath.Text))
            {
                AutoWhiteList.Properties.Settings.Default.mainFolder = txtMainPath.Text;
                AutoWhiteList.Properties.Settings.Default.Save();
                var list = Directory.GetDirectories(txtMainPath.Text, "*", SearchOption.TopDirectoryOnly).ToList();
                //MessageBox.Show(list.Count.ToString());
                for (int i = 0; i < list.Count; i++)
                {
                    dataGridView2.Rows.Add(1, list[i].ToString());
                }
                //dataGridView1.DataSource = list;
            }
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelectAll.Checked == true)
            {
                foreach (DataGridViewRow dataGridViewRow in dataGridView2.Rows)
                {
                    dataGridViewRow.Cells["Select"].Value = 1;
                }
            }
            else
            {
                foreach (DataGridViewRow dataGridViewRow in dataGridView2.Rows)
                {
                    dataGridViewRow.Cells["Select"].Value = 0;
                }
            }
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dataGridViewRow in dataGridView2.Rows)
            {
                if (Convert.ToBoolean(dataGridViewRow.Cells["Select"].Value) == true)
                {
                    //do here
                    ChromeOptions chromeOptions = new ChromeOptions();
                    chromeOptions = new ChromeOptions();
                    //chromeOptions.AddArgument(@"--user-data-dir=G:\Account\Chrome\1-connorarthurmoss8@gmail.com-84583126062\Data\profile");
                    chromeOptions.AddArgument(@"--user-data-dir=" + dataGridViewRow.Cells["Path"].Value.ToString() + @"\Data\profile");
                    chromeOptions.AddArgument("--disable-extensions");
                    ChromeDriver chrome = new ChromeDriver(chromeOptions);
                    chrome.Manage().Window.Maximize();
                    //MessageBox.Show(get_Twitter_Account(chrome));
                    try
                    {
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            switch (row.Cells["No"].Value.ToString())
                            {
                                case "1":
                                    Twitter_LR(chrome, row.Cells["Value"].Value.ToString());
                                    break;
                                case "2":
                                    Twitter_Comment(chrome, row.Cells["Value"].Value.ToString(), row.Cells["Value2"].Value.ToString());
                                    break;
                                case "3":
                                    Follow_Twitter(row.Cells["Value"].Value.ToString(), chrome);
                                    break;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    Task.Delay(5000).Wait();
                    chrome.Quit();
                    Task.Delay(5000).Wait();
                    //change ip 
                    // changeIP();
                }

            }
        }

        private void btncopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtCurrentIP.Text);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            changeIP();
        }

        private void txtCurrentIP_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure", "Notice", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                connect.exedata("delete from TrackingIP");
                LoadData();
            }

        }

        private void btnRemoveJobs_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure", "Notice", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    connect.exedata("delete from Job");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void btnconnect_Click(object sender, EventArgs e)
        {
            connectDcom();
        }

        private void btndisconnect_Click(object sender, EventArgs e)
        {
            disconnectDcom();
        }
    }
}
