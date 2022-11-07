using Newtonsoft.Json;
using Realtime.Chat.Common.Dto;
using Realtime.Chat.Common.TransportLayer;
using Realtime.Chat.Common.TransportLayer.Commands.Request;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Realtime.Chat.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Guid _clientSessionId = Guid.NewGuid();
        private const string _serverUrl = "http://localhost:5289";
        private readonly Guid _chatId = Guid.Parse("dcada075-07a7-4372-851c-8591aaa440f2");

        public MainWindow(
            IHttpClientFactory httpClientFactory)
        {
            InitializeComponent();
            _httpClientFactory = httpClientFactory;
        }

        private async void SendMessageTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;

            if (string.IsNullOrWhiteSpace(SendMessageTextBox.Text)) return;

            var sendMessageRequest = new SendMessageRequest
            {
                ChatId = _chatId,
                Message = SendMessageTextBox.Text
            };

            var httpClient = _httpClientFactory.CreateClient();

            httpClient.DefaultRequestHeaders.Add(Headers.ClientSessionId, _clientSessionId.ToString());

            try
            {
                await httpClient.PostAsync($"{_serverUrl}/SendMessage", GetStringContent(sendMessageRequest));

                SendMessageTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageWindowListBox.Items.Add($"Error sending message: {ex.Message}");
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ClientSessionLabel.Content = $"Client session id: {_clientSessionId}";

            ServerUrlLabel.Content = $"Server url: {_serverUrl}";

            await SubscribeToChatAsync();

            _ = ReceiveMessagesAsync();
        }

        private async Task ReceiveMessagesAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.DefaultRequestHeaders.Add(Headers.ClientSessionId, _clientSessionId.ToString());

            while (true)
            {
                try
                {
                    var receiveMessagesResponse = await httpClient.GetAsync($"{_serverUrl}/ReceiveMessages");

                    var messages = await GetResultAsync<List<ChatMessageDto>>(receiveMessagesResponse);

                    foreach (var message in messages)
                    {
                        MessageWindowListBox.Items.Add($"{message.SendingTime:T} {message.SenderSessionId.ToString()[..7]}: {message.Message}");

                        if (VisualTreeHelper.GetChild(MessageWindowListBox, 0) is Decorator border)
                        {
                            if (border.Child is ScrollViewer scrollViewer)
                            {
                                scrollViewer.ScrollToVerticalOffset(scrollViewer.ScrollableHeight);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageWindowListBox.Items.Add($"Error receiving message: {ex.Message}");
                }
            }
        }

        private async Task SubscribeToChatAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.DefaultRequestHeaders.Add(Headers.ClientSessionId, _clientSessionId.ToString());

            var subscribeToChatRequest = new SubscribeToChatRequest
            {
                ChatId = _chatId
            };

            try
            {
                var subscribeToChatResponse = await httpClient.PostAsync($"{_serverUrl}/SubscribeToChat", GetStringContent(subscribeToChatRequest));

                ConnectionInfoLabel.Content = subscribeToChatResponse.IsSuccessStatusCode
                    ? $"Successfully joined in. Chat ID: {_chatId}"
                    : $"Failed to join in chat. Chat ID: {_chatId}";
            }
            catch (Exception ex)
            {
                ConnectionInfoLabel.Content = $"Server error: {ex.Message}";
            }
        }

        private static StringContent GetStringContent(object model)
        {
            var json = JsonConvert.SerializeObject(model);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private static async Task<T?> GetResultAsync<T>(HttpResponseMessage response, JsonSerializerSettings? jsonSerializerSettings = default)
        {
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseBody, jsonSerializerSettings);
        }
    }
}