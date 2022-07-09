using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SpawnMap : MonoBehaviour {

    public GameObject Player;
    public AudioSource Intro;
    public int BaseEncounterRate = 20;
    public int MinLevel;
    public int MaxLevel;
    public string Terrain;
    public Image[] Transitions;
    public double[] ModifiedRarity = new double[6] {
        0,0,0,0,0,0
    };

    public string[] PotentialSpawnsVeryRare;
    public string[] PotentialSpawnsRare;
    public string[] PotentialSpawnsAverage;
    public string[] PotentialSpawnsCommon;
    public string[] PotentialSpawnsVeryCommon;

    public void GenerateEncounter() {

        Engine.BattleArgs = new BattleArgs();
    }
    

    /*public TallGrass[] PatchesOfGrass;

    private void Start() {
        foreach(TallGrass TG in PatchesOfGrass) {
            TG.PC = Player;
            TG.VR = PotentialSpawnsVeryRare;
            TG.R = PotentialSpawnsRare;
            TG.A = PotentialSpawnsAverage;
            TG.C = PotentialSpawnsCommon;
            TG.VC = PotentialSpawnsVeryCommon;
            TG.Transitions = Transitions;
            TG.Intro = Intro;
        }
    }*/
}
