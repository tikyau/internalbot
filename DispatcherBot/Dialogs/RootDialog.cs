using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using DispatcherBot.Models;
using System.Linq;
using Newtonsoft.Json;

namespace DispatcherBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            // If the message matches a navigation command, take the correct action (post something to the conversation, call a dialog to change the conversation flow, etc.
            if (message.Text.ToLowerInvariant() == Models.Consts.MainBotKey.ToLowerInvariant())
            {
                await this.ShowNavMenuAsync(context);
            }
            else
            {
                // Else something other than a navigation command was sent, and this dialog only supports navigation commands, so explain the bot doesn't understand the command.
                await this.StartOverAsync(context, string.Format("I don't understand", message.Text));
            }
        }
        private async Task ShowNavMenuAsync(IDialogContext context)
        {
            var reply = context.MakeMessage();

            var menuHeroCard = new HeroCard
            {
                Text = $"Hi, {context.Activity.From.Name} ({context.Activity.From.Id})::{JsonConvert.SerializeObject(context.Activity)}",
                Buttons = DataSource.RegisteredBots.Where(r => r.Key != Consts.MainBotKey).Select(b => new CardAction(ActionTypes.ImBack, b.Key, value: b.Key)).ToList<CardAction>()
            };
            
            reply.Attachments.Add(menuHeroCard.ToAttachment());
            
            await context.PostAsync(reply);

            context.Wait(this.ShowNavMenuResumeAfterAsync);
        }
        private async Task ShowNavMenuResumeAfterAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            // If we got here, it's because something other than a navigation command was sent to the bot (navigation commands are handled in NavigationScorable middleware), 
            //  and this dialog only supports navigation commands, so explain bot doesn't understand the message.
            await this.StartOverAsync(context, string.Format("I don't understand {0}", message.Text));
        }
        private async Task StartOverAsync(IDialogContext context, string text)
        {
            var message = context.MakeMessage();
            message.Text = text;
            await this.StartOverAsync(context, message);
        }
        private async Task StartOverAsync(IDialogContext context, IMessageActivity message)
        {
            await context.PostAsync(message);
            await this.ShowNavMenuAsync(context);
        }
    }
}