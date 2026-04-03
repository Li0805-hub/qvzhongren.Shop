using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace qvzhongren.Shared.Helper
{
    public class Tool
    {

        private static readonly string DESIV = "CryDesIv";

        public static string DESEncrypt(string originalValue, string key)
        {
            string result;
            try
            {
                key += "CryDeKey";
                key = key.Substring(0, 8);
                ICryptoTransform transform = new DESCryptoServiceProvider
                {
                    Key = Encoding.UTF8.GetBytes(key),
                    IV = Encoding.UTF8.GetBytes(Tool.DESIV)
                }.CreateEncryptor();
                byte[] bytes = Encoding.UTF8.GetBytes(originalValue);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
                cryptoStream.Write(bytes, 0, bytes.Length);
                cryptoStream.FlushFinalBlock();
                cryptoStream.Close();
                result = Convert.ToBase64String(memoryStream.ToArray());
            }
            catch
            {
                result = originalValue;
            }
            return result;
        }

        public static string DESDecrypt(string encryptedValue, string key)
        {
            string result;
            try
            {
                key += "CryDeKey";
                key = key.Substring(0, 8);
                ICryptoTransform transform = new DESCryptoServiceProvider
                {
                    Key = Encoding.UTF8.GetBytes(key),
                    IV = Encoding.UTF8.GetBytes(Tool.DESIV)
                }.CreateDecryptor();
                byte[] array = Convert.FromBase64String(encryptedValue);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
                cryptoStream.Write(array, 0, array.Length);
                cryptoStream.FlushFinalBlock();
                cryptoStream.Close();
                result = Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            catch
            {
                result = encryptedValue;
            }
            return result;
        }


        /// <summary>
        /// 字符串自动补0
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="length">目标长度</param>
        /// <returns>补0后的字符串</returns>
        public static string PadZero(string input, int length)
        {
            if (string.IsNullOrEmpty(input))
                return new string('0', length);

            if (input.Length >= length)
                return input;

            return input.PadLeft(length, '0');
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input">需要加密的字符串</param>
        /// <returns>MD5加密后的字符串</returns>
        public static string MD5Encrypt(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);
                
                var sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// 计算年龄，根据时间差返回合适的描述
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>年龄描述（xx分钟/xx小时/xx天/xx月/xx岁）</returns>
        public static string GetAge(DateTime startTime, DateTime endTime)
        {
            var timeSpan = endTime - startTime;

            // 不足1小时，返回分钟
            if (timeSpan.TotalHours < 1)
            {
                return $"{Math.Floor(timeSpan.TotalMinutes)}分钟";
            }

            // 不足1天，返回小时
            if (timeSpan.TotalDays < 1)
            {
                return $"{Math.Floor(timeSpan.TotalHours)}小时";
            }

            // 不足30天，返回天数
            if (timeSpan.TotalDays < 30)
            {
                return $"{Math.Floor(timeSpan.TotalDays)}天";
            }

            // 计算月份差
            int months = ((endTime.Year - startTime.Year) * 12) + endTime.Month - startTime.Month;
            if (endTime.Day < startTime.Day)
            {
                months--;
            }

            // 不足1岁，返回月份
            if (months < 12)
            {
                return $"{months}月";
            }

            // 大于等于1岁，返回年龄
            int years = months / 12;
            return $"{years}岁";
        }

        
    }
}