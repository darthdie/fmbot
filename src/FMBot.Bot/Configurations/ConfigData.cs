using System;
using System.IO;
using System.Threading;
using FMBot.Bot.Models;
using Newtonsoft.Json;

namespace FMBot.Bot.Configurations
{
    public static class ConfigData
    {
        private const string ConfigFolder = "configs";
        private const string ConfigFile = "config.json";

        public static ConfigModel Data { get; }

        /// <summary>
        /// Loads all the <see cref="ConfigData"/> needed to start the bot.
        /// </summary>
        static ConfigData()
        {
            if (!Directory.Exists(ConfigFolder))
            {
                Directory.CreateDirectory(ConfigFolder);
            }

            if (!File.Exists(ConfigFolder + "/" + ConfigFile))
            {
                // Default config template
                Data = new ConfigModel
                {
                    Database = new DatabaseConfig
                    {
                        ConnectionString = "Host=localhost;Port=5433;Username=postgres;Password=password;Database=fmbot;Command Timeout=15;Timeout=30;Persist Security Info=True"
                    },
                    Bot = new BotConfig
                    {
                        Prefix = ".fm",
                        BotWarmupTimeInSeconds = 30,
                        FeaturedTimerStartupDelayInSeconds = 20,
                        FeaturedTimerRepeatInMinutes = 60,
                    }, 
                    BotLists = new BotListConfig(),
                    Discord = new DiscordConfig(),
                    LastFm = new LastFmConfig
                    {
                        UserUpdateFrequencyInHours = 24,
                        UserIndexFrequencyInDays = 120
                    },
                    Genius = new GeniusConfig(),
                    Spotify = new SpotifyConfig(),
                    Google = new GoogleConfig()
                };

                var json = JsonConvert.SerializeObject(Data, Formatting.Indented);
                File.WriteAllText(ConfigFolder + "/" + ConfigFile, json);

                Console.WriteLine("Created new bot configuration file with default values. \n" +
                                  $"Please set your API keys in {ConfigFolder}/{ConfigFile} before running the bot again. \n \n" +
                                  "Exiting in 10 seconds...", 
                    ConsoleColor.Red);

                Thread.Sleep(10000);
                Environment.Exit(0);
            }
            else
            {
                var json = File.ReadAllText(ConfigFolder + "/" + ConfigFile);
                Data = JsonConvert.DeserializeObject<ConfigModel>(json);
            }
        }
    }
}
