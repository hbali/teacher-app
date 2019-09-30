using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class Strings
    {
        public static string GetString(string s)
        {
            string translated = "";
            return I2.Loc.LocalizationManager.TryGetTranslation(s, out translated) ? translated : s; 
        }

        public static class Account
        {
            public const string error_email_format = "error_email_format";
            public const string teacher_doesnt_exist = "teacher_doesnt_exist";
            public const string student_doesnt_exist = "student_doesnt_exist";
            public const string success_register = "success_register";
            public const string click_button_to_login = "click_button_to_login";
            public const string account_already_exists = "account_already_exists";
            public const string login = "login";
            public const string male = "Male";
            public const string female = "Female";
            public const string no_subjects_reg = "no_subjects_reg";
            public const string error01 = "Something went wrong";
            public const string try_again = "Please try again later";
            public const string logging_in = "logging_in";

        }

        public static class Rating
        {
            public const string All = "All";
            public const string rating = "Rating";
            public const string rateSuccess = "success_rate";
            public const string rateUnsuccess = "unsuccess_rate";
            public const string empty_text = "empty_rate_text";
            public const string rate_yourself = "rate_yourself";
            public const string report_comment = "report_comment";
            public const string rate_more_than_once = "rate_more_than_once";
        }

        public static class Subjects
        {
            public const string successSubjectsAdded = "success_subjects_added";
            public const string failSubjectsAdded = "fail_subjects_added";
        }

        internal static string GetString(object oK)
        {
            throw new NotImplementedException();
        }

        public static class Database
        {
            public const string Faculties = "Faculties";
            public const string Teachers = "Teachers";
            public const string Students = "Students";
        }

        public static class UI
        {
            public const string universities = "Universities";
            public const string subjects = "Subjects";
            public const string OK = "OK";
        }

        public static class Paths
        {
            public const string DefaultProfPic = "Icons/DefaultProfile";
        }

        public static class Languages
        {
            public const string Hungarian = "Hungarian";
            public const string English = "English";
        }
    }
}
