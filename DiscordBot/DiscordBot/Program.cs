using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace DiscordBot
{
    public class Program
    {
        public string Token => File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Text.txt");
        public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            DiscordSocketClient _client = new DiscordSocketClient();
            _client.Log += Log;

            await _client.LoginAsync(TokenType.Bot, Token);
            await _client.StartAsync();
            await _client.SetGameAsync("We make project yis");

            _client.MessageReceived += MessageReceived;

            await Task.Delay(-1);
        }

        private Task Log(LogMessage logMsg)
        {
            Console.WriteLine(logMsg.ToString());
            return Task.CompletedTask;
        }

        private async Task MessageReceived(SocketMessage msg)
        {
            if (msg.Content.Contains("?jakob"))
            {
                EmbedBuilder build = BuildJakob();
                await msg.Channel.SendMessageAsync("", false, build);
            }
            else if (msg.Content == "?r")
            {
                EmbedBuilder build = Left();
                await msg.Channel.SendMessageAsync("", false, build);
            }
            else if (msg.Content.Contains("?v"))
            {
                EmbedBuilder build = Random();
                await msg.Channel.SendMessageAsync("", false, build);
            }
        }

        #region commands
        private EmbedBuilder BuildJakob()
        {
            EmbedBuilder build = new EmbedBuilder();
            Random random = new Random();
            string answer = "";
            int i = random.Next(1, 100);

            if (i <= 48)
            {
                answer = "Ja";
                build.WithThumbnailUrl("http://www.hvadsigerjakob.dk/jakob_yes.jpg");
            }
            else if (i > 96)
            {
                answer = "Ved ikke";
                build.WithThumbnailUrl("http://www.hvadsigerjakob.dk/jakob_does_not_know.jpg");
            }
            else
            {
                answer = "Nej";
                build.WithThumbnailUrl("http://www.hvadsigerjakob.dk/jakob_no.jpg");
            }

            build.WithTitle("Hvadsigerjakob.dk v2")
                    .WithDescription("Jakob siger: " + answer)
                    .WithColor(Color.DarkBlue);

            return build;
        }

        private EmbedBuilder Random()
        {
            EmbedBuilder build = new EmbedBuilder();
            Random random = new Random();
            string person = "";
            int i = random.Next(1, 5);
            switch (i)
            {
                case 1: person = "Leiki"; break;
                case 2: person = "Johannes"; break;
                case 3: person = "Mathias"; break;
                case 4: person = "Lasse"; break;
            }
            build.WithTitle("A volunteer has appeared!")
                .WithDescription("You turn, " + person)
                .WithColor(Color.DarkMagenta);

            return build;
        }

        private EmbedBuilder Left()
        {
            EmbedBuilder build = new EmbedBuilder();

            DateTime turnInTime = new DateTime(2018, 6, 20, 8, 30, 0);
            TimeSpan timeRemaining = turnInTime.Subtract(DateTime.Now);
            string timeRemainingString = timeRemaining.Days + " days, " + timeRemaining.Hours + " hours, " + timeRemaining.Minutes + " minutes";
            if (timeRemaining.Days == 0)
            {
                timeRemainingString = timeRemaining.Hours + " hours, " + timeRemaining.Minutes + " minutes";
            }

            build.WithTitle("We got da exam in")
                .WithDescription(timeRemainingString)
                .WithColor(Color.Purple);

            return build;
        }
        #endregion
    }
}
