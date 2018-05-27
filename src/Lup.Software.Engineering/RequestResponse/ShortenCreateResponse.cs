using System.Collections.Generic;
using System.Linq;

namespace Lup.Software.Engineering.RequestResponse
{
    public class ShortenCreateResponse
    {
        public ShortenCreateResponse(
            string originalUrl,
            string shortUrl = null,
            params (string, string)[] errors
            )
        {
            ShortUrl = shortUrl;
            OriginalUrl = originalUrl;
            Errors = errors?.ToDictionary(error => error.Item1, error => error.Item2);
        }

        public string ShortUrl { get; set; }

        public string OriginalUrl { get; }

        public Dictionary<string, string> Errors { get; set; }
    }
}
