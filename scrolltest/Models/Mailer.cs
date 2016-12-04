using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;


namespace scrolltest.Models
{
    public class Mailer
    {
       private MailMessage messge;
       private SmtpClient client;

        public Mailer(string mail, string subject, string body)
        {
            messge = new MailMessage();
            messge.From = new MailAddress("info@dirtysecrets.eu");
            messge.IsBodyHtml = true;
            client = new SmtpClient("smtp.zoho.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential("info@dirtysecrets.eu", "qazwsx123");
            messge.To.Add(new MailAddress(mail));
            messge.Subject = subject;
            messge.Body = body;
           
        }

        public void Send()
        {
            client.Send(messge);
        }

    }

   


}