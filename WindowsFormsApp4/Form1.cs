using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using  Keys= OpenQA.Selenium.Keys;


namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
      
private IWebDriver driver = new ChromeDriver(@"C:\Users\dimad\Desktop");
public Form1()
        {
            InitializeComponent();
            FirstLogin();
            pause(10000);
            string bosslikeTasks = "https://api-public.bosslike.ru/v1/bots/tasks/?service_type=3&task_type=3";
            for (int j = 0; j < 40; j++)
            {
                var jobj = GetJson(bosslikeTasks);
                for (int i = 0; i < 20; i++)
                {


                    string taskId = jobj["data"]["items"][i]["id"].ToString();
                    var taskInfo = GetJson("https://api-public.bosslike.ru/v1/bots/tasks/" + taskId + "/do/");
                    if (GetJson("https://api-public.bosslike.ru/v1/bots/tasks/" + taskId + "/do/") == null)
                    {
                        continue;
                    }

                    int delay = Int32.Parse((string)taskInfo["data"]["seconds"]);
                    Random rndi = new Random();
                    int sluchay = rndi.Next(0, 3000);
                    pause(5000 + delay + sluchay);

                    string instUrl = taskInfo["data"]["url"].ToString();

                    driver.Url = instUrl;
                    try
                    {
                        driver.FindElement(By.XPath("/html/body/div[1]/section/main/div/header/section/div[1]/div[1]/span/span[1]/button")).Click();
                        pause(2000 + sluchay);
                    }
                    catch
                    {
                        pause(2000 + sluchay);
                    }
                    var taskCheck = GetJson("https://api-public.bosslike.ru/v1/bots/tasks/" + taskId + "/check/");
                    if ((taskCheck == null) || (taskCheck["status"].ToString() == null))
                    {
                        continue;
                    }

                    string prov = taskCheck["status"].ToString();
                    if (prov != "200")
                    {
                        var taskCheck2 = GetJson("https://api-public.bosslike.ru/v1/bots/tasks/" + taskId + "/check/");
                        string prov2 = taskCheck2["status"].ToString();
                        if (prov2 != "200")
                        {
                            continue;
                        }
                    }



                    GC.Collect();
                }
                Random rnd = new Random();
                int randi = rnd.Next(14000, 24000);
                pause(randi*(j^2));
                GC.Collect();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           





        }
        public void pause(int vr)
        {
            double time = DateTime.Now.TimeOfDay.TotalMilliseconds;
            while ((DateTime.Now.TimeOfDay.TotalMilliseconds - time) < vr)
            {
                Application.DoEvents();
            }

        }
        private static JObject GetJson(string url)
        {
            HttpWebRequest request =
             (HttpWebRequest)WebRequest.Create(url);

            request.Method = "GET";
            request.Accept = "application/json";
            try
            {
                request.Headers["X-Api-Key"] = "04e86f019e06d3a1ae861d6c87e29fcf722385bee27bfbeb";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                StringBuilder output = new StringBuilder();
                output.Append(reader.ReadToEnd());
                response.Close();
                var promJsonTasks = (JObject)JsonConvert.DeserializeObject(output.ToString());
                return promJsonTasks;
            }
            catch
            {
                JObject promJsonTasks = null;
                return promJsonTasks;
            }
            
           
        }
        private void FirstLogin()
        {

            driver.Url = "https://www.instagram.com/anyavlasova2020/";
            driver.FindElement(By.TagName("button")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Name("username")).SendKeys("anyavlasova2020");
            driver.FindElement(By.Name("password")).SendKeys("Dimas135");
            driver.FindElement(By.Name("password")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Name("password")).SendKeys(Keys.Enter);
            Thread.Sleep(2000);
        }

    }

   
}

