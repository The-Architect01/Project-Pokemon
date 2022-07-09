using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;

public static class Monica {

    public static string AccountName { get; private set; }
    public static string PersonaName { get; private set; }
    public static string[] Friends { get; private set; }
    public static string[] Games { get; private set; }

    public static Status Jevil { get; private set; }
    public static Status SpamtonNeo { get; private set; }
    public static Status Flowey { get; private set; }
    public static Status Sans { get; private set; }
    public static Status Chara { get; private set; }

    static string FilePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFilesX86);
    static string SavePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);

    static Monica() {
        if (File.Exists(FilePath + @"\Steam\config\loginusers.vdf")) { 
            string Document = File.ReadAllText(FilePath + @"\Steam\config\loginusers.vdf");
            string[] Lines = Document.Split('\n');
            foreach(string line in Lines) {
                if (line.Contains("\"AccountName\"")) {
                    AccountName = line.Split('\"')[3].Trim();
                }else if (line.Contains("\"PersonaName\"")) {
                    PersonaName = line.Split('\"')[3].Trim();
                }
            }
            if(Directory.Exists(FilePath + @"\Steam\userdata")) {
                GetFriends();
            } else {
                Friends = new string[] { "None" };
            }
        } else {
            Debug.Log("File Does not Exist");
         //   AccountName = Player.Name;
           // PersonaName = "Not Found";
        }
    }
    static void GetFriends() {
        string LastDirectory = "";
        System.DateTime LastEdit = new System.DateTime();
        foreach (string Directory in Directory.GetDirectories(FilePath + @"\Steam\userdata")) {
            if (File.Exists(Directory + @"\config\localconfig.vdf")) {
                System.DateTime FileEdit = File.GetLastWriteTime(Directory + @"\config\localconfig.vdf");
                LastEdit = LastEdit < FileEdit ? FileEdit : LastEdit;
                if (LastEdit == FileEdit) {
                    LastDirectory = Directory + @"\config\localconfig.vdf";
                    GetGames(Directory + @"\config\shortcuts.vdf");
                }
            } else {
                Debug.Log($"Can't find {Directory + @"\config\localconfig.vdf"}");
            }
        }

        if (!string.IsNullOrEmpty(LastDirectory) && File.Exists(LastDirectory)) {
            string Document = File.ReadAllText(LastDirectory);
            string[] Lines = Document.Split('\n');
            List<string> friends = new List<string>();
            foreach (string line in Lines) {
                if (line.Contains("\"0\"")) {
                    try {
                        if (!string.IsNullOrEmpty(line.Split('\"')[3])) {
                            string friend = line.Split('\"')[3];
                            try {
                                int.Parse(friend[0].ToString());
                            } catch {
                                friends.Add(friend);
                            }
                        }
                    } catch { }
                }
            }
            friends.Remove(PersonaName);
            Friends = friends.ToArray();
        }
    }
    static void GetGames(string FileName) {
        List<string> GameNames = new List<string>();
        foreach (string Directory in Directory.GetFileSystemEntries(FilePath + @"\Steam\steamapps")) {
            if (Directory.Contains("appmanifest")) {
                string GameData = File.ReadAllLines(Directory)[5];
                if (!GameData.Split('\"')[3].Contains("Steamworks")) {
                    GameNames.Add(GameData.Split('\"')[3]);
                }
            }
        }

        if (File.Exists(FileName)) {
            string GameData = File.ReadAllText(FileName);
            string[] Games = GameData.Split('\0');
            for(int i = 0; i < Games.Length; i++) {
                try {
                    if (Games[i - 1].Contains("AppName"))
                        GameNames.Add(Games[i]);
                } catch { }
            }
        } else {
            Debug.Log("No File");
        }
        Games = GameNames.ToArray();
    }

    static void SetJevil() {

        if (File.Exists(SavePath + @"\DELTARUNE\filech1_0")) {
            string Document = File.ReadAllText(SavePath + @"\DELTARUNE\filech1_0");
            Jevil = (Status)int.Parse(Document.Split('\n').Skip(557).ToArray()[0]);
        } else {
            Jevil = Status.NotSeen;
        }
    }
    static void SetFlowey() {
        if (File.Exists(SavePath + @"\UNDERTALE\file8")) {
            if(File.Exists(SavePath + @"\UNDERTALE\file0")) {
                string Document = File.ReadAllText(SavePath + @"\UNDERTALE\file0");
                Flowey = int.Parse(Document.Split('\n').Skip(475).ToArray()[0]) == 0 ? Status.Seen : Status.Killed;
            }else if(File.Exists(SavePath + @"\UNDERTALE\file9")) {
                Flowey = Status.Spared; //Flowey attacks user
            }
        } else if (File.Exists(SavePath + @"\UNDERTALE\undertale.ini")) {
            string[] Document = File.ReadAllText(SavePath + @"\UNDERTALE\undertale.ini").Split('\n');
            for(int i = 0; i < Document.Length; i++) {
                if (Document[i] == "[Flowey]") {
                    if (Document[i + 1].Contains("Met1=\"1")) {
                        Flowey = Status.Known;
                        break;
                    }
                }
            }
        } else { 
            Flowey = Status.NotSeen;
        }
    }
    static void SetSpamtonNeo() {
        if (File.Exists(SavePath + @"\DELTARUNE\filech2_0")) {
            string Document = File.ReadAllText(SavePath + @"\DELTARUNE\filech2_0");
            int Snowgrave = int.Parse(Document.Split('\n').Skip(1467).ToArray()[0]);
            if (Snowgrave != 0) {
                if(Snowgrave <= 3) {
                    SpamtonNeo = Status.HasFreezeRing;
                    return;
                }else if(Snowgrave >= 6) {
                    SpamtonNeo = Status.Snowgrave;
                    return;
                }
            } else {
                int Flag = int.Parse(Document.Split('\n').Skip(861).ToArray()[0]);
                switch (Flag) {
                    case 4:
                        SpamtonNeo = Status.Known;
                        break;
                    case 7: case 8: case 10:
                        SpamtonNeo = Status.Seen;
                        break;
                    case 9:
                        SpamtonNeo = Status.Defeated;
                        break;
                }
            }
        } else {
            SpamtonNeo = Status.NotSeen;
        }
    }
    static void SetGenocide() {
        //system_information_962 - Erase World
        //system_information_963 - Sold Soul
        Sans = (Status)(File.Exists(SavePath + @"\UNDERTALE\system_information_962") ? 0 : 6);
        Chara = (Status)(File.Exists(SavePath + @"\UNDERTALE\system_information_963") ? 0 : 5);
    }
}
public enum Status {
    NotSeen = 0,
    Known = 1,
    Seen = 5,
    Defeated = 9,
    Snowgrave = 12,
    HasFreezeRing = 3,
    Killed = 6,
    Spared = 7,
}
#region Bonus Routes
/*
 * Special ACTION - Triggered When After 2nd faint
 * pokemon
 * Throws a blue screen. UI Becomes Corrupted (All Text
 * Becomes Unown/Glitch Text). Does 120 Damage 100% Accuracy
 * Battle Background becomes Blue Screen Error
 * 
 * Jevil: QR Code - Blacephalon; Code: JOKER_CHAOS_OVERLOAD
 * Spamton: QR Code - Xurkitree; Code: SNEO_DEAL_DENIED
 * Flowey: QR Code - Guzzlord; Code: FIREWALL_BREACH_DETECTED
 */
/* ---------------------------------------------------
* Jevil
* ----------------------------------------------------
* PKMN1: Mr. Rime
* PKMN2: Blacephalon
* PKMN3: Klefki
* PKMN4: Malamar
* PKMN5: Chadelure
* PKMN6: Yvetal
* Dialogue
* Intro:
*       UEE HEE HEE! VISITORS, VISITORS! NOW WE CAN PLAY, PLAY! THEN,
*       AFTER YOU, I CAN PLAY WITH EVERYONE ELSE, TOO! THOSE PREVIOUS
*       TRAINERS - THE ONES YOU WERE SUPPOSED TO FACE - WERE TOO EASY.
*       NOW, NOW!! LET THE GAMES BEGIN!! <LAUGH>
* After First Faint:
*       HA HA HA. WHAT FUN!!! YOU'RE FAST, FAST, STRONG, STRONG. BUT THERE ARE 
*       YET FASTER, YET STRONGER. 
* On Player Faint:
*       NU-HA!! I NEVER HAD SUCH FUN, FUN!!
* <I Can do Anything>
* On Special Attack:
*       THIS IS IT, BOISENGIRLS! SEE YA!
* <Chaos Chaos>
* On Last Pokemon - Spared:
*        A BEAUTY IS JOYING IN MY HEART! EVEN DEVILSKNIFE IS SMILING!
*        IT'S ALL TOO MUCH FUN!!!
* <Bye Bye>
* On Last Pokemon - Killed:
*       IT'S ALL TOO MUCH FUN!!!
*       THIS BODY CANNOT BE KILLED!
* <Bye Bye>       
* On Last Pokemon - Default:
*       PLEASE, IT'S JUST A SIMPLE CHAOS, CHAOS.
*       WHO KEEPS SPINNING THE WORLD AROUND?
* <Bye Bye>
* On Defeat:
*       -Spared/Killed
*           HA HA HA. WHAT FUN!!! YOU'RE FAST, FAST, STRONG, STRONG. THANKS
*           FOR PLAYING WITH ME AGAIN! UEE HEE HEE! A MISCHIEF-MISCHIEF, A 
*           CHAOS-CHAOS...! FROM INSIDE YOUR LITTLE CELL!!
*       -else
*           HA HA HA. WHAT FUN!! I'M EXHAUSTED!! THANKS FOR PLAYING WITH ME!
*           UNTIL NEXT TIME, A MISCHIEF-MISCHIEF, A CHAOS-CHAOS! UEE HEE HEE!
*           ENJOY YOUR LITTLE CELL!
*/
/* ---------------------------------------------------
* Spamton
* ----------------------------------------------------
* PKMN1: Rotom
* PKMN2: Xurkitree
* PKMN3: Hypno
* PKMN4: Porygon-Z
* PKMN5: Zoroark
* PKMN6: Hoopa (unbound)
* Dialogue
* Intro: HEY    EVERY   !! IT'S ME!!!
*        EV3RY BUDDY    's FAVORITE [[Number 1 Rated Salesman1997]]
*        WOAH!! IF IT ISN"T <NAME>!!! YOU [Little Sponge]! I KNEW YOU'D COME
         HERE [[On A <DAY of Week> <time of Day>]]!
*        HAVE YOU COME TO VISIT ME NOW THAT I AM A [Big Shot]!
*        AFTER ALL, YOU WANNA BE A [Big Shot] too! [[Unintelligble Laughter]] 
*        <LAUGH>
* After First Faint:
*        <NAME>!?!? WAS THAT A [BIGH SHOT] JUST NOW!?
*        WOW!!! I'M SO [Proud] OF YOU, I COULD [Killed] YOU!
*        [Heaven], are you WATCHING?
* On Player Faint:
*        YOU WON'T FIND HIGHER LEVEL AND DAMAGE ANYWHERE ELSE!!! THE SMOOTH
*        TASTE OF NEO "WAKE UP AND TASTE THE PAIN". GO AHEAD AND [Scream]
*        INTO THE [Reciever]. NO ONE WILL EVER PICK UP!
* On Special Attack:
*        LET'S turn those [Cathode Screens] INTO [Cathode Screams]
*        <LAUGH>
* On Last Pokemon - Snowgrave:
*        MY ESTEEM CUSTOMER I SEE YOU ARE ATTEMTING [Frozen Chicken] ON ME.
*        BUT, YOUR [Side Chick] CAN'T HELP YOU NOW! DON'T BLAME ME WHEN
*        YOU'RE [Crying] IN A [Broken Home] WISHING YOU LET YOUR OLD PAL
*        SPAMTON [Kill You]! NOW...ENJOY THE FIR3WORKS, KID!!!
* On Last Pokemon - Has Ring:
*        WAIT!! [$!?!] THE PRESSES! I SEE YOU ARE ATTEMPTING [Frozen Addison]
*        ON ME. TO [$!$!] ME OVER RIGHT AT THE [Good part]!? WHAT ARE YOU, A 
*        [Gameshow Host]!? ARE YOU READY FOR MY [Next Trick]! I WILL BECOME 
*        SPAMTON [EX]! NOW...ENJOY THE FIR3WORKS, KID!!!
* On Last Pokemon - Default:
*        MY ESTEEM CUSTOMER I SEE YOU ARE ATTEMPTING TO DEPLETE MY HOOPA
*        [Unbound]! I'LL ADMIT YOU'VE GOT SOME [Guts] KID! BUT IN A [1 for 1] 
*        BATTLE, NEO NEVER LOSES!!! IT'S TIME FOR A LITTLE [Bluelight Specil].
*        NOW...ENJOY THE FIR3WORKS, KID!!!
* ON DEFEAT:
*        HAHAHA...<NAME>!!! YOU THINK DEPLETING MY [Party] MEANS YOU'VE WON
*        [A Free Meal] TO [Winning]?! AFTER ALL THE [Unforgettable D3als] 
*        [Free KROMER] I GAVE YOU - AFTER EVERYTHING I DID TO YOU...!?
*        IT'S TIME FOR YOU TO BE A [Big Shot]!! ARE YOU GETTING ALL THIS 
*        [Mike]!?
*/
/* ---------------------------------------------------
* Flowey
* ----------------------------------------------------
* PKMN1: Victreebel
* PKMN2: Guzzlord
* PKMN3: Floette (Eterna Forme)
* PKMN4: Aegislash (Gen 6/7 stats)
* PKMN5: Spiritomb
* PKMN6: Zygarde
* Dialogue
* Intro: Did you miss me ;P
*        I noticed that you stopped playing with me, and I got ever so bored.
*        But I finally found you! And now we can play forever...
*        Did you really think that you could get away from me?!
*        <Laugh>
*        //Did you miss me ;P It took me a while, but I finally made it through the last firewall!
*        //I saw this program and thought that it would be a fun one to mess with. 
* 
*/
#endregion