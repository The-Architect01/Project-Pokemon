using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour {
    public BattleArgs Args;
    //public Pokemon Enemy;
    //public Pokemon Player;

    private void Awake() {
    //    Args = Engine.BattleArgs;
    //    Enemy = new Pokemon() { Dex = Args.DexNumber };

    }
    private void Start() { 
       // DiscordController.UpdateActivity("Battling",$"vs. Lv. 1 Brilliant Rowlet");
    }

}
public class BattleArgs {

    public bool IsTrainer { get; set; } = false;
    public int DexNumber { get; set; } = 1;
    public int Form { get; set; } = 0;
    public int TrainerID { get; set; } = 0;

}
