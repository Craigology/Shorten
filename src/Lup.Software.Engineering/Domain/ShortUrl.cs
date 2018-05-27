using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lup.Software.Engineering.Domain
{
    public class ShortUrl : TableEntity
    {
        public static implicit operator string(ShortUrl shortUrl) => shortUrl.Short;

        public const int MaxLength = 7;

        public static readonly Version Version = new Version(1, 0, 0);

        private static readonly Regex ValidShortPattern = new Regex(@"^[A-Za-z0-9]{7}$", RegexOptions.Compiled);

        public string Short { get; set; }

        public string Original { get; set; }

        public int Visits { get; set; }

        public static ShortUrl FromOriginalUrl(string original)
        {
            return new ShortUrl { Original = original, Short = Shorten(original) };
        }

        public int Visit()
        {
            return ++Visits;
        }

        private static string Shorten(string original)
        {
            var shortCandidate = string.Empty;

            while (!ValidShortPattern.IsMatch(shortCandidate))
            {
                var iteration = 0;

                shortCandidate = Guid
                    .NewGuid()
                    .ToString("N")
                    .Substring(0, MaxLength)
                    .ToLower();

                Debug.WriteLine($"ShortUrl.Shorten({original}): Iteration: {iteration++} => {shortCandidate}");
            }

            return shortCandidate;
        }
    }
}