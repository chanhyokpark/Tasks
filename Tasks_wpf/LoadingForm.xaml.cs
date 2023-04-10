using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumUndetectedChromeDriver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Windows.UI.Xaml.Controls;

namespace Tasks_wpf
{
    /// <summary>
    /// LoadingForm.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoadingForm : Window
    {
        bool running = true;
        List<string> l = new List<string>();
        List<string> tmp=new List<string>();
        UndetectedChromeDriver driver;
        string str;
        public LoadingForm()
        {
            InitializeComponent();
        }

        public void StartGen(List<object> lst)
        {
            
            Status.ItemsSource= l;
            l.Add("시작 중...");
            l.Add("변환 중...");
            str = "write schedule for me. the schedule starts from "+DateTime.Today.ToString("MM.dd")+", and continues until all tasks are finished. " +
                "I have maximum of 3 hours per 1 day, but this limit could be broken if the schedule is impossible. Note that you couldn't break the submission dates. " +
                "Also, write it reasonably. For example, you don't need to pour all time into one task that has many days left." +
                "And, don't forget to mix some tasks together, or the schedule will become boring. For example, math, math, math, ..., science, science is a bad idea. Plus, it is recommended to not do a task more than 2 hours in a day. "+
                "finally, the input format is [name], [submission date], [hours needed], and the data is seperated by semicolon. the answer format must be [date], [task name], [hours], [total time until today/needed time](if there is other task scheduled in that day, you can add that in the same line using same format, seperating with semicolon). And if there is task that is too far away(more than two weeks), you can write that on the end of the response with [task name], [submission date], [hours needed] (same as the input form, if there is more than one tasks too far away, you can seperate with semicolon) from start day Answer with only this format, and do NOT add aditional information. this is important because I want it to be automated. "+
                "After you wrote all schedule, write a daily breifing for each day. the breifing, unlike schedule, must be written in full sentences, and it must contain what to study, what to submit(the submission date is based on the original input, not the completion date based on your schedule). "+
                "here are the conditions: ";
            foreach(var o in lst)
            {
                string v1 = (string)(o?.GetType().GetProperty("name")?.GetValue(o, null));
                string v2 = (string)(o?.GetType().GetProperty("duration")?.GetValue(o, null));
                string v3 = (string)(o?.GetType().GetProperty("neededtime")?.GetValue(o, null));
                str += string.Format("{0}, {1}, {2}; ", v1, v2, v3);
            }
            l.Add("변환된 텍스트: " + str);
            File.WriteAllText("q.txt", str);
            l.Add("GPT 호출 중...");
            var options = new ChromeOptions();
            options.AddArguments("--window-size=1920,1080");
            options.AddArguments("--disable-gpu");
            options.AddArguments("-- disable-extensions");

            options.AddArgument("--headless=new");
            options.AddArguments("--no-sandbox");
            options.AddArguments("--disable-dev-shm-usage");
            options.AddArguments("--verbose");
            options.AddArguments("--whitelisted-ips=''");
            options.AddArgument("--remote-debugging-port=9292");
            options.AddArguments(@"--user-data-dir=C:\Users\ikard\AppData\Local\Google\Chrome\User Data\Selenium");
            driver = UndetectedChromeDriver.Create(driverExecutablePath: "C:\\Users\\ikard\\source\\repos\\Tasks_wpf\\Tasks_wpf\\bin\\Debug\\chromedriver.exe", hideCommandPromptWindow: true, options: options);
            new Thread(delegate () { runCommand(); }).Start();
            l.Add("기다리는 중...");
            
            
        }
        public void runCommand()
        {
            driver.GoToUrl("https://chat.openai.com/chat?model=gpt-4");
            Thread.Sleep(1000);
            /*
            var b = driver.FindElement(By.XPath("//*[@id=\"headlessui-dialog-panel-:r1:\"]/div[2]/div[4]/button"));
            b.Click();
            Thread.Sleep(500);
            b = driver.FindElement(By.XPath("/html/body/div[3]/div/div/div/div[2]/div/div/div[2]/div[4]/button[2]"));
            b.Click();
            Thread.Sleep(500);
            b.Click();
            Thread.Sleep(500);
            MessageBox.Show("b");*/
            var ip = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div[1]/main/div[2]/form/div/div[2]/textarea"));
            ip.SendKeys(str);
            ip.Submit();
            Thread.Sleep(2000);
            var o = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div[1]/main/div[1]/div/div/div/div[3]"));
            var prev = "32";
            l.Clear();
            l.Add(prev);
            
            while(o.Text!=prev)
            {
                prev = o.Text;
                Thread.Sleep(2000);
                l[l.Count-1]=prev;
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(
                delegate
                {

                    Status.Items.Refresh();
                }));
            }
            
            driver.Quit();
            l.Add("제작이 완료되었습니다.\n이 창을 닫아 시간표를 저장하십시오.");
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(
                delegate
                {

                    Status.Items.Refresh();
                }));
            running = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (running == true) e.Cancel = true;
            else
            {
                File.WriteAllText("a.csv", l[0].Replace(";", "\n,"));
            }
        }
        /*
public void runCommand()
{
   //* Create your Process
   Process process = new Process();
   process.StartInfo.FileName = "C:\\Users\\ikard\\AppData\\Local\\Programs\\Python\\Python39\\python.exe";
   process.StartInfo.Arguments = "\"C:\\Users\\ikard\\source\\repos\\Tasks_wpf\\Tasks_wpf\\bin\\Debug\\main.py\"";
   process.StartInfo.UseShellExecute = false;
   process.StartInfo.CreateNoWindow = true;
   process.StartInfo.RedirectStandardOutput = true;
   process.StartInfo.RedirectStandardError = true;
   //* Set your output and error (asynchronous) handlers
   //process.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
   //process.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
   //* Start process and handlers
   process.Start();
   //process.BeginOutputReadLine();
   //process.BeginErrorReadLine();
   process.WaitForExit();
   string output = process.StandardOutput.ReadToEnd();
   string[] ll=output.Split('\n');
   File.WriteAllText("log.txt", output);

   Dispatcher.Invoke(DispatcherPriority.Normal, new Action(
delegate
{
   MessageBox.Show("t");
   l.AddRange(ll);
   Status.Items.Refresh();
}));
}

public void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
{
   //* Do your stuff with the output (write to console/log/StringBuilder)
   try
   {
       tmp.Add(outLine.Data);
       //l.Add(outLine.Data);
       //Status.Items.Refresh();
   }
   catch (Exception ex)
   {

   }
}
*/
    }
}
