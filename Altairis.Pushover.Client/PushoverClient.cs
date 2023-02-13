using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json;

namespace Altairis.Pushover.Client;

public class PushoverClient {
    private const string API_ADDRESS = "https://api.pushover.net/1/";

    private readonly string token;
    private readonly HttpClient httpClient;
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public PushoverClient(string token) {
        this.token = token;

        // Initialize HTTP client
        this.httpClient = new HttpClient();
        this.httpClient.BaseAddress = new Uri(API_ADDRESS);

        // Initialize JSON serializer options
        this.jsonSerializerOptions = new JsonSerializerOptions();
        this.jsonSerializerOptions.Converters.Add(new NumericBooleanConverter());
        this.jsonSerializerOptions.Converters.Add(new UnixToDateTimeOffsetConverter());
        this.jsonSerializerOptions.Converters.Add(new UnixToNullableDateTimeOffsetConverter());
        this.jsonSerializerOptions.PropertyNameCaseInsensitive = true;
        this.jsonSerializerOptions.PropertyNamingPolicy = new JorgeSerrano.Json.JsonSnakeCaseNamingPolicy();
    }

    // General

    public Task<PushoverLimitsResponse> GetLimits(CancellationToken cancellationToken = default)
        => this.PerformGetRequest<PushoverLimitsResponse>($"apps/limits.json?token={this.token}", cancellationToken);

    public Task<PushoverSoundsResponse> GetSounds(CancellationToken cancellationToken = default)
        => this.PerformGetRequest<PushoverSoundsResponse>($"sounds.json?token={this.token}", cancellationToken);

    public Task<PushoverValidateResponse> ValidateUserOrGroup(string userOrGroup, string? device = null, CancellationToken cancellationToken = default)
        => this.PerformPostRequest<PushoverValidateResponse>($"users/validate.json?token={this.token}", new { user = userOrGroup, device }, cancellationToken);

    // Messaging

    public async Task<PushoverMessageResponse> SendMessage(PushoverMessage message, string? attachmentFileName = null, Stream? attachmentStream = null, CancellationToken cancellationToken = default) {
        // Prepare form fields
        var form = new Dictionary<string, string> {
            { "user", message.User },
            { "message", message.Message }
        };
        if (!string.IsNullOrEmpty(message.Title)) form.Add("title", message.Title);
        if (!string.IsNullOrEmpty(message.Device)) form.Add("device", message.Device);
        if (message.Format == MessageFormat.Html) form.Add("html", "1");
        if (message.Format == MessageFormat.Monospace) form.Add("monospace", "1");
        if (message.Timestamp != null) form.Add("timestamp", message.Timestamp.Value.ToUnixTimeSeconds().ToString());
        if (message.SupplementaryUrl != null) {
            form.Add("url", message.SupplementaryUrl.ToString());
            if (!string.IsNullOrEmpty(message.SupplementaryUrlTitle)) form.Add("url_title", message.SupplementaryUrlTitle);
        }
        if (!string.IsNullOrEmpty(message.Sound)) form.Add("sound", message.Sound);
        switch (message.Priority.Level) {
            case MessagePriorityLevel.Lowest:
                form.Add("priority", "-2");
                break;
            case MessagePriorityLevel.Low:
                form.Add("priority", "-1");
                break;
            case MessagePriorityLevel.High:
                form.Add("priority", "1");
                break;
            case MessagePriorityLevel.Emergency:
                form.Add("priority", "2");
                form.Add("retry", Math.Round(message.Priority.Retry.TotalSeconds).ToString());
                form.Add("expire", Math.Round(message.Priority.Expire.TotalSeconds).ToString());
                if (message.Priority.Callback != null) form.Add("callback", message.Priority.Callback.ToString());
                if (message.Priority.Tags.Any()) form.Add("tags", string.Join(',', message.Priority.Tags));
                break;
            default:
                break;
        }

        // Prepare POST content with form fields
        var content = new MultipartFormDataContent();
        foreach (var item in form) content.Add(new StringContent(item.Value), item.Key);

        // Add attachment
        if (!string.IsNullOrEmpty(attachmentFileName)) content.Add(new StreamContent(attachmentStream ?? File.OpenRead(attachmentFileName)), "attachment", Path.GetFileName(attachmentFileName));

        // Send HTTP request
        var response = await this.httpClient.PostAsync($"messages.json?token={this.token}", content, cancellationToken);

        // Parse HTTP response
        var result = await response.EnsurePushoverSuccessStatusCode().Content.ReadFromJsonAsync<PushoverMessageResponse>(this.jsonSerializerOptions, cancellationToken);
        return result ?? throw new Exception("Error deserializing API response.");
    }

    public Task<PushoverReceiptResponse> GetMessageReceipt(string receipt, CancellationToken cancellationToken = default)
        => this.PerformGetRequest<PushoverReceiptResponse>($"receipts/{receipt}.json?token={this.token}", cancellationToken);

    public Task<PushoverResponse> CancelMessageByReceipt(string receipt, CancellationToken cancellationToken = default)
        => this.PerformPostRequest<PushoverResponse>($"receipts/{receipt}/cancel.json?token={this.token}", cancellationToken);

    public Task<PushoverResponse> CancelMessageByTag(string tag, CancellationToken cancellationToken = default)
    => this.PerformPostRequest<PushoverResponse>($"receipts/cancel_by_tag/{tag}.json?token={this.token}", cancellationToken);

    // Groups

    public Task<PushoverGroupResponse> GetGroup(string group, CancellationToken cancellationToken = default)
        => this.PerformGetRequest<PushoverGroupResponse>($"groups/{group}.json?token={this.token}", cancellationToken);

    public Task<PushoverResponse> RenameGroup(string group, string newName, CancellationToken cancellationToken = default)
        => this.PerformPostRequest<PushoverResponse>($"groups/{group}/rename.json?token={this.token}", new { name = newName }, cancellationToken);

    public Task<PushoverResponse> AddUserToGroup(string group, string user, string? device = null, string? memo = null, CancellationToken cancellationToken = default)
        => this.PerformPostRequest<PushoverResponse>($"groups/{group}/add_user.json?token={this.token}", new { user, device, memo }, cancellationToken);

    public Task<PushoverResponse> DeleteUserFromGroup(string group, string user, CancellationToken cancellationToken = default)
        => this.PerformPostRequest<PushoverResponse>($"groups/{group}/delete_user.json?token={this.token}", new { user }, cancellationToken);

    public Task<PushoverResponse> DisableUserInGroup(string group, string user, CancellationToken cancellationToken = default)
        => this.PerformPostRequest<PushoverResponse>($"groups/{group}/disable_user.json?token={this.token}", new { user }, cancellationToken);

    public Task<PushoverResponse> EnableUserInGroup(string group, string user, CancellationToken cancellationToken = default)
        => this.PerformPostRequest<PushoverResponse>($"groups/{group}/enable_user.json?token={this.token}", new { user }, cancellationToken);

    // Private helper methods

    private async Task<T> PerformGetRequest<T>(string url, CancellationToken cancellationToken = default) where T : PushoverResponse {
        var response = await this.httpClient.GetAsync(url, cancellationToken);
        var result = await response.EnsurePushoverSuccessStatusCode().Content.ReadFromJsonAsync<T>(this.jsonSerializerOptions, cancellationToken);
        return result ?? throw new Exception("Error deserializing API response.");
    }

    private async Task<T> PerformPostRequest<T>(string url, object? parameters, CancellationToken cancellationToken = default) where T : PushoverResponse {
        // Send HTTP request
        var response = await this.httpClient.PostAsync(url, GetFormFromParameters(parameters), cancellationToken);

        // Parse HTTP response
        var result = await response.EnsurePushoverSuccessStatusCode().Content.ReadFromJsonAsync<T>(this.jsonSerializerOptions, cancellationToken);
        return result ?? throw new Exception("Error deserializing API response.");
    }

    private static FormUrlEncodedContent? GetFormFromParameters(object? parameters) {
        if (parameters == null) return null;

        // Add property values to form fields
        var formFields = new Dictionary<string, string>();
        foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(parameters)) {
            var value = descriptor.GetValue(parameters);
            if (value != null) formFields.Add(descriptor.Name, value.ToString() ?? string.Empty);
        }

        return new FormUrlEncodedContent(formFields);
    }

}
