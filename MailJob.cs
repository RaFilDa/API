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
    public class MailJob : IJob
    {
        private MyContext myContext = new ();
        public List<string> messageList = new();
        public string body;
        public int lastSentId;
        public string[] mailInfo;
        public string subject;
        public string bodymsg;

        public Task Execute(IJobExecutionContext context)
        {
            myContext.SaveChanges();

            mailInfo = File.ReadAllLines("mailInfo.txt");

            if (Convert.ToBoolean(mailInfo[6]))
            {
                subject = "Backup Error Reports - " + DateTime.Now.ToString("d/M/yyyy");
                bodymsg = "Error reports:";
            }
            else
            {
                subject = "Backup Reports - " + DateTime.Now.ToString("d/M/yyyy");
                bodymsg = "All reports:";
            }

            try {
            lastSentId = Convert.ToInt32(mailInfo[0]);
            foreach (Report report in myContext.Reports.ToList())
            {
                if (Convert.ToBoolean(mailInfo[6]))
                    {if (report.IsError && report.Id > lastSentId) messageList.Add(report.Date + " | " + report.Message);}
                else
                    {if (report.Id > lastSentId) messageList.Add(report.Date + " | " + report.Message);}
            }
            mailInfo[0] = myContext.Reports.ToList().Last().Id.ToString();
            File.WriteAllLines("mailInfo.txt", mailInfo);
            }
            catch { Debug.WriteLine("Nelze se připojit k databázi"); }
            if (messageList.Count == 0)
            { body = "<h1>There were no reports.</h1>"; }
            else
            { body = "<h1>" + bodymsg + "</h1> " + "<br />" + string.Join("<br />", messageList); }
            string from = mailInfo[4];
            List<User> recipients = myContext.Users.ToList();
            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = true;
            SmtpClient smtpServer = new SmtpClient(mailInfo[1]);
            mail.From = new MailAddress(from);
            foreach (User recipient in recipients)
            {
                try{ mail.To.Add(recipient.Email); }
                catch{ Debug.WriteLine("Neplatný email"); };
            }

            var inlineLogo = new LinkedResource(@".\Logo.png", "image/png");
            inlineLogo.ContentId = Guid.NewGuid().ToString();
            body += "<br /> <br /> <br /> <br /> ";
            body += String.Format(@"<br /> <img src=""cid:{0}"" />", inlineLogo.ContentId);
            var view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
            view.LinkedResources.Add(inlineLogo);
            mail.AlternateViews.Add(view);

            mail.Subject = subject;
            mail.Body = body;
            smtpServer.Port = Convert.ToInt32(mailInfo[2]);
            smtpServer.Credentials = new System.Net.NetworkCredential(mailInfo[4], mailInfo[5]);
            smtpServer.EnableSsl = Convert.ToBoolean(mailInfo[3]);
            try { smtpServer.Send(mail); }
            catch(Exception e) { Debug.WriteLine("Send error"); }
            messageList.Clear();
            body = "";
            return Task.CompletedTask;
        }
    }
}