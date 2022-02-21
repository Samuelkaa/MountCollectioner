using MountCollectioner.Models.Lodestone;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MountCollectioner.Requests
{
    public static class LodestoneSearchCharRequest
    {
        public static async Task<LodestoneSearchResults> SearchCharacters(string characterName, string selectedWorld, CancellationToken cancellationToken)
        {
            var uriBuilder = new UriBuilder($"https://xivapi.com/character/search?name={characterName}&server={selectedWorld}");

            cancellationToken.ThrowIfCancellationRequested();

            using var client = new HttpClient();
            var res = await client
              .GetStreamAsync(uriBuilder.Uri, cancellationToken)
              .ConfigureAwait(false);

            cancellationToken.ThrowIfCancellationRequested();

            var parsedRes = await JsonSerializer
              .DeserializeAsync<LodestoneSearchResults>(res, cancellationToken: cancellationToken)
              .ConfigureAwait(false);

            return parsedRes;
        }
    }
}
