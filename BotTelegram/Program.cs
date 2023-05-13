using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static System.Net.Mime.MediaTypeNames;

namespace BotTelegram
{
    internal class Program
    {
        static TelegramBotClient Bot = new TelegramBotClient("5815906352:AAHfk_sSBzahLVT5_rlfJwAXXy_TgCTMJTU");
        //public struct BotUpdate 
        //{
        //    public long Id;
        //    public string Text;
        //    public string? Username;
        //}

        //static string filename = "messages.json";
        //static List<BotUpdate> BotUpdates = new List<BotUpdate>();

        static List<string> questions = new List<string>()
        {
            "2+2",
            "3+2",
            "4+3",
            "10+5"
        };

        static List<string> answers = new List<string>()
        {
            "4",
            "5",
            "7",
            "15"
        };
        static void Main(string[] args)
        {

            #region Version 1
            //try
            //{
            //    var botUpdateStrings =  System.IO.File.ReadAllText(filename);
            //    BotUpdates = JsonConvert.DeserializeObject<List<BotUpdate>>(botUpdateStrings);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            //var receiverOptions = new ReceiverOptions
            //{
            //    AllowedUpdates = new UpdateType[]
            //    {
            //        UpdateType.Message,
            //        UpdateType.EditedMessage
            //    }
            //};
            //Bot.StartReceiving(UpdateHandler,ErrorHandler,receiverOptions);
            #endregion

            StartReceiver();
            Console.ReadLine();
        }

        #region Version 1
        //private static Task ErrorHandler(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        //{
        //    throw new NotImplementedException();
        //}

        //private static async Task UpdateHandler(ITelegramBotClient boot, Update update, CancellationToken token)
        //{
        //    if(update.Type == UpdateType.Message)
        //    {
        //        if(update.Message.Type == MessageType.Text)
        //        {
        //            BotUpdate botUpdate = new BotUpdate
        //            {
        //                Id = update.Message.Chat.Id,
        //                Text = update.Message.Text,
        //                Username = update.Message.Chat.Username,
        //            };
        //            BotUpdates.Add(botUpdate);

        //            string? botUpdateMessage = JsonConvert.SerializeObject(botUpdate);
        //            System.IO.File.WriteAllText(filename, botUpdateMessage);
        //        }
        //    }
        //}
        #endregion

        public static async Task StartReceiver()
        {
            var cancellationToken = new CancellationTokenSource();
            var token = cancellationToken.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };

            await Bot.ReceiveAsync(OnMessage, ErrorMessage, receiverOptions);
        }

        static async Task OnMessage(ITelegramBotClient botClient,Update update,CancellationToken token)
        {
            if(update.Message is Message message)
            {
                string? text = message.Text;
                if (questions.Contains(text))
                {
                    int index = questions.IndexOf(text);
                    await botClient.SendTextMessageAsync(update.Message.Chat.Id, answers[index]);
                }
                else
                {
                    await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Sorry, i don't know this question");
                }
            }
        }

        static async Task ErrorMessage(ITelegramBotClient botClient,Exception e,CancellationToken token)
        {
            if(e is ApiRequestException requestException)
            {
                await botClient.SendTextMessageAsync("", e.Message);
            }
        }

    }
}