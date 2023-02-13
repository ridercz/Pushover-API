[![NuGet Status](https://img.shields.io/nuget/v/Altairis.Pushover.Client.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/Altairis.Pushover.Client/)

# Pushover API Client

[Pushover](https://pushover.net/) is simple notification service and mobile device application. For sending notifications (messages) if offers a [web API](https://pushover.net/api). 

The API is quite weird combination of REST-like approach and old-school HTTP POST form emulation and I was not able to find any .NET client that would seem to be maintained and actually working.

So, I wrote one, with fairly modern approach.

## Getting started

```csharp
// API token obtained from app registration
var myApiToken = "axxxxxxxxxxxxxxxxxxxxxxxxxxxxx";

// User ID obtained from recipient user registration
var myUserId = "uxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";

// Create client class
var client = new PushoverClient(myApiToken);

// Create a message
var message = new PushoverMessage(myUserId, "This is a test message") {
    Title = "Test message"
};

// Send message
var result = await client.SendMessage(message);

// Check for success
if (result.Status) {
    Console.WriteLine("Message sent successfully!");
} else {
    Console.WriteLine("Message send failed!");
}
```

## Documentation

API reference documentation can be found in [wiki](https://github.com/ridercz/Pushover-API/wiki/).

## Legal

* Developed by [Michal Altair Valášek](https://www.rider.cz/).
* Licensed under terms of the MIT License.
* This project adheres to [No Code of Conduct](CODE_OF_CONDUCT.md). We are all adults. We accept anyone's contributions. Nothing else matters.