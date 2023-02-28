using System.Text;
using System;

namespace VladB.Utility
{
    public static partial class Extensions
    {
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static bool IsHaveSomething(this string s)
        {
            return !string.IsNullOrEmpty(s);
        }

        public static string GetRandomPPKey(int length = 20)
        {
            return GetRandomGUID(length);
        }

        public static string GetRandomGUID(int length = 20)
        {
            string s1 = Guid.NewGuid().ToString();
            byte[] s2 = Encoding.UTF8.GetBytes(s1);
            string s3 = Convert.ToBase64String(s2);
            return s3.Substring(0, length);
        }
    }
}