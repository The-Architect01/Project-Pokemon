using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class SaveData {

    public PCTrainer PC { get; set; }
    public string LastSaveLocation { get; set; } = "Route 1";
    public Dictionary<string, CaptureStatus> PkmnSeen;
    //public Egg Egg;
    public Box[] PokemonBoxes = new Box[14];

    public void Save() { IO.Save(this); }

}
public static class IO {
    private static readonly string Path = Application.persistentDataPath + "/File0";
    private static readonly BinaryFormatter Serializer = new BinaryFormatter();

    public static SaveData Load() {
        if (File.Exists(Path)) {
            using FileStream file = new FileStream(Path, FileMode.Open);
            Debug.Log("Save Data Loaded");
            return Serializer.Deserialize(file) as SaveData;
        } else {
            Debug.Log("File Not Found");
            return new SaveData();
        }
    }

    public static void Save(SaveData save) {
        using FileStream file = new FileStream(Path, FileMode.Create);
        Serializer.Serialize(file, save);
        UnityEngine.Debug.Log("Saved Data");
    }

}