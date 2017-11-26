using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using DispatcherBot.Models;
using System;
using System.Linq;
using DispatcherBot.Dialogs;
using Newtonsoft.Json;
using System.Text;

namespace DispatcherBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            string conversationID = activity.Conversation.Id;

            StateClient stateClient = activity.GetStateClient();
            BotData conversationData = await stateClient.BotState.GetConversationDataAsync(activity.ChannelId, conversationID);

            // The URL of the currently active bot, either main or one of the sub bots.
            var forwardingUrl = conversationData.GetProperty<string>(Consts.ForwardingUrlKey);

            if (activity.Type == ActivityTypes.Message)
            {
                var message = activity as IMessageActivity;
                if(message != null && !string.IsNullOrEmpty(message.Text))
                {
                    var commandUrl = (from cmd in DataSource.RegisteredBots
                                      where message.Text.Equals(cmd.Key, StringComparison.InvariantCultureIgnoreCase)
                                      select cmd.Value).FirstOrDefault();
                    if(commandUrl != null && !string.IsNullOrEmpty(commandUrl))
                    {
                        forwardingUrl = commandUrl;
                        conversationData.SetProperty<string>(Consts.ForwardingUrlKey, forwardingUrl);
                        await stateClient.BotState.SetConversationDataAsync(activity.ChannelId, conversationID, conversationData);
                    }
                }
//                await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
            }

            if (string.IsNullOrEmpty(forwardingUrl) || Request.RequestUri.ToString().Equals(forwardingUrl,StringComparison.InvariantCultureIgnoreCase))
            {
                if(activity.Type == ActivityTypes.Message)
                {
                    await Conversation.SendAsync(activity, () => new RootDialog());
                }
                else
                {
                    HandleSystemMessage(activity);
                }
                var response = Request.CreateResponse(HttpStatusCode.OK);
                return response;
            }
            else
            {
#if false
                var http = new HttpClient();
                await http.AddAPIAuthorization("3db271c5-3562-49da-b3a3-0a1ff69876a6", "jcWCIBZ7(pxdncNQ6742*;~");
                
                var json = JsonConvert.SerializeObject(activity);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var resp = await http.PostAsync(new Uri(forwardingUrl), content);

                return resp;
#else
                var http = new HttpClient();

                //In order to post message to expert bot, we need to pass Bot Authentication
                //Here, we first check if we have token cached, if no, we will request a new one
                //Since we are here, we must have below BotName
                //var botName = DataSource.RegisteredBots.Where(b => b.Value == forwardingUrl).Select(o => o.Key).SingleOrDefault();
                //var botBearerToken = await BotTokenCache.Get(botName);

                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(forwardingUrl),
                    Method = HttpMethod.Post
                };

                try
                {
                    foreach (var header in Request.Headers)
                    {
                        if (header.Key == "Authorization")
                            continue;
                        request.Headers.Add(header.Key, header.Value);
                    }
                    request.Headers.Host = request.RequestUri.Host;
                    var json = JsonConvert.SerializeObject(activity);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", botBearerToken);
                    request.Content = content;
                    //return await http.PostAsync(request.RequestUri,content);
                    return await http.SendAsync(request);

                }
                catch (Exception exp)
                {
                    throw;
                    //var log = $"[Exception]{exp.Message}";
                    //var inner = exp.InnerException != null ? exp.InnerException.Message : "";
                    //log += $"\n\n{inner}";
                    //activity.Text = log;
                    //await Conversation.SendAsync(activity, () => new RootDialog());
                    //return await http.SendAsync(request);
                }
#endif
            }
        }

        private Activity HandleSystemMessage(Activity message)
        {
            /*
            var connector = new ConnectorClient(new Uri(message.ServiceUrl));
            var reply = message.CreateReply($"{message.Type}:{JsonConvert.SerializeObject(message)}");
            Task.Run(() => connector.Conversations.ReplyToActivityAsync(reply));
            */
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}