using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Domain.Entities;
using System.Net.Mail;
using System.Net;

namespace BookStore.Domain.Concrete
{
    // Assign email settings here that i will pass it to EmailOrderProcessor constructor
    public class EmailSettings
    {
        public string MailToAddress = "amanyawadallah27@gmail.com";
        public string MailFromAddress = "Mohammed.Awadallah@gmail.com";
        public bool UseSsl = true;
        public string Username = "Mohammed.Awadallah@gmail.com";
        public string Password = "22128225181";
        public string ServerName = "smtp.gmail.com";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        public string FileLocation = @"e:\BookStore_Emails";
    }
    // EmailOrderProcessor that take email setting and implement ProcessOrder that from IOrderProcessor

    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;
        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }
        //Here we make new smtpclient that is responsible about sendin the order as email
        public void ProcessOrder(Cart cart, ShippingDetails shippingInfo)
        {
           using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);

                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                // here we use new stringbuilder to compose message body
                StringBuilder body = new StringBuilder()
                    .AppendLine("A new order has been submitted")
                    .AppendLine("---")
                    .AppendLine("Books:");

                foreach (var line in cart.Lines)
                {
                    body.AppendFormat("\n");
                    var subtotal = line.Book.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (subtotal:{2:c})",
                                       line.Quantity, line.Book.Title, subtotal);
                    body.AppendFormat("\n");
                   
                }
                body.AppendFormat("---------------------------------------------------\n");
                body.AppendFormat("Total order value: {0:c}", cart.ComputeTotalValue())
                    .AppendLine()
                    .AppendLine("Ship to:")
                    .AppendLine(shippingInfo.Name)
                    .AppendLine(shippingInfo.Line1)
                    .AppendLine(shippingInfo.Line2 ?? "")
                    .AppendLine(shippingInfo.City)
                    .AppendLine(shippingInfo.State)
                    .AppendLine(shippingInfo.Country)
                    .AppendFormat("Is A Gift:{0}", shippingInfo.IsGift ? "Yes" : "No");
                MailMessage mailMessage = new MailMessage(emailSettings.MailFromAddress,
                                                          emailSettings.MailToAddress,
                                                          "New order submitted",
                                                          body.ToString());
                if (emailSettings.WriteAsFile) mailMessage.BodyEncoding = Encoding.ASCII;
                smtpClient.Send(mailMessage);



            }
        }
    }
}
