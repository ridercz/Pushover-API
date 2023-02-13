namespace Altairis.Pushover.Client;
internal static class Extensions {

    public static HttpResponseMessage EnsurePushoverSuccessStatusCode(this HttpResponseMessage message)
        => message.StatusCode is System.Net.HttpStatusCode.OK or System.Net.HttpStatusCode.BadRequest
            ? message
            : throw new HttpRequestException("Unexpected HTTP status code.", null, message.StatusCode);

}
