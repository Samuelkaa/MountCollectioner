using MountCollectioner.Serialize.Lodestone;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MountCollectioner.Requests
{
    public static class LodestoneRequest
    {
        public static async Task<CharacterInformation> GetCharacterMountsData(CancellationToken cancellationToken)
        {
            var uriBuilder = new UriBuilder($"https://xivapi.com/character/32983470?data=MIMO");

            cancellationToken.ThrowIfCancellationRequested();

            using var client = new HttpClient();
            var res = await client
              .GetStreamAsync(uriBuilder.Uri, cancellationToken)
              .ConfigureAwait(false);

            cancellationToken.ThrowIfCancellationRequested();

            var parsedRes = await JsonSerializer
              .DeserializeAsync<CharacterInformation>(res, cancellationToken: cancellationToken)
              .ConfigureAwait(false);

            return parsedRes;
        }
    }
}
