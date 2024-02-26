using System;
using System.Globalization;
using System.Text;

namespace Utilities
{
    public class TitleNormalizer
    {
        public static string NormalizeTitle(string title)
        {
            title = title.Replace("☆", " ");
            title = title.ToLower();
            title = RemoveSpecialCharacters(title);
            title = title.Replace(" ", "-");
            return title;
        }

        private static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
