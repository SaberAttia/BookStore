using BookStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Domain.Entities;
using System.Net.Mail;
using System.Net;
using System.Diagnostics;

namespace BookStore.Domain.Concrete
{
    public class EmailSettings  //139
    {
        public string MailToAddress = "animearabs0@gmail.com";
        public string MailFromAddress = "saber100111@gmail.com";
        public bool UseSsl = true;
        public string Username = "saber100111@gmail.com";
        public string Password = "sab55538";
        public string ServerName = "smtp.gmail.com";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        public string FileLocation = @"C:\orders_bookstore_emails";
    }
    public class EmailOrderProcessor : IOrderProcessor //140,141
    {
        private EmailSettings emailSetting; //object form above class
        public EmailOrderProcessor(EmailSettings setting) //Constructor from current class
        {
            emailSetting = setting;  //take setting and and deliver it to emailSetting
        }
        public void ProcessOrder(Cart cart, ShippingDetails shippingDetails) //141
        {
            using (var smtpClient = new SmtpClient()) //142
            {
                smtpClient.EnableSsl = emailSetting.UseSsl;
                smtpClient.Host = emailSetting.ServerName;
                smtpClient.Port = emailSetting.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new 
                            NetworkCredential(emailSetting.Username, emailSetting.Password);
                if (emailSetting.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSetting.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder()
                   .AppendLine("A new order has been submitted")
                   .AppendLine("------------")
                   .AppendLine("Books: ");
                foreach(var line in cart.Lines)
                {
                    var subtotal = line.Book.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (subtotal: {2:c})", line.Quantity, line.Book.Title, subtotal);
                }
                body.AppendFormat("Total order value:{0:c}", cart.ComputeTotalValue())
                         .AppendLine("------------")
                         .AppendLine("Ship to: ")
                         .AppendLine(shippingDetails.Name)
                         .AppendLine(shippingDetails.Line1)
                         .AppendLine(shippingDetails.Line2)
                         .AppendLine(shippingDetails.State)
                         .AppendLine(shippingDetails.City)
                         .AppendLine(shippingDetails.Country)
                         .AppendLine("------------")
                         .AppendFormat("Gift wrap: {0} ", shippingDetails.GiftWrap ? "Yes" : "No");

                MailMessage mailMessage = new MailMessage(
                                                            emailSetting.MailFromAddress,
                                                            emailSetting.MailToAddress,
                                                            "new order submitted",
                                                            body.ToString());

                if (emailSetting.WriteAsFile)
                    mailMessage.BodyEncoding = Encoding.ASCII;
                try {  //154
                smtpClient.Send(mailMessage);
                }catch (Exception ex)
                {
                   Debug.Print(ex.Message) ;
                }
            }
        }
    }
}
