using MountCollectioner.Models;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MountCollectioner.Requests
{
    public static class FFXIVCollectRequest
    {
        public static async Task<MountDataResponse> GetMountsData(uint selectedMountId, CancellationToken cancellationToken)
        {
            var uriBuilder = new UriBuilder($"https://ffxivcollect.com/api/mounts/{selectedMountId}");

            cancellationToken.ThrowIfCancellationRequested();

            using var client = new HttpClient();
            var res = await client
              .GetStreamAsync(uriBuilder.Uri, cancellationToken)
              .ConfigureAwait(false);

            cancellationToken.ThrowIfCancellationRequested();

            var parsedRes = await JsonSerializer
              .DeserializeAsync<MountDataResponse>(res, cancellationToken: cancellationToken)
              .ConfigureAwait(false);

            return parsedRes;
        }
    }
}
