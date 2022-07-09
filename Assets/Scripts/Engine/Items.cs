using UnityEngine;
[System.Serializable]
public enum Items {
ArmorFossil,
BeachGlass,
BigMushroom,
BigPearl,
BottleCap,
ChalkyStone,
ClawFossil,
CometShard,
CoverFossil,
DawnStone,
DomeFossil,
DragonScale,
DuskStone,
EscapeRope,
FestivalTicket,
FireStone,
FossilizedBird,
FossilizedDino,
FossilizedDrake,
FossilizedFish,
GoldBottleCap,
HeartScale,
HelixFossil,
HometownMuffin,
IceStone,
JubilifeMuffin,
LeafLetter,
LeafStone,
LinkingCord,
MedicinalLeek,
MoonStone,
Nugget,
OddKeystone,
OldAmber,
Pearl,
PearlString,
Permit,
PlumeFossil,
PokéDoll,
PrismScale,
ReaperCloth,
RootFossil,
ShinyCharm,
ShinyStone,
SkullFossil,
StarPiece,
SunStone,
ThunderStone,
WaterStone,
}
[System.Serializable]
public enum BattleItems {
    DireHit,
GuardSpec,
XAccuracy,
XAttack,
XDefense,
XSpAtk,
XSpDef,
XSpeed,
}
[System.Serializable]
public enum SpecialItems {
BigRoot,
BlankPlate,
DampRock,
DeepSeaScale,
DeepSeaTooth,
DracoPlate,
DreadPlate,
DroppedItem,
EarthPlate,
ElevatorKey,
FistPlate,
FlamePlate,
GriseousOrb,
GrubbyHanky,
IciclePlate,
InsectPlate,
IntriguingStone,
IronPlate,
LegendPlate,
LookerTicket,
MeadowPlate,
MindPlate,
PixiePlate,
PlasmaCard,
PowerPlantPass,
PrisonBottle,
ProfsLetter,
RazorClaw,
SkyPlate,
SplashPlate,
SpookyPlate,
StonePlate,
ToxicPlate,
ZapPlate,
}
[System.Serializable]
public enum Medicine {
Antidote,
Awakening,
BerryJuice,
BigMalasada,
BurnHeal,
Calcium,
Carbos,
Casteliacone,
CleverWing,
Elixir,
EnergyPowder,
EnergyRoot,
Ether,
ExpCandyL,
ExpCandyM,
ExpCandyS,
ExpCandyXL,
ExpCandyXS,
FineRemedy,
FreshWater,
FullHeal,
FullRestore,
GeniusWing,
HealPowder,
HealthWing,
HPUp,
HyperPotion,
IceHeal,
Iron,
LavaCookie,
LumioseGalette,
MaxElixir,
MaxEther,
MaxPotion,
MaxRevive,
MoomooMilk,
MuscleWing,
OldGateau,
Potion,
Protein,
RageCandyBar,
RareCandy,
Remedy,
ResistWing,
RevivalHerb,
Revive,
SacredAsh,
ShalourSable,
SuperPotion,
SwiftWing,
Zinc,

}
[System.Serializable]
public enum Pokeballs {
Cherish,
Dive,
Dream,
Dusk,
Fast,
Great,
Heavy,
Level,
Love,
Master,
Net,
Poké,
Premier,
Quick,
Repeat,
Timer,
Ultra,
}

public static class PokeBall {


    public static PokeBallArgs Capture(CaptureArgs Args) {
        float Modifier = Args.Pokeball switch {
            Pokeballs.Cherish => 1,
            Pokeballs.Dive => Engine.WaterDwelling.Contains(Args.TargetPokemon.Name) ? 3.5f : 1,
            Pokeballs.Dream => Args.TargetPokemon.Stable == StatusCondition.Sleep ? 4f:1,
            Pokeballs.Dusk => Engine.IsNight || Args.Terrain == BattleTerrain.Cave ? 3:1,
            Pokeballs.Fast => Args.TargetPokemon.Speed.Base >=100 ? 4:1,
            Pokeballs.Great => 1.5f,
            Pokeballs.Heavy => Args.TargetPokemon.Weight <= 220.2m ? -20f :
                Args.TargetPokemon.Weight >= 220.5m && Args.TargetPokemon.Weight <= 440.7m ? 0 :
                Args.TargetPokemon.Weight >=440.9m && Args.TargetPokemon.Weight <=661.2m ? 20f : 30f,
            Pokeballs.Love => Args.AllyPokemon.Gender != Args.TargetPokemon.Gender &&
                (Args.AllyPokemon.Gender != Gender.Nonbinary || Args.TargetPokemon.Gender != Gender.Nonbinary) ? 8:1,
            Pokeballs.Master => 255,
            Pokeballs.Net => Args.TargetPokemon.Type.Type1 == PokemonType.Types.Bug ||
                Args.TargetPokemon.Type.Type2 == PokemonType.Types.Bug || 
                Args.TargetPokemon.Type.Type1 == PokemonType.Types.Water || 
                Args.TargetPokemon.Type.Type2 == PokemonType.Types.Water ? 3.5f : 1,
            Pokeballs.Poké => 1,
            Pokeballs.Premier => 1,
            Pokeballs.Quick => Args.Turn == 1 ? 5:1,
            Pokeballs.Repeat => Engine.PkmnSeen[Args.TargetPokemon.Name] == CaptureStatus.Caught ? 3.5f: 1,
            Pokeballs.Timer => Args.Turn >= 10 ? 4 : (1 + Args.Turn * (1229/4096f)),
            _ => 1,
        };

        
        float a = 3 * Args.TargetPokemon.HP.Value;
        float b = -2 * Args.TargetPokemon.CurrentHP;
        float c = a + b;
        float d = c * Args.TargetPokemon.CatchRate;
        float e = d * Modifier;
        float f = e / a;
        float i = (Args.TargetPokemon.Stable == StatusCondition.Freeze || Args.TargetPokemon.Stable == StatusCondition.Sleep) ? 2f : (Args.TargetPokemon.Stable != StatusCondition.None) ? 1.5f : 1f;
        float j = f * i;

        int shake = 0;
        if (j >= 255) {
            shake = 4;
        } else {
            for (int m = 1; m <= 4; m++) {
                int B = (int)(104856 / Mathf.Sqrt(Mathf.Sqrt(16711680 / j)));
                int C = Random.Range(0, ushort.MaxValue);
                if(C > B) { break; } else { shake = m; }
            }
        }
        if (shake == 4) {
            Args.TargetPokemon.Pokeball = Args.Pokeball;
            return new PokeBallArgs(shake, true);
        }
        return new PokeBallArgs(shake, false);
    }

    public struct PokeBallArgs {
        public int Shakes { get; set; }
        public bool Captured { get; set; }
        public PokeBallArgs(int Shakes, bool Captured) { this.Shakes = Shakes; this.Captured = Captured; }
    }
    public struct CaptureArgs {
        public Pokeballs Pokeball { get; }
        public Pokemon TargetPokemon { get; }
        public Pokemon AllyPokemon { get; }
        public BattleTerrain Terrain { get; }
        public int Turn { get; }
        public CaptureArgs(Pokeballs Pokeball, Pokemon Target, Pokemon Ally, BattleTerrain battle, int CurrentTurn) { this.Pokeball = Pokeball; TargetPokemon = Target; AllyPokemon = Ally; Terrain = battle; Turn = CurrentTurn; }
    }
}
[System.Serializable]
public enum StatusCondition {
    None,
    Burn,
    Freeze,
    Paralysis,
    Poison,
    Sleep,
}
[System.Serializable]
public enum VolitleStatusCondition {

}
public enum BattleTerrain {
    None,
    Cave,
}