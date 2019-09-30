using System.Text.RegularExpressions;

namespace Core.Validation
{
    public class EmailValidator
    {
        private const string Pattern = 
            "^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*"
            + "@"
            + "[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$";

        public static bool Validate(string value)
        {
             return (Regex.IsMatch(value, Pattern));
        }
    }
}
