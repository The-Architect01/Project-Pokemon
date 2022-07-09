using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BattleUIController : MonoBehaviour
{

    public Button Fight;
    public Button Pokemon;
    public Button Items;
    public Button Run;

    public Button Move1;
    public Button Move2;
    public Button Move3;
    public Button Move4;

    public GameObject ActionMenu;
    public GameObject MoveMenu;
    public GameObject MoveData;
    public GameObject PokeballData;

    //public UniGifImage AI;
    //public UniGifImage PC;

    private void Awake() {
//        AI.SetGifFromUrl(@$"Front\Shiny\Absol.gif");
  //      PC.SetGifFromUrl($@"Back\Shiny\Absol.gif");
    }

    private void Start() {
        Fight.onClick.AddListener(delegate {
            ActionMenu.SetActive(false);
            MoveMenu.SetActive(true);
        });
        Pokemon.onClick.AddListener(delegate {
            Debug.Log("Pokemon Clicked");
        });
        Items.onClick.AddListener(delegate {
            Debug.Log("Items Clicked");
        });
        Run.onClick.AddListener(delegate {
            TallGrass.HasLoaded = false;
            Player.CanMove = true;
            SceneManager.UnloadSceneAsync("Battle");
        });
        Move1.onClick.AddListener(delegate {
            Debug.Log("Move 1 Clicked");
        });
        Move2.onClick.AddListener(delegate {
            Debug.Log("Move 2 Clicked");
        });
        Move3.onClick.AddListener(delegate {
            Debug.Log("Move 3 Clicked");
        });
        Move4.onClick.AddListener(delegate {
            Debug.Log("Move 4 Clicked");
        });
    }

    private void Update() {
        if (Input.GetKey(KeyCode.I) && MoveMenu.activeInHierarchy) {
            Debug.Log("Info Requested");
        } else if (Input.GetKey(KeyCode.C) && MoveMenu.activeInHierarchy) {
            Debug.Log("Capture Requested");
        } else if (Input.GetKey(KeyCode.Escape)) {
            if (MoveMenu.activeInHierarchy) {
                MoveMenu.SetActive(false);
                ActionMenu.SetActive(true);
            } else if (MoveData.activeInHierarchy) {
                MoveData.SetActive(false);
                MoveMenu.SetActive(true);
            } else if (PokeballData.activeInHierarchy) {
                PokeballData.SetActive(false);
                MoveMenu.SetActive(true);
            }
        }
    }

    
}
