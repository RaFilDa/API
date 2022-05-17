using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Quartz;
using RaFilDaAPI.Entities;

namespace RaFilDaAPI
{
    [DisallowConcurrentExecution]
    public class MailJob : IJob
    {
        private readonly MyContext myContext;
        public List<string> messageList = new List<string>{};
        public string body;
        public int lastSentId;

        public MailJob(MyContext myContext)
        {
            this.myContext = myContext;
        }
        public Task Execute(IJobExecutionContext context)
        {
            myContext.SaveChanges();

            string subject = "Backup Error Reports - " + DateTime.Now.ToString("d/M/yyyy");

            try {
            lastSentId = Convert.ToInt32(File.ReadAllText("mailInfo.txt"));
            foreach (Report report in myContext.Reports.ToList())
            {
                if (report.IsError && report.Id > lastSentId) messageList.Add(report.Message);
            }
            File.WriteAllText("mailInfo.txt", myContext.Reports.ToList().Last().Id.ToString());
            }
            catch { Debug.WriteLine("Nelze se připojit k databázi"); }
            if (messageList.Count == 0)
            { body = "<h1>There were no error reports.</h1>"; }
            else
            { body = "<h1>Error reports:</h1> " + "<br />" + string.Join("<br />", messageList); }
            string from = "RaFilDaReports@post.cz";
            List<User> recipients = myContext.Users.ToList();
            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = true;
            SmtpClient smtpServer = new SmtpClient("smtp.seznam.cz");
            mail.From = new MailAddress(from);
            foreach (User recipient in recipients)
            {
                try{ mail.To.Add(recipient.Email); }
                catch{ Debug.WriteLine("Neplatný email"); };
            }

            var inlineLogo = new LinkedResource(@".\Logo.png", "image/png");
            inlineLogo.ContentId = Guid.NewGuid().ToString();
            body += "<br /> <br /> <br /> <br /> ";
            body += (@"<br /> <img src=""cid:{0}"" />", inlineLogo.ContentId).ToString();
            var view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
            view.LinkedResources.Add(inlineLogo);
            mail.AlternateViews.Add(view);

            mail.Subject = subject;
            mail.Body = body;
            smtpServer.Port = 587; 
            smtpServer.Credentials = new System.Net.NetworkCredential("RaFilDaReports@post.cz", "123456Ab");
            smtpServer.EnableSsl = false;
            try { smtpServer.Send(mail); }
            catch { Debug.WriteLine("Žádný příjemce"); }
            messageList.Clear();
            body = "";
            return Task.CompletedTask;
        }
    }
}