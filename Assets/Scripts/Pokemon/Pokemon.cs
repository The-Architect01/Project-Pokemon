using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class Pokemon : MonoBehaviour {
    #region Public
    public Shine Shine { get; private set; }
    public int Dex;
    public int Form = 0;

    #region ID
    public Pokeballs Pokeball { get; set; }
    public string GivenName { get; set; }
    public string Name { get { return GivenName == null ? SpeciesName : GivenName; } }
    public Trainer OriginalTrainer { get; private set; }
    public Trainer CurrentTrainer { get; private set; }
    #endregion
    #region Stats
    public int CurrentHP { get; set; }
    public bool Fainted { get { return CurrentHP <= 0; } }

    public Stat HP { get; private set; }
    public Stat Attack { get; private set; } 
    public Stat Defense { get; private set; }
    public Stat SpecialAttack { get; private set; } 
    public Stat SpecialDefense { get; private set; } 
    public Stat Speed { get; private set; }

    private Stat[] Stats;

    public int Friendship { get; set; }
    public int CatchRate { get; private set; }
    public decimal FleeRate { get; private set; }
    #endregion
    public uint Personality { get; private set; }
    public string Characteristic { get; }
    public Nature Nature { get; private set; }

    public Gender Gender {
        get {
            if(GenderThreshold == 0) {
                return Gender.Male;
            }else if(GenderThreshold == 254) {
                return Gender.Female;
            }else if(GenderThreshold == 255) {
                return Gender.Nonbinary;
            } else {
                if((Personality % 256) >= GenderThreshold) {
                    return Gender.Male;
                } else {
                    return Gender.Female;
                }
            }
        }
    }

    public StatusCondition Stable;
    public VolitleStatusCondition Volitile;
    public XP XP;
    public Move Move1 { get; set; }
    public Move Move2 { get; set; }
    public Move Move3 { get; set; }
    public Move Move4 { get; set; }
    #endregion

    private string SpeciesName;
    public int Height { get; private set; }
    public decimal Weight { get; private set; }
    public PokemonType Type { get; private set; }
    private int FriendshipBase;
    private int GenderThreshold;

    public void Awake() {

        OriginalTrainer = new Trainer();
        CurrentTrainer = OriginalTrainer;

        TextAsset PokemonDNA = Resources.Load<TextAsset>("PokemonDNA");
        TextAsset Pokedex = Resources.Load<TextAsset>("Pokedex/PokedexData");
        string[] DNA = PokemonDNA.text.Split('\n')[Dex].Split('_');
        
        string[] DexData = Pokedex.text.Split('\n')[Dex].Split('_');
        SpeciesName = DNA[0].Split(',')[0];
        Height = int.Parse(DexData[2]);
        Weight = decimal.Parse(DexData[3]);
        Type = DexData[5] == "" ? new PokemonType(DexData[4]) : new PokemonType(DexData[4],DexData[5]);

        GenderThreshold = int.Parse(DNA[1]);
        CatchRate = int.Parse(DNA[2]);
        FriendshipBase = int.Parse(DNA[5]);
        Friendship = FriendshipBase;
        HP = new Stat {
            IsHP = true,
            Base = int.Parse(DNA[6]),
            EV_Reward = int.Parse(DNA[7]),
            IV = Random.Range(0, 31),
        };
        Attack = new Stat {
            Base = int.Parse(DNA[8]),
            EV_Reward = int.Parse(DNA[9]),
            IV = Random.Range(0, 31),
        };
        Defense = new Stat {
            Base = int.Parse(DNA[10]),
            EV_Reward = int.Parse(DNA[11]),
            IV = Random.Range(0, 31),
        };
        SpecialAttack = new Stat {
            Base = int.Parse(DNA[12]),
            EV_Reward = int.Parse(DNA[13]),
            IV = Random.Range(0, 31),
        };
        SpecialDefense = new Stat {
            Base = int.Parse(DNA[14]),
            EV_Reward = int.Parse(DNA[15]),
            IV = Random.Range(0, 31),
        };
        Speed = new Stat {
            Base = int.Parse(DNA[16]),
            EV_Reward = int.Parse(DNA[17]),
            IV = Random.Range(0, 31),
        };
        FleeRate = decimal.Parse(DNA[18]);
        Personality = (uint)Random.Range(uint.MinValue, uint.MaxValue);

        Nature = new Nature(Personality);
        XP = new XP(int.Parse(DNA[4]), DNA[3]);
        
        Shine = Engine.CalculateShine(OriginalTrainer, Personality);

        Stats = new Stat[] { HP, Attack, Defense, SpecialAttack, SpecialDefense, Speed };

        for (int i = 0; i < 6; i++) { Stats[i].Update(XP.Level, Nature.nature[i]); }
        GenerateMoveSet(SpeciesName);

        Debug.Log(this);
        
    }

    public override bool Equals(object other) {
        if (other is Pokemon otherpkmn)
            return (otherpkmn.Personality == Personality && otherpkmn.Name == Name && otherpkmn.OriginalTrainer == OriginalTrainer);
        return base.Equals(other);
    }
    public override int GetHashCode() {return base.GetHashCode();}
    public override string ToString() {
        return $"{SpeciesName},{Gender},{HP.Value},{Attack.Value},{Defense.Value},{SpecialAttack.Value},{SpecialDefense.Value},{Speed.Value},{Personality},{Shine},{Nature}";
    }

    public void GenerateMoveSet(string Name) {
        return;
        Dictionary<int, Moves[]> MovesKnown = Engine.GetMoves(Name);
        List<Moves> MovePool = new List<Moves>();
        for(int i = 1; i<= XP.Level;i++) {
            foreach(Moves move in MovesKnown[i]) {
                MovePool.Add(move);
            }
        }
        Move[] Moves = { Move1, Move2, Move3, Move4 };
        for(int i = 0; i<Moves.Length;i++) {
            int RandomNum = Random.Range(0,MovePool.Count - 1);
            Moves[i] = new Move(MovePool[RandomNum]);
            MovePool.RemoveAt(RandomNum);
        }

    }

    /* The Following will be stored in a file
     * Species Name
     * Height
     * Weight
     * Type 1
     * Type 2
     * Friendship Base
     * Gender Threshold
     * Catch Rate
     * Flee Rate
     */
}
[System.Serializable]
public enum Shine {
    Shiny = 1,
    Normal = 0,
    Brilliant = -1,
}

[System.Serializable]
public struct PokemonType {

    public enum Types {
        None,
        Normal,
        Fighting,
        Flying,
        Poison,
        Ground,
        Rock,
        Bug,
        Ghost,
        Steel,
        Fire,
        Water,
        Grass,
        Electric,
        Psychic,
        Ice,
        Dragon,
        Dark,
        Fairy
    }

    public Types Type1 { get; }
    public Types Type2 { get; }
    
    public PokemonType(string Type1, string Type2 = "None") {
        this.Type1 = (Types) System.Enum.Parse(typeof(Types), Type1);
        this.Type2 = (Types)System.Enum.Parse(typeof(Types), Type2);
    }

    public Dictionary<Types, double> GetTypeEffectivness(string Type) {
        return (Dictionary<Types, double>)GetType().GetField(Type.ToUpper(),
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static).GetValue(this);
    }

    public override bool Equals(object obj) {
        if (obj is PokemonType secondObject)
            return (secondObject.Type1 == Type1 && secondObject.Type2 == Type2);
        return base.Equals(obj);
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }

    public override string ToString() {
        return Type1 + "_" + Type2;
    }

    #region TypeEffectiveness
    static readonly Dictionary<Types, double> NORMAL = new Dictionary<Types, double>() {
        { Types.Ghost, 0f },
        { Types.Steel, .5f },
        { Types.Rock, .5f },
    };

    static readonly Dictionary<Types, double> DARK = new Dictionary<Types, double>() {
        { Types.Dark, .5f },
        { Types.Fairy, .5f },
        { Types.Ghost, 2f },
        { Types.Psychic, 2f },
        { Types.Fighting, .5f }
    };

    static readonly Dictionary<Types, double> FAIRY = new Dictionary<Types, double>() {
        { Types.Dark, 2f },
        { Types.Fairy, .5f },
        { Types.Dragon, 2f },
        { Types.Steel, .5f },
        { Types.Fighting, 2f },
        { Types.Poison, .5f }
    };

    static readonly Dictionary<Types, double> DRAGON = new Dictionary<Types, double>() {
        { Types.Fairy, 0f },
        { Types.Dragon, 2f },
        { Types.Steel, .5f },
    };

    static readonly Dictionary<Types, double> FIRE = new Dictionary<Types, double>() {
        { Types.Dragon, .5f },
        { Types.Fire, .5f },
        { Types.Grass, 2f },
        { Types.Ice, 2f },
        { Types.Water, .5f },
        { Types.Steel, 2f },
        { Types.Bug, 2f },
        { Types.Rock, .5f }
    };

    static readonly Dictionary<Types, double> GRASS = new Dictionary<Types, double>() {
        { Types.Dragon, .5f },
        { Types.Fire, .5f },
        { Types.Grass, .5f },
        { Types.Water, 2f },
        { Types.Steel, .5f },
        { Types.Bug, .5f },
        { Types.Rock, 2f },
        { Types.Flying, .5f },
        { Types.Poison, .5f },
        { Types.Ground, 2f }
    };

    static readonly Dictionary<Types, double> ICE = new Dictionary<Types, double>() {
        { Types.Dragon, 2f },
        { Types.Fire, .5f },
        { Types.Grass, 2f },
        { Types.Ice, .5f },
        { Types.Water, .5f },
        { Types.Steel, .5f },
        { Types.Flying, 2f },
        { Types.Ground, 2f }
    };

    static readonly Dictionary<Types, double> WATER = new Dictionary<Types, double>() {
        { Types.Dragon, .5f },
        { Types.Fire, 2f },
        { Types.Grass, .5f },
        { Types.Water, .5f },
        { Types.Rock, 2f }
    };

    static readonly Dictionary<Types, double> ELECTRIC = new Dictionary<Types, double>() {
        { Types.Dragon, .5f },
        { Types.Grass, .5f },
        { Types.Water, 2f },
        { Types.Electric, .5f },
        { Types.Flying, 2f },
        { Types.Ground, 0f }
    };

    static readonly Dictionary<Types, double> GHOST = new Dictionary<Types, double>() {
        { Types.Normal, 0f },
        { Types.Dark, .5f },
        { Types.Ghost, 2f },
        { Types.Psychic, 2f }
    };

    static readonly Dictionary<Types, double> PSYHCIC = new Dictionary<Types, double>() {
        { Types.Dark, 0f },
        { Types.Psychic, .5f },
        { Types.Steel, .5f },
        { Types.Fighting, 2f },
        { Types.Poison, 2f }
    };

    static readonly Dictionary<Types, double> STEEL = new Dictionary<Types, double>() {
        { Types.Fairy, 2f },
        { Types.Fire, .5f },
        { Types.Ice, 2f },
        { Types.Water, .5f },
        { Types.Electric, .5f },
        { Types.Steel, .5f },
        { Types.Rock, 2f }
    };

    static readonly Dictionary<Types, double> BUG = new Dictionary<Types, double>() {
        { Types.Dark, 2f },
        { Types.Fairy, .5f },
        { Types.Fire, .5f },
        { Types.Grass, 2f },
        { Types.Ghost, .5f },
        { Types.Psychic, 2f },
        { Types.Steel, .5f },
        { Types.Fighting, .5f },
        { Types.Flying, .5f },
        { Types.Poison, .5f }
    };

    static readonly Dictionary<Types, double> ROCK = new Dictionary<Types, double>() {
        { Types.Fire, 2f },
        { Types.Ice, 2f },
        { Types.Steel, .5f },
        { Types.Bug, 2f },
        { Types.Fighting, .5f },
        { Types.Flying, 2f },
        { Types.Ground, .5f }
    };

    static readonly Dictionary<Types, double> FIGHTING = new Dictionary<Types, double>() {
        { Types.Normal, 2f },
        { Types.Dark, 2f },
        { Types.Fairy, .5f },
        { Types.Ice, 2f },
        { Types.Ghost, 0f },
        { Types.Psychic, .5f },
        { Types.Steel, 2f },
        { Types.Bug, .5f },
        { Types.Rock, 2f },
        { Types.Flying, .5f },
        { Types.Poison, .5f }
    };

    static readonly Dictionary<Types, double> FLYING = new Dictionary<Types, double>() {
        { Types.Grass, 2f },
        { Types.Electric, .5f },
        { Types.Steel, .5f },
        { Types.Bug, 2f },
        { Types.Rock, .5f },
        { Types.Fighting, 2f }
    };

    static readonly Dictionary<Types, double> POISON = new Dictionary<Types, double>() {
        { Types.Fairy, 2f },
        { Types.Grass, 2f },
        { Types.Ghost, .5f },
        { Types.Steel, 0f },
        { Types.Rock, .5f },
        { Types.Poison, .5f },
        { Types.Ground, .5f }
    };

    static readonly Dictionary<Types, double> GROUND = new Dictionary<Types, double>() {
        { Types.Fire, 2f },
        { Types.Grass, .5f },
        { Types.Electric, 2f },
        { Types.Steel, 2f },
        { Types.Bug, .5f },
        { Types.Rock, 2f },
        { Types.Flying, 0f },
        { Types.Poison, 2f }
    };
    #endregion

}