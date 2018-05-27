using System.Net;
using System.Threading.Tasks;
using Lup.Software.Engineering.Domain;
using Lup.Software.Engineering.Models;
using Lup.Software.Engineering.Storage;
using Microsoft.Extensions.Options;
using Lup.Software.Engineering.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Shouldly;

namespace Lup.Software.Engineering.Tests.Controllers
{
    public class ShortenControllerTests
    {
        [Fact]
        public async Task ShortenController_NullRequest_ItReturnsBadRequest()
        {
            // Arrange
            var sut = CreateSUT();

            // Act
            var result = await sut.Create(null);

            // Assert
            result.ShouldBeOfType<ObjectResult>();
            var objectResult = (ObjectResult) result;
            objectResult.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
        }

        //
        // TODO: Many other tests relating to the supplied test scope are possible.
        //

        private ShortenController CreateSUT()
        {
            return new ShortenController(
                new ShortUrlRepository(
                    new TableStorageContext(
                        Options.Create<AppSettings>(new AppSettings
                        {
                            ConnectionStrings = new ConnectionStrings { TableStorage = "UseDevelopmentStorage=true" }
                        }))));
        }
    }
}