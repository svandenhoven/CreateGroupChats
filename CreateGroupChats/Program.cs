using System;
using System.Collections.Generic;
using Azure.Identity;
using Microsoft.Graph;


namespace CreateGroupChats
{
    internal class Program
    {

        static void Main(string[] args)
        {
            var topic1 = args[0];
            var topic2 = args[1];

            var scopes = new[] { "https://graph.microsoft.com/.default" };
            var tenantId = "TBD";
            var clientId = "TDB";
            var clientSecret = "TDB";

            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };

            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret, options);

            var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

            Chat chat1 = CreateChat(topic1);
            Chat chat2 = CreateChat(topic2);

            var result = graphClient.Chats.Request().AddAsync(chat1).Result;
            Console.WriteLine($"Chat 1 {result.Id}");


            result = graphClient.Chats.Request().AddAsync(chat2).Result;
            Console.WriteLine($"Chat 2 {result.Id}");

        }

        private static Chat CreateChat(string topic)
        {
            var chat = new Chat
            {
                ChatType = ChatType.Group,
                Topic = topic,
                Members = new ChatMembersCollectionPage()
                {
                    new AadUserConversationMember
                    {
                        Roles = new List<String>()
                        {
                            "owner"
                        },
                        AdditionalData = new Dictionary<string, object>()
                        {
                            {"user@odata.bind", "https://graph.microsoft.com/v1.0/users('<<user1>>')"}
                        }
                    },
                    new AadUserConversationMember
                    {
                        Roles = new List<String>()
                        {
                            "owner"
                        },
                        AdditionalData = new Dictionary<string, object>()
                        {
                            {"user@odata.bind", "https://graph.microsoft.com/v1.0/users('<<user2>>')"}
                        }
                    },
                    new AadUserConversationMember
                    {
                        Roles = new List<String>()
                        {
                            "owner"
                        },
                        AdditionalData = new Dictionary<string, object>()
                        {
                            {"user@odata.bind", "https://graph.microsoft.com/v1.0/users('<<user3>>')"} //me
						}
                    }
                }
            };
            return chat;
        }
    }
}
