using System;
using System.Linq;

namespace Lup.Software.Engineering.RequestResponse
{
    public class ShortenCreateRequest
    {
        private readonly string[] _validSchemes = { Uri.UriSchemeHttp, Uri.UriSchemeHttps, Uri.UriSchemeFtp };

        public string OriginalUrl { get; set; }

        public (bool IsValid, (string, string) [] Errors) Validate()
        {
            return 
                (!Uri.IsWellFormedUriString(OriginalUrl, UriKind.RelativeOrAbsolute) || !_validSchemes.Contains(new Uri(OriginalUrl).Scheme))

            ?
                (IsValid: false, Errors: new[] { (nameof(OriginalUrl), @"The OriginalUrl field is not a valid fully-qualified http, https, or ftp URL.") } )
            :
                (IsValid: true,  Errors: null);
        }
    }
}
