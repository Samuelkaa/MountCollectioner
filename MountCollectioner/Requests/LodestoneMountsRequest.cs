using MountCollectioner.Models.Lodestone;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MountCollectioner.Requests
{
    public static class LodestoneMountsRequest
    {
        public static async Task<CharacterInformation> GetCharacterMountsData(int characterID, CancellationToken cancellationToken)
        {
            var uriBuilder = new UriBuilder($"https://ffxivcollect.com/api/characters/{characterID}?ids=1");

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
