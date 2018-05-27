using System.Net;
using System.Threading.Tasks;
using Lup.Software.Engineering.Domain;
using Lup.Software.Engineering.RequestResponse;
using Microsoft.AspNetCore.Mvc;

namespace Lup.Software.Engineering.Controllers
{
    public class ShortenController : Controller
    {
        private readonly ShortUrlRepository _shortUrlRepository;

        public ShortenController(ShortUrlRepository shortUrlRepository)
        {
            _shortUrlRepository = shortUrlRepository;
        }

        [HttpPost]
        [Route("api/shorten")]
        public async Task<IActionResult> Create([FromBody]ShortenCreateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.OriginalUrl))
                return new ObjectResult(new ShortenCreateResponse(request?.OriginalUrl, errors: new[] { (nameof(ShortenCreateRequest.OriginalUrl), @"The OriginalUrl field is null or only contains whitespace.")})) { StatusCode = (int)HttpStatusCode.BadRequest };

            var requestValidity = request.Validate();

            var shortUrl = await _shortUrlRepository.GetByOriginalUrl(request.OriginalUrl);
            if (shortUrl == null && requestValidity.IsValid)
            {
                shortUrl = ShortUrl.FromOriginalUrl(request.OriginalUrl);
                await _shortUrlRepository.Save(shortUrl);
            }

            return requestValidity.IsValid
                ? new OkObjectResult(new ShortenCreateResponse(request.OriginalUrl, shortUrl))
                : new ObjectResult(new ShortenCreateResponse(request.OriginalUrl, errors: requestValidity.Errors)) { StatusCode = (int)HttpStatusCode.BadRequest };
        }

        [HttpGet]
        [Route("api/shorten/{shortUrl}")]
        public async Task<IActionResult> Visit(string shortUrl)
        {
            var existing = await _shortUrlRepository.GetByShortUrl(shortUrl);
            if (existing != null)
            {
                existing.Visit();
                await _shortUrlRepository.Save(existing);

                return new RedirectResult(existing.Original, true);
            }

            return this.View("Error");
        }
    }
}
