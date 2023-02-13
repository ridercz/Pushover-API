using Altairis.Pushover.Client;

// Get arguments from command line
if(args.Length != 3) {
    Console.WriteLine("USAGE: apcdemo <api_token> <user_id> <group_id>");
    Console.WriteLine("You can get these values at Pushover.net");
    return;
}
var myApiToken = args[0];
var myUserId = args[1];
var myGroupId = args[2];

// This is wrapper used to call any client methods, will show what it's doing and when fails, shows list of errors
async Task<T> PerformSafeRequest<T>(string message, Func<Task<T>> operation) where T : PushoverResponse {
    Console.Write(message);
    T r = await operation();
    if (r.Status) {
        Console.WriteLine($"OK, {r.Request}");
    } else {
        Console.WriteLine("Failed!");
        foreach (var error in r.Errors) {
            Console.WriteLine($"  Error: {error}");
        }
    }
    return r;
}

// Create client class
var client = new PushoverClient(myApiToken);

// Get message limits
var limitsResponse = await PerformSafeRequest("Getting message limits...", () => client.GetLimits());
Console.WriteLine($"  Available {limitsResponse.Remaining} of {limitsResponse.Limit} messages, reset at {limitsResponse.Reset}");
Console.WriteLine();

// Get sounds
var soundsResponse = await PerformSafeRequest("Getting sounds...", () => client.GetSounds());
foreach (var item in soundsResponse.Sounds) {
    Console.WriteLine($"  {item.Key} = {item.Value}");
}

// Validate some users and groups
var validateResponse = await PerformSafeRequest("Validating user...", () => client.ValidateUserOrGroup(myUserId));
Console.WriteLine(validateResponse.Group ? "  ID belongs to group" : "  ID belongs to user");
if (validateResponse.Devices.Any()) Console.WriteLine($"  Devices: {string.Join(", ", validateResponse.Devices)}");
if (validateResponse.Licenses.Any()) Console.WriteLine($"  Licenses: {string.Join(", ", validateResponse.Licenses)}");

validateResponse = await PerformSafeRequest("Validating group...", () => client.ValidateUserOrGroup(myGroupId));
Console.WriteLine(validateResponse.Group ? "  ID belongs to group" : "  ID belongs to user");
if (validateResponse.Devices.Any()) Console.WriteLine($"  Devices: {string.Join(", ", validateResponse.Devices)}");
if (validateResponse.Licenses.Any()) Console.WriteLine($"  Licenses: {string.Join(", ", validateResponse.Licenses)}");

validateResponse = await PerformSafeRequest("Validating non-existent user...", () => client.ValidateUserOrGroup("uaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"));
Console.WriteLine();

// Work with groups
await PerformSafeRequest("Renaming group...", () => client.RenameGroup(myGroupId, $"Test group {DateTime.Now}"));
await PerformSafeRequest("Adding user to group...", () => client.AddUserToGroup(myGroupId, myUserId, memo: "Test user"));
await PerformSafeRequest("Disabling user...", () => client.DisableUserInGroup(myGroupId, myUserId));
var groupResponse = await PerformSafeRequest("Getting group info...", () => client.GetGroup(myGroupId));
Console.WriteLine($"  Members of '{groupResponse.Name}' are:");
foreach (var item in groupResponse.Users) {
    Console.Write($"  {item.User} [{item.Device}] {item.Memo}");
    Console.WriteLine(item.Disabled ? " (disabled)" : string.Empty);
    Console.WriteLine(item.Valid ? string.Empty : " (invalid)");
}
await PerformSafeRequest("Enabling user...", () => client.EnableUserInGroup(myGroupId, myUserId));
await PerformSafeRequest("Removing user from group...", () => client.DeleteUserFromGroup(myGroupId, myUserId));
Console.WriteLine();

// Send emergency-priority message
var messageResponse = await PerformSafeRequest("Sending message...", () => client.SendMessage(new PushoverMessage(myUserId, "Test message") {
    Priority = MessagePriority.Emergency(TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(60))
}));

// Check for receipt status
if (messageResponse.Receipt != null) {
    Console.WriteLine($"  Receipt: {messageResponse.Receipt}");

    while (true) {
        var receiptResponse = await PerformSafeRequest("Checking receipt...", () => client.GetMessageReceipt(messageResponse.Receipt));
        if (receiptResponse.LastDeliveredAt.HasValue) Console.WriteLine($"  Last delivered at {receiptResponse.LastDeliveredAt}");
        if (receiptResponse.Expired) {
            Console.WriteLine($"  Expired at {receiptResponse.ExpiresAt}");
        } else {
            Console.WriteLine($"  Will expire at {receiptResponse.ExpiresAt}");
        }
        if (receiptResponse.Acknowledged) Console.WriteLine($"  Acknowledged by user {receiptResponse.AcknowledgedBy} on device {receiptResponse.AcknowledgedByDevice} at {receiptResponse.AcknowledgedAt}");
        if (receiptResponse.CalledBack) Console.WriteLine($"  Called back at {receiptResponse.CalledBackAt}");

        // Exit when message expires or is acknowledged
        if (receiptResponse.Acknowledged || receiptResponse.Expired) break;
        await Task.Delay(5000);
    }
}
Console.WriteLine();