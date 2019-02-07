using Microsoft.Extensions.Options;

namespace BookStore.Api.Identity.UnitTests.Fixtures
{
    public class AppSettingsFixture : IOptions<AppSettings>
    {
        public AppSettings Value => new AppSettings { Secret = "Fake secret key. Should be replaced by actual key." };
    }
}