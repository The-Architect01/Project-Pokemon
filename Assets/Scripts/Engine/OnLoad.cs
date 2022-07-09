using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnLoad : MonoBehaviour {
    public string ScreenName;
    public PlayerState PlayerState = PlayerState.Exploring;

    private void Awake() {
    //   DiscordController.UpdateActivity(PlayerState.ToString(), ScreenName);
        Engine.PlayerSaveData.LastSaveLocation = ScreenName;
    }

    public int TransitionSpeed = 2;

}

public enum PlayerState {
    None,
    Exploring,
    Battling,
    Loading,
}