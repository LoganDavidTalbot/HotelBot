using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HotelBot.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;

namespace HotelBot.Dialogs
{
    //[Serializable]
    public class HotelBotDialog// : IDialog<object>
    {
        public static readonly IDialog<string> dialog = Chain.PostToChain()
            .Select(msg => msg.Text)
            .Switch(
                new RegexCase<IDialog<string>>(new Regex("^hi", RegexOptions.IgnoreCase), (context, text) =>
                {
                    return Chain.ContinueWith(new GreetingDialog(), AfterGreetingContinuation);
                }),
                new DefaultCase<string, IDialog<string>>((context, text) =>
                {
                    return Chain.ContinueWith(FormDialog.FromForm(RoomReservation.BuildForm, FormOptions.PromptInStart), AfterGreetingContinuation);
                })
            )
            .Unwrap()
            .PostToUser();

        private async static Task<IDialog<string>> AfterGreetingContinuation(IBotContext context, IAwaitable<object> item)
        {
            var token = await item;
            var name = "User";
            context.UserData.TryGetValue<string>("Name", out name);
            return Chain.Return($"Thank you for using the hotel bot: {name}");
        }

        //public Task StartAsync(IDialogContext context)
        //{
        //    context.Wait(MessageReceivedAsync);

        //    return Task.CompletedTask;
        //}

        //private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        //{
        //    var activity = await result as IMessageActivity;

        //    // TODO: Put logic for handling user message here

        //    context.Wait(MessageReceivedAsync);
        //}
    }
}