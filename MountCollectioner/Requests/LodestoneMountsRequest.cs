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
        public static async Task<List<ObtainedMounts>> GetCharacterMountsData(int characterID, CancellationToken cancellationToken)
        {
            var uriBuilder = new UriBuilder($"https://ffxivcollect.com/api/characters/{characterID}/mounts/owned");

            cancellationToken.ThrowIfCancellationRequested();

            using var client = new HttpClient();
            var res = await client
              .GetStreamAsync(uriBuilder.Uri, cancellationToken)
              .ConfigureAwait(false);

            cancellationToken.ThrowIfCancellationRequested();

            var parsedRes = await JsonSerializer
              .DeserializeAsync<List<ObtainedMounts>>(res, cancellationToken: cancellationToken)
              .ConfigureAwait(false);

            return parsedRes!;
        }
    }
}
