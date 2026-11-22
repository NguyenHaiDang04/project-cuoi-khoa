using System;
using System.Text.RegularExpressions;

namespace NovaCenter.BUS
{
    internal static class Validation
    {
        public static string Code(string code)
        {
            code = (code ?? "").Trim().ToUpperInvariant();
            if (!Regex.IsMatch(code, @"^[A-Z0-9-]{1,20}$"))
                throw new ArgumentException("Mã gồm 1–20 ký tự chữ, số hoặc dấu gạch ngang.");
            return code;
        }

        public static string Required(string value, int max, string field)
        {
            value = (value ?? "").Trim();
            if (value.Length == 0 || value.Length > max)
                throw new ArgumentException("Nhập " + field + " từ 1 đến " + max + " ký tự.");
            return value;
        }
    }
}
