using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLayer.Models;
using Navasthala.ViewModel;

namespace Navasthala.MailManager
{
    public static class MailManager
    {
        
        public static void SendWelcomeMessage(UserViewModel userProfile)
        {
            var mailDefinition = new MailDefinition
                {
                    From = "rajeevkanth.m@gmail.com",
                    Subject = "Welcome to Navasthala",
                    IsBodyHtml = true
                };
            var replacements = new ListDictionary
                {
                    {"<%LastName%>", userProfile.LastName},
                    {"<%UserName%>", userProfile.UserName}
                };

            var path = AppDomain.CurrentDomain.BaseDirectory + @"\Mail\WelcomeMail.html";
            var body = System.IO.File.ReadAllText(path);
            var mail = mailDefinition.CreateMailMessage(userProfile.Email,replacements,body,new Control());
            GetSmtpServer().Send(mail);


        }

        public static void SendPasswordResetMail(string token, UserViewModel userProfile)
        {
            var mailDefinition = new MailDefinition
            {
                From = "rajeevkanth.m@gmail.com",
                Subject = "Password reset mail",
                IsBodyHtml = true
            };
            var replacements = new ListDictionary
                {
                    {"<%LastName%>", userProfile.LastName},
                    {"<%Token%>", token}
                };



            var path = AppDomain.CurrentDomain.BaseDirectory + @"\Mail\PasswordResetMail.html";
            var body = System.IO.File.ReadAllText(path);
            var mail = mailDefinition.CreateMailMessage(userProfile.Email, replacements, body, new Control());
            GetSmtpServer().Send(mail);

        }

        private static SmtpClient GetSmtpServer()
        {
            var smtpServer = new SmtpClient
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("rajeevkanth.m@gmail.com", "Apster123"),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true
                };

            return smtpServer;
        }

    }
}
