using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*using Discord;
using UnityEngine;

namespace Assets.Scripts {
    public static class DiscordController {

        static long ID { get; } = 953351630668062720;
        static Discord.Discord Discord { get; set; } = new Discord.Discord(ID, (ulong)CreateFlags.NoRequireDiscord);
        static ActivityManager Manager { get; } = Discord.GetActivityManager();

        public static void UpdateActivity(string State, string Route) {
            if (Discord != null || InitializeDiscord()) {
                Activity me = new Activity() {
                    ApplicationId = ID,
                    Name = $"Pokémon {Engine.Version}",
                    State = State,
                    Type = ActivityType.Playing,
                    Details = Route,
                    Timestamps = { Start = Unix(DateTime.UtcNow), },
                    Assets = {
                        LargeImage = "pkmnicon_v2",
                        LargeText = $"Pokémon {Engine.Version}",
                        SmallImage = Engine.Version == "Pride" ? "arceus" : "giratina",
                    }
                };
                Discord.GetActivityManager().UpdateActivity(me, (result) => {
                    if (result == Result.Ok) {
                        Debug.Log("Discord Activity Updated!");
                    } else {
                        Debug.Log("Discord Activity Update Failed: " + result);
                    }
                });
            }
        }

        static bool InitializeDiscord() {
            try {
                // If discord integration has not been initialized for the game, try to initialize it
                Discord = new Discord.Discord(ID, (ulong)CreateFlags.NoRequireDiscord);

                Debug.Log("Discord Loaded Successfully");

                return true;
            } catch (ResultException ) // To receive information about the exception, uncomment this variable and the following Debug.log()
              {
                // Debug.Log(e.ToString());
                Debug.Log("Discord load failed. User probably has not opened Discord.");
            }

            return false;
        }

        static DiscordController() {
            if (InitializeDiscord()) // If discord initializes properly
                UpdateActivity("Loading...","Loading..."); // Update the discord status
            Application.quitting += OnApplicationQuit;
        }

        public static void OnApplicationQuit() {
            if (Discord != null) {
                // Use discord's built in cleanup function
                Discord.Dispose();

                // Set discord to null to prevent update on the last frame
                Discord = null;

                Debug.Log("Discord Controller Closed");
            }
        }

        public static void Update() {
            if (Discord != null) {
                try {
                    Discord.RunCallbacks();
                } catch (ResultException) {
                    Debug.Log("Discord Update Failed. Gracefully closing the Discord Controller.\nUser may have closed Discord during the game.");
                    Discord.Dispose();
                    Discord = null;
                }
            }
        }

        public static long Unix(DateTime time) {
            DateTime UnixEpoch = new DateTime(1970, 1, 1);
            return (long)time.Subtract(UnixEpoch).TotalSeconds;
        }

        //Logo = Pokeball_Pokemon
        //Small Logo = Version_Version
        //Name = Pokemon {version} Version
        //State = Route
        //Timestamps = now
        //Details = PlayState
    }
}
*/