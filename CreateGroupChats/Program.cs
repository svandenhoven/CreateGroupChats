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
                            {"user@odata.bind", "https://graph.microsoft.com/v1.0/users('7cb9448e-debf-4595-b09b-502f58a36379')"}
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
                            {"user@odata.bind", "https://graph.microsoft.com/v1.0/users('058f8fd4-c908-4524-bb38-f2d8627d5df3')"}
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
                            {"user@odata.bind", "https://graph.microsoft.com/v1.0/users('eda3b4a5-7672-4588-9ea2-a8d34ae42013')"} //me
						}
                    }
                }
            };
            return chat;
        }
    }
}
