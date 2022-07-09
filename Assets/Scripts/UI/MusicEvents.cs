using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicEvents : MonoBehaviour {

    public AudioClip Intro;
    public AudioClip Main;
    public AudioSource Output;

    void Start() {
        StartCoroutine(PlaySound());
    }

    IEnumerator PlaySound() {
        Output.clip = Intro;
        Output.Play();
        yield return new WaitForSeconds(Intro.length);
        Output.clip = Main;
        Output.loop = true;
        Output.Play();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
