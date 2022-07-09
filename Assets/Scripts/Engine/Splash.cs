using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Splash : MonoBehaviour {

    public Animation Transition;
    public AudioSource Cry;

    public static string FALL { get; } = "Pokemon Cries/Giratina";
    public static string PRIDE { get; } = "Pokemon Cries/Arceus";

    public static string FALLSPLASH { get; } = "Splash/Splash - Fall";
    public static string PRIDESPLASH { get; } = "Splash/Splash - Pride";

    private void Awake() {
    //    GetComponent<Image>().sprite = Resources.Load<Sprite>(Engine.Version == "Pride" ? PRIDESPLASH : FALLSPLASH);
    }
    // Update is called once per frame
    void Update() {
        if (Input.anyKey) {
            Cry.playOnAwake = false;
            Cry.loop = false;
        //    Cry.clip = Resources.Load<AudioClip>(Engine.Version == "Pride" ? PRIDE : FALL);
            StartCoroutine(nameof(TransitionPlayer));
        }
    }
    IEnumerator TransitionPlayer() {
        Cry.Play();
        Transition.Play();
        yield return new WaitForSeconds(Cry.clip.length);
        SceneManager.LoadScene(Engine.PlayerSaveData.LastSaveLocation);
    }
}
