# Elisoft.Notificator

**Elisoft.Notificator** is a central API service acting as a universal notification gateway. It allows for unified message sending to multiple communication channels (such as Microsoft Teams, Slack, Pushover, Twilio) using a single, consistent HTTP entry point.

## Key Features
* **Single API Endpoint** - All notifications are sent to the same address: `POST /api/Notification/send`.
* **Extensibility** - Ability to add new channels (e.g., Mail, Discord, etc.) using appropriate design patterns (`RequestFactory` and `IRequestHandler` implementations).
* **Custom Libraries (NuGet)** - The project relies on dedicated, custom-built libraries for sending logic (`Elisoft.Teams`, `Elisoft.Slack`, `Elisoft.Twilio`, `Elisoft.Pushover`).
* **Authorization** - The API is secured with a key passed in the header. It must be provided as `X-API-KEY`.

## How to use the API?

All POST requests use a JSON object that contains two primary properties:
1. `channel` (String type) - explicitly specifies which communicator the notification is sent to (values are case-insensitive, e.g., "Teams", "slack", "TWILIO").
2. `payload` (Object type) - a dynamic JSON object whose structure changes depending on the selected channel. Below are the payload structures for the supported communicators.

---

## Request Examples (Payload Format)

To test the service locally or simply view its behavior, check out the educational request file – [Elisoft.Notificator.template.http](src/Elisoft.Notificator.Api/Elisoft.Notificator.template.http).

Below are examples of ready-to-use request bodies. Remember that in your HTTP communication headers, you must include:
```http
Content-Type: application/json
X-API-KEY: {Your_API_Key}
```

### 1. Microsoft Teams
The webhook for MS Teams uses an Adaptive Card structure, but the request only requires basic data, and the Notificator will build the correct card in the background.

```json
{
  "channel": "Teams",
  "payload": {
    "webhookUrl": "https://default...", 
    "title": "Notification Title",
    "message": "Content of your message"
  }
}
```

### 2. Slack
Allows you to send a notification to a specific channel using a Webhook.

```json
{
  "channel": "Slack",
  "payload": {
    "webhookUrl": "https://hooks.slack.com/services/...",
    "channelName": "#general",
    "message": "Notification content directed to Slack"
  }
}
```

### 3. Pushover
Used for sending push notifications to mobile devices, among others. The priority parameter is an integer, e.g., `0` (normal), `1` (high).

```json
{
  "channel": "Pushover",
  "payload": {
    "title": "Mobile Notification Title",
    "message": "Push notification content",
    "priority": 0
  }
}
```

### 4. Twilio
Primarily used for sending SMS (or Voice) messages.

```json
{
  "channel": "Twilio",
  "payload": {
    "to": "+48111222333",
    "message": "SMS content that will arrive on the phone"
  }
}
```

## Architecture and Design

The application is built in a classic multi-layered approach (Clean Architecture / CQRS pattern):
* `Elisoft.Notificator.Api` - exposes the HTTP contact point (`NotificationController`). Maps incoming JSON to a specific dedicated class (e.g., `TeamsNotificationRequest`) using a converter class (e.g., `RequestFactory`).
* `Elisoft.Notificator.Core` - implements business logic. Request classes fall into handlers (thanks to the *Paramore.Brighter* library and `RequestHandlerAsync` implementations), and then the handler calls an external dependency which makes the underlying query to the given external API.
* For specific solutions and communication channels, the API has linked, nested repositories (as NuGet or project references, e.g., `Elisoft.Teams`, `Elisoft.Slack`).

To run the application, it is recommended to start the entire solution and perform test requests by inserting your configured environment security key into the `X-API-KEY` header.
