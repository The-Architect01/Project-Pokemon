using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum Gender {
    Nonbinary,
    Female,
    Male
}
[Serializable]
public class Trainer {
   
    protected uint SecretID;
    protected uint PublicID;
    public string Name { get; protected set; }
    public Gender Gender { get; protected set; }

    public new bool Equals(object obj) {
        if (obj is Trainer) {
            Trainer comp = obj as Trainer;
            if (comp.SecretID == SecretID && comp.PublicID == PublicID && comp.Name == Name && comp.Gender == Gender)
                return true;
        } else {
            return base.Equals(obj);
        }
        return false;
    }
    public int GetBitXOR() {
        return (int)(PublicID ^ SecretID);
    }
}
[Serializable]
public class PCTrainer : Trainer {

    public ulong TimePlayed { get; private set; } = 0;
    public uint Money { get; set; } = 0;
    public GymBadge GymBadge { get; set; } = GymBadge.None;
    public Dictionary<Items, uint> Items { get; set; }//Item, Amount
    public Dictionary<Pokeballs, uint> PokeBalls { get; set; }//Item, Amount
    public Dictionary<SpecialItems, bool> KeyItems { get; set; }//Item, Have

    public PCTrainer() : base() {
        SecretID = (uint)UnityEngine.Random.Range(uint.MinValue, uint.MaxValue);
        PublicID = (uint)UnityEngine.Random.Range(uint.MinValue, uint.MaxValue);
        TimePlayed = 0;
    }

}
[Serializable]
public enum GymBadge {
    None,

}