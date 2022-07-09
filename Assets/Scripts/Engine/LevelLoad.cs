using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoad : MonoBehaviour {

    public Animator Transition;
    public float TransitionTime;

    public void LoadBattle() {
        StartCoroutine(LoadNextScene("Battle"));
    }

    IEnumerator LoadNextScene(string Scene) {
        Transition.SetTrigger("Start");
        yield return new WaitForSeconds(TransitionTime);
        SceneManager.LoadScene(Scene);
    }

}
