using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace Zad3
{
    public partial class MainPage : ContentPage
    {
        Dictionary<string, List<Message>> _chatMessages = new Dictionary<string, List<Message>>(); // Store messages for each chat

        public MainPage()
        {
            var backgroundImageSource = ImageSource.FromFile("background.jpg");
            BackgroundImageSource = backgroundImageSource;

            var backgroundImage = new Image
            {
                Source = backgroundImageSource,
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                IsPlatformEnabled = true
            };

            var layout = new AbsoluteLayout();
            layout.Children.Add(backgroundImage);

            InitializeComponent();
            LoadChats();

            MessageEntry.BackgroundColor = Color.FromRgba(0, 0, 0, 128);
            ChatsLayout.BackgroundColor = Color.FromRgba(0, 0, 0, 128);
            ChatHeaderFrame.BackgroundColor = Color.FromRgba(13, 46, 18, 128);
        }

        private void LoadChats()
        {
            var chats = new List<ChatItem>()
            {
                new ChatItem { Name = "John", LastMessage = "" },
                new ChatItem { Name = "Lisa", LastMessage = "" },
                new ChatItem { Name = "Kim", LastMessage = "" },
            };

            ChatsView.ItemsSource = chats;
            ChatsView.SelectionChanged += ChatsView_SelectionChanged;

            // Load saved messages for each chat
            foreach (var chat in chats)
            {
                if (_chatMessages.ContainsKey(chat.Name))
                {
                    _chatMessages[chat.Name].ForEach(message =>
                    {
                        message.ChatName = chat.Name;
                        AddMessageToChat(message);
                    });

                    
                }
                else
                {
                    // Add predefined messages for each chat
                    var messages = GetPredefinedMessagesForChat(chat);
                    _chatMessages[chat.Name] = messages;

                    // Display the messages in the chat view
                    foreach (var message in messages)
                    {
                        AddMessageToChat(message);
                    }
                }
            }

            // Select the first chat item by default
            if (chats.Any())
                ChatsView.SelectedItem = chats.First();
        }

        private List<Message> GetPredefinedMessagesForChat(ChatItem chat)
        {
            // Return predefined messages for each chat
            switch (chat.Name)
            {
                case "John":
                    return new List<Message>
                    {
                        new Message { Sender = "John", Content = "Hey, how are you?" },
                        new Message { Sender = "Me", Content = "I'm good, thanks!" },
                        new Message { Sender = "John", Content = "Great!" }
                    };
                case "Lisa":
                    return new List<Message>
                    {
                        new Message { Sender = "Me", Content = "Hi Lisa!" },
                        new Message { Sender = "Lisa", Content = "Hey, what's up?" }
                    };
                case "Kim":
                    return new List<Message>
                    {
                        new Message { Sender = "Me", Content = "Hello Kim." },
                        new Message { Sender = "Kim", Content = "Hi there!" }
                    };

                default:
                    return new List<Message>();
            }
            
        }


        private void ChatsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is ChatItem selectedChat)
            {
                // Update detail view with selected chat
                SelectedChatLabel.Text = $"{selectedChat.Name}";

                // Clear previous messages
                ChatMessagesLayout.Children.Clear();

                // Retrieve messages for the selected chat
                var messages = _chatMessages.ContainsKey(selectedChat.Name) ? _chatMessages[selectedChat.Name] : new List<Message>();

                // Add messages to the chat view
                foreach (var message in messages)
                {
                    AddMessageToChat(message);
                }
            }
        }

        private void SendMessage_Clicked(object sender, EventArgs e)
        {
            var selectedChat = ChatsView.SelectedItem as ChatItem;
            if (selectedChat != null && !string.IsNullOrWhiteSpace(MessageEntry.Text))
            {
                // Create a new message
                var newMessage = new Message
                {
                    Sender = "Me",
                    Content = MessageEntry.Text
                };

                // Add the message to the list of messages for the selected chat
                AddMessageToChat(newMessage);

                // Save the message in the chat
                SaveMessageInChat(selectedChat.Name, newMessage);

                MessageEntry.Text = ""; // Clear the message entry after sending
            }
        }

        private void AddMessageToChat(Message message)
        {
            var messageLabel = new Label
            {
                Text = $"{message.Sender}: {message.Content}",
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                HorizontalOptions = message.Sender == "Me" ? LayoutOptions.End : LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(5),
                BackgroundColor = Color.FromRgba(104, 196, 217, 120),
                Padding = new Thickness(10)
            };
            ChatMessagesLayout.Children.Add(messageLabel);
        }

        private void SaveMessageInChat(string chatName, Message message)
        {
            // Add the message to the list of messages for the specified chat
            if (_chatMessages.ContainsKey(chatName))
            {
                _chatMessages[chatName].Add(message);
            }
            else
            {
                _chatMessages[chatName] = new List<Message> { message };
            }

            
        }

        public class Message
        {
            public string Sender { get; set; }
            public string Content { get; set; }
            public string ChatName { get; set; }
        }

        public class ChatItem
        {
            public string Name { get; set; }
            public string LastMessage { get; set; }
        }

        private void TapGestureRecognizer(object sender, TappedEventArgs e)
        {
            var selectedChat = ChatsView.SelectedItem as ChatItem;
            if (selectedChat != null)
            {
                Navigation.PushAsync(new UserPage(selectedChat.Name));
            }
        }
    }
}