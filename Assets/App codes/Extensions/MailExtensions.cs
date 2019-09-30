using Core;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Extensions
{
    public static class MailExtensions
    {
        private const string reportEmail = "csanadya@gmail.com";


        public static void ReportComment(Comment comment)
        {
            SendEmail(Strings.GetString(Strings.Rating.report_comment), comment.id, reportEmail);
        }

        public static void SendEmail(string subject, string body, string email = reportEmail)
        {
            subject = MyEscapeURL(subject);
            body = MyEscapeURL(body);
            Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
        }

        private static string MyEscapeURL(string url)
        {
            return WWW.EscapeURL(url).Replace("+", "%20");
        }
    }
}
