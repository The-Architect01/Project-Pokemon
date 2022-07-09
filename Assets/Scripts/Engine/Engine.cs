using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;

public class Engine : MonoBehaviour {

    public static bool IsLucas = true;
    public static SaveData PlayerSaveData;

    public static BattleArgs BattleArgs { get; set; }

    void Awake() {
       // PlayerSaveData = IO.Load();
        DontDestroyOnLoad(this);
        //Application.quitting += PlayerSaveData.Save;
        
        TextAsset line = Resources.Load("Data\\PokedexData") as TextAsset;
        PkmnSeen.Clear();
        foreach (string Pkmn in line.text.Split('\n').Skip(1)) { 
            PkmnSeen.Add(Pkmn.Split('_')[1], CaptureStatus.Seen);
        }
        UnityEngine.Debug.Log($"Dusk = {IsDuskOrDawn}");
        UnityEngine.Debug.Log($"Night = {IsNight}");
        UnityEngine.Debug.Log($"Day = {IsDay}");
    }

    public static int DexSeen { get { 
            int f = 0; 
            foreach (CaptureStatus cs in PkmnSeen.Values) {
                if (cs == CaptureStatus.NotSeen) { f++; } 
            } 
            return f; 
        } 
    }

    private void Update() {
    //    DiscordController.Update();
    }

    public static bool IsDuskOrDawn { get {
            TimeSpan time = DateTime.Now.TimeOfDay;
            return time > new TimeSpan(5, 0, 0) && time < new TimeSpan(7, 30, 0) ||  //Between 5:00:00 and 7:30:00 AM
                   time > new TimeSpan(19, 0, 0) && time < new TimeSpan(20, 30, 0); // Between 7:00:00 and 8:30:00 PM 
        }
    }
    public static bool IsNight { get {
            TimeSpan time = DateTime.Now.TimeOfDay;
            return time > new TimeSpan(20, 30, 1) && time < new TimeSpan(23, 59, 59) || //Between 8:30:01 and 11:59:59 PM
                   time > new TimeSpan(0, 0, 0) && time < new TimeSpan(4, 59, 59); //Between 12:00:00 and 4:59:59 AM
        }
    }
    public static bool IsDay { get {
            TimeSpan time = DateTime.Now.TimeOfDay;
            return time > new TimeSpan(7, 30, 1) && time < new TimeSpan(18, 59, 59); //Between 7:30:01 AM and 6:59:59 PM
        }
    }
    public static bool IsSpring { 
        get {
            return new int[] { 3, 4, 5 }.Contains(DateTime.Now.Month);
        }
    }
    public static bool IsSummer {
        get {
            return new int[] { 6, 7, 8 }.Contains(DateTime.Now.Month);
        }
    }
    public static bool IsFall {
        get {
            return new int[] { 9, 10, 11 }.Contains(DateTime.Now.Month);
        }
    }
    public static bool IsWinter {
        get {
            return new int[] { 12, 1, 2 }.Contains(DateTime.Now.Month);
        }
    }

    public static List<string> WaterDwelling { get; } = new List<string>() { };

    //public static string Version { get; } = "Pride";

    public static Dictionary<string, CaptureStatus> PkmnSeen = new Dictionary<string, CaptureStatus>();

    public static CaptureStatus GetSeen(string Pkmn) {
        return CaptureStatus.Caught;
        //return (CaptureStatus)UnityEngine.Random.Range(1,3);
        //return PkmnSeen[Pkmn];
    }

    public static Shine CalculateShine(Trainer Trainer, uint PV) {
        ulong XOR = (ulong) Trainer.GetBitXOR() ^ (PV / 65535 ^ PV % 65535);
        /*if (8 > XOR)
            return Shine.Brilliant;
        else */if (16 > XOR)
            return Shine.Shiny;
        else
            return Shine.Normal;
    }

    public static uint GetIdealValue(Trainer Trainer, int Characteristic = 0, Shine Shine = Shine.Normal) {
        uint PV;
    Loop:
        PV = (uint)UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        PV = (uint)(PV - (PV % 25) + Characteristic);
        if (PV % 25 != Characteristic) { goto Loop; }
        if (CalculateShine(Trainer, PV) != Shine) { goto Loop; }
        return PV;
    }

    public static string GetForm(int Dex, uint PV) {
        switch (Dex) {
            //case 105: return IsNight ? "Alola" : "";
            case 201: return CalculateUnown(PV);
            case 585: return IsSpring ? "Spring" : IsSummer ? "Summer" : IsFall ? "Autumn" : "Winter";
            case 586: return IsSpring ? "Spring" : IsSummer ? "Summer" : IsFall ? "Autumn" : "Winter";
            case 664: case 665: case 666: return CalculateScatterbug(PV);
            case 669: return CalculateFlower(PV);
            case 670: return CalculateFlower(PV);
            case 671: return CalculateFlower(PV);
            case 745: return IsDay ? "Midday" : IsNight ? "Midnight" : "Dusk";
            case 774: return CalculateCore(PV);
            default: return string.Empty;
        }
    }

    public static string CalculateScatterbug(uint PV) {
        string[] Forms = new string[] { "archipelago", "jungle", "monsoon", "sandstorm", "savanna", "continental", "ocean", "river", "sun", "garden", "marine", "highplains", "modern", "elegant", "meadow", "icysnow", "polar", "tundra" };
        //return (int)PV % Forms.Length;
        return Forms[PV % Forms.Length];
    }
    public static string CalculateUnown(uint PV) {
        string[] Forms = new string[] {
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P",
            "Q","R","S","T","U","V","W","X","Y","Z","!","?"
        };
        //return (int)PV % Forms.Length;
        return Forms[PV % Forms.Length];
    }
    public static string CalculateFlower(uint PV) {
        string[] Forms = new string[] { "Red", "Yellow", "Orange", "Blue", "White" };
        //return (int)PV % Forms.Length;
        return Forms[PV % Forms.Length];
    }
    public static string CalculateCore(uint PV) {
        string[] Forms = new string[] { "Red", "Yellow", "Orange", "Green", "Blue", "Indigo", "Violet" };
        //return (int)PV % Forms.Length;
        return Forms[PV % Forms.Length];
    }

    public static string[] PRIDE_EXCLUSIVES { get; } = { "" };
    public static string[] FALL_EXCLUSIVES { get; } = { "" };
    public static int[] POKEMON_WITH_FORMS { get; } = {
        /*105,*/ 201, 585, 586, 664, 665, 666, 669, 670, 671, 745, 774, 
    };

    public static Dictionary<int, Moves[]> GetMoves(string Name) {
        return null;
    }

}

[Serializable]
public class Box { }