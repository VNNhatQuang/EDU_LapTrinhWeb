using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using _19T1021198.DomainModels;

namespace _19T1021198.Web
{
    /// <summary>
    /// Lớp cung cấp các hàm chuyển đôỉ dữ liệu
    /// </summary>
    public static class Converter
    {
        /// <summary>
        /// Hàm chuyển từ định dạng dd/MM/yyyy sang giá trị ngày (trả về null nếu không xử lý được)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime? DMYStringToDateTime(string s, string format = "d/M/yyyy")
        {
            try
            {
                return DateTime.ParseExact(s, format, CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }

        public static UserAccount CookieToUserAccount(string value)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<UserAccount>(value);
        }

    }
}