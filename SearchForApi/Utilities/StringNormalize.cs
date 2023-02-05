using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SearchForApi.Utilities
{
    public static class StringNormalize
    {
        public static string RemoveNewLines(this string text) => text.Replace('\n', ' ');

        public static string ReplaceSpaceWithDash(this string text) => text.Replace(' ', '-');

        public static string UrlEncoded(this string src) => src == null ? src : Microsoft.AspNetCore.WebUtilities.WebEncoders.Base64UrlEncode(Encoding.ASCII.GetBytes(src));
        public static string UrlDecoded(this string src) => src == null ? src : Encoding.ASCII.GetString(Microsoft.AspNetCore.WebUtilities.WebEncoders.Base64UrlDecode(src));

        public static string NormalizeKeyword(this string text) => text.Replace(" ", "").ToLower();

        public static string CleanKeyword(this string text) => text?.Trim().ToLower();

        public static string FlatItems<T>(this List<T> list) => string.Join('\n', list);

        public static string ToSnakeCase(this string text) => string
            .Concat((text ?? string.Empty)
            .Select((x, i) => i > 0 && char.IsUpper(x) && !char.IsUpper(text[i - 1]) ? $"_{x}" : x.ToString()))
            .ToLower();

        public static bool IsPersian(this string phrase) => Regex.IsMatch(phrase.NormalizeKeyword(), @"^[\u0600-\u06FF]+$");

        public static string RandomToken => Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
    }
}

