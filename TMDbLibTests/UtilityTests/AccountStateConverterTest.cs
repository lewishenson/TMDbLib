using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.TvShows;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests
{
    public class AccountStateConverterTest : TestBase
    {
        [Fact]
        public void DeserializeAccountState_WithRatedData()
        {
            // {"favorite":true,"id":100,"rated":{"value":5},"watchlist":true}

            var original = new
            {
                favorite = true,
                id = 100,
                rated = new { value = 5 },
                watchlist = true
            };

            string json = Serializer.Serialize(original);
            AccountState result = Serializer.Deserialize<AccountState>(json);

            Verify(new
            {
                json,
                result
            });
        }

        [Fact]
        public void DeserializeAccountState_WithoutRatedData()
        {
            // {"favorite":true,"id":100,"rated":{"value":5},"watchlist":true}

            var original = new
            {
                favorite = true,
                id = 100,
                rated = false,
                watchlist = true
            };

            string json = Serializer.Serialize(original);
            AccountState result = Serializer.Deserialize<AccountState>(json);

            Verify(new
            {
                json,
                result
            });
        }

        /// <summary>
        /// Tests the AccountStateConverter on the AccountState type
        /// </summary>
        [Fact]
        public async Task TestAccountStateConverterAccountState()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
            AccountState accountState = await TMDbClient.GetMovieAccountStateAsync(IdHelper.Avatar);

            await Verify(accountState);
        }

        /// <summary>
        /// Tests the AccountStateConverter on the TvEpisodeAccountState type
        /// </summary>
        [Fact]
        public async Task TestAccountStateConverterTvEpisodeAccountState()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
            ResultContainer<TvEpisodeAccountStateWithNumber> season = await TMDbClient.GetTvSeasonAccountStateAsync(IdHelper.BigBangTheory, 1);

            // Episode 1 has a rating
            TvEpisodeAccountStateWithNumber episodeA = season.Results.Single(s => s.EpisodeNumber == 1);

            Assert.NotNull(episodeA.Rating);

            // Episode 2 has no rating
            TvEpisodeAccountStateWithNumber episodeB = season.Results.Single(s => s.EpisodeNumber == 2);
            Assert.Null(episodeB.Rating);

            await Verify(new
            {
                episodeA,
                episodeB
            });
        }
    }
}