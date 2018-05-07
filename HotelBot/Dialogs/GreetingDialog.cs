using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace HotelBot.Dialogs
{
    [Serializable]
    public class GreetingDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync($"Hi I'm Log Bot");
            await Respond(context);
            context.Wait(MessageReceivedAsync);

            //return Task.CompletedTask;
        }

        private static async Task Respond(IDialogContext context)
        {
            var username = String.Empty;
            context.UserData.TryGetValue<string>("Name", out username);
            if (string.IsNullOrEmpty(username))
            {
                await context.PostAsync("What is your name?");
                context.UserData.SetValue<bool>("GetName", true);
            }
            else
            {
                await context.PostAsync($"Hi {username}. How can I help you today?");
            }
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result as IMessageActivity;

            // TODO: Put logic for handling user message here
            var username = String.Empty;
            var getName = false;
            context.UserData.TryGetValue<string>("Name", out username);
            context.UserData.TryGetValue<bool>("GetName", out getName);


            if (getName)
            {
                username = message.Text;
                context.UserData.SetValue<string>("Name", username);
                context.UserData.SetValue<bool>("GetName", false);
            }

            await Respond(context);

            context.Done(message);
        }
    }
}