using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Timer
{
    class Program
    {
        //Test comment
        static void Main(string[] args)
        {
            var command = "";

            var tomatoCount = 0;
           

            do
            {
                Console.Clear();
                Console.WriteLine("Date: " + DateTime.Now.ToString());
                Console.WriteLine("Tomato count: " + tomatoCount);
                Console.WriteLine("Enter 'b' for break, 'j' for work or 'exit' for close");
                SystemSounds.Beep.Play();
                command = Console.ReadLine();
                int minutes;

                if(command == "j")
                {
                    minutes = 25;
                    for (var i = 0; i< minutes; i++)
                    {
                        Console.WriteLine(i +1+ " minute");
                        Thread.Sleep(1000 * 60);
                    }
                    SystemSounds.Beep.Play();
                    if (!SendPomodoro())
                    {
                        Console.WriteLine("Error saving pomodoro");
                    }
                    tomatoCount++;
                    MessageBox.Show("Need break");

                }
                else if(command == "b")
                {
                    minutes = 5;
                    for (var i = 0; i < minutes; i++)
                    {
                        Console.WriteLine(i+1 + " minute");
                        Thread.Sleep(1000 * 60);
                    }
                    SystemSounds.Beep.Play();
                    MessageBox.Show("Need work");
                }
                

            } while (command != "exit");
        }

        static bool SendPomodoro()
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(Config.PomodoroURL);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";
            var date = DateTime.Now;
            var stringDate = "";

            stringDate += (date.Month >= 10) ? date.Month.ToString() : "0" + date.Month.ToString();
            stringDate += ".";
            stringDate += (date.Day >= 10) ? date.Day.ToString() : "0" + date.Day.ToString();
            stringDate += ".";
            stringDate += date.Year + " " + date.ToLongTimeString();

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"Code\":\""+Config.Code.ToString()+"\"," +
                              "\"Date\":\""+ stringDate + "\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            try {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if(httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}
