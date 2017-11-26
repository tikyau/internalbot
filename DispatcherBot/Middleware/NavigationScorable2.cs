using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Scorables.Internals;
using Microsoft.Bot.Connector;
using DispatcherBot.Models;
using DispatcherBot.Dialogs;

namespace DispatcherBot.Middleware
{
    public class NavigationScorable2 : ScorableBase<Activity, string, double>
    {
        private IDialogTask task;
        private List<string> navigationCommands;

        public NavigationScorable2(IDialogTask task)
        {
            SetField.NotNull(out this.task, nameof(task), task);

            this.navigationCommands = new List<string>();

            this.navigationCommands.Add(Consts.MainBotKey);
        }
        protected override Task DoneAsync(Activity item, string state, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        protected override double GetScore(Activity item, string state)
        {
            return 1;
        }

        protected override bool HasScore(Activity item, string state)
        {
            return !string.IsNullOrEmpty(state);
        }

        protected async override Task PostAsync(Activity item, string state, CancellationToken token)
        {
            var message = item as IMessageActivity;
            
            if(message != null)
            {
                var root = new RootDialog();
                
                await task.PollAsync(CancellationToken.None);
            }
        }

        protected async override Task<string> PrepareAsync(Activity item, CancellationToken token)
        {
            var message = item as IMessageActivity;
            if (message != null && !string.IsNullOrEmpty(message.Text))
            {
                var command = (from cmd in navigationCommands
                               where message.Text.Equals(cmd, StringComparison.InvariantCultureIgnoreCase)
                               select cmd).FirstOrDefault();

                return message.Text;
            }
            else
            {
                return null;
            }
        }
    }
}