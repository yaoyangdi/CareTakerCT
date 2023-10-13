using CareTakerCT.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace CareTakerCT.Utils
{
    public class EmailSender
    {
        // Please use your API KEY here.
        private const String API_KEY = "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";

        public async void Send(String fromEmail, String toEmail, String subject, String contents, Files file)
        {
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress(fromEmail, "CareTakerCT Client Email");
            var to = new EmailAddress(toEmail, "");
            var plainTextContent = contents;
            var htmlContent = "<p>" + contents + "</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            
            // Send email with attachment
            if (file != null)    
            {
                // Read from file path("~/Uploads/..") and attach in the email message
                using (var fileStream = File.OpenRead(file.Path))
                {
                    await msg.AddAttachmentAsync(file.Name, fileStream);
                    var response = await client.SendEmailAsync(msg);
                }
            } else     // If file is null then send the plain text
            {
                var response = client.SendEmailAsync(msg);
            }


        }
    }
}