using CricketService.Domain.RequestDomains;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace CricketService.Utils
{
    public class TelegramBot
    {
        public static async Task Main(string[] args)
        {
            Dictionary<ChatId, TelegramBotClient> channels = new();

            channels.Add(new ChatId("@odimatchespdf"), new TelegramBotClient("6265798559:AAG-e1xG0Jy5ajjKqKoT9FpwXKkVOSKGoaw"));
            channels.Add(new ChatId("@testmatchesscoreboard"), new TelegramBotClient("6259090677:AAFV0-Am0qOrD6efF_MK4WqGwmTtqaTRjRg"));

            var filePaths = Directory.GetFiles("D:\\CricketData\\ODIMatches");

            foreach (var filePath in filePaths.OrderBy(x => Convert.ToInt32(x.Split("_").ElementAt(2))))
            {
                if (filePath.EndsWith(".pdf"))
                {
                    await SendImagesInDocumentCaptionAsync(channels.ElementAt(0), filePath);
                }
            }
        }

        private static async Task SendImagesInDocumentCaptionAsync(KeyValuePair<ChatId, TelegramBotClient> channel, string filePath)
        {
            StreamReader r = new StreamReader(filePath.Replace(".pdf",".json"));

            var matchData = JsonConvert.DeserializeObject<TestCricketMatchRequest>(r.ReadToEnd());

            string matchNumber = $"ODI NUMBER <b>{Convert.ToInt32(matchData.MatchNumber.Replace("ODI no. ", string.Empty))}</b>\n\n";
            string matchDetails = $"<b>{matchData.Team1.Team.Name.ToUpper()} vs {matchData.Team2.Team.Name.ToUpper()}</b>\n{matchData.Series}-{matchData.Season}" +
                $"\n\n {matchData.MatchDate}\n{matchData.Venue}";

            string captionWithPhotos = matchNumber + matchDetails;

            Stream stream = System.IO.File.OpenRead(filePath);

            Console.WriteLine($"Started sending {matchData.MatchNumber}");

            Message message = await channel.Value.SendDocumentAsync(
                                                         chatId: channel.Key,
                                                         document: new InputOnlineFile(content: stream, fileName: $"{matchData.MatchNumber}_{matchData.MatchTitle}.pdf"),
                                                         thumb: null,
                                                         caption: captionWithPhotos,
                                                         ParseMode.Html);

            Thread.Sleep(3000);

            Console.WriteLine($"Successfully send {matchData.MatchNumber}");
        }
    }
}
