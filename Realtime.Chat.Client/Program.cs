using Newtonsoft.Json;
using Realtime.Chat.Common.TransportLayer;
using Realtime.Chat.Common.TransportLayer.Commands.Request;
using System.Text;

var serverUrl = "http://localhost:5289";

var clientSessionId = Guid.NewGuid();

var chatId = Guid.Parse("dcada075-07a7-4372-851c-8591aaa440f2");

Console.WriteLine($"Client session ID: {clientSessionId}.");

using (var httpClient = new HttpClient())
{
    httpClient.DefaultRequestHeaders.Add(Headers.ClientSessionId, clientSessionId.ToString());

    var subscribeToChatRequest = new SubscribeToChatRequest
    {
        ChatId = chatId
    };

    var subscribeToChatResponse = await httpClient.PostAsync($"{serverUrl}/SubscribeToChat", GetStringContent(subscribeToChatRequest));

    if (subscribeToChatResponse.IsSuccessStatusCode)
    {
        Console.WriteLine($"Successfully connected to the chat: {chatId}");

        _ = ReceiveMessagesAsync(clientSessionId, serverUrl);

        while (true)
        {
            var message = Console.ReadLine();

            ClearCurrentConsoleLine();

            var sendMessageRequest = new SendMessageRequest
            {
                ChatId = chatId,
                Message = message
            };

            await httpClient.PostAsync($"{serverUrl}/SendMessage", GetStringContent(sendMessageRequest));
        }
    }
}

static void ClearCurrentConsoleLine()
{
    Console.SetCursorPosition(0, Console.CursorTop - 1);
    int currentLineCursor = Console.CursorTop;
    Console.SetCursorPosition(0, Console.CursorTop);
    Console.WriteLine(new string(' ', Console.WindowWidth));
    Console.SetCursorPosition(0, currentLineCursor);
}

static async Task ReceiveMessagesAsync(Guid clientSessionId, string serverUrl)
{
    using (var httpClient = new HttpClient())
    {
        httpClient.DefaultRequestHeaders.Add(Headers.ClientSessionId, clientSessionId.ToString());

        while (true)
        {
            var receiveMessagesResponse = await httpClient.GetAsync($"{serverUrl}/ReceiveMessages");

            var messages = await GetResultAsync<List<string>>(receiveMessagesResponse);

            foreach (var message in messages)
            {
                Console.WriteLine(message);
            }
        }
    }
}

static StringContent GetStringContent(object model)
{
    var json = JsonConvert.SerializeObject(model);

    return new StringContent(json, Encoding.UTF8, "application/json");
}

static async Task<T?> GetResultAsync<T>(HttpResponseMessage response, JsonSerializerSettings? jsonSerializerSettings = default)
{
    response.EnsureSuccessStatusCode();

    var responseBody = await response.Content.ReadAsStringAsync();

    return JsonConvert.DeserializeObject<T>(responseBody, jsonSerializerSettings);
}