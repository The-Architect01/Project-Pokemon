using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HiddenPatch : MonoBehaviour {

    public GameObject PC { get; set; }
    public string[] VR { get; set; }

    public int BaseEncounterRate { get; set; }

    public Image[] Transitions { get; set; }

    bool InGrass;
    public static bool HasLoaded = false;

    AudioSource Intro;
    public bool HasAttempted = false;
    public SpawnMap Host;

    private void Start() {
        if (Host != null) {
            PC = Host.Player;
            VR = Host.PotentialSpawnsVeryRare;
            Transitions = Host.Transitions;
            BaseEncounterRate = Host.BaseEncounterRate;
            Intro = Host.Intro;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == PC && !HasAttempted) {
            HasAttempted = true;
            Attempt();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject == PC) {
            InGrass = false;
            HasAttempted = false;
        }
    }

    void Attempt() {
        HasLoaded = true;
        Player.CanMove = false;
        if (CheckForEncounters()) {
            Host.GenerateEncounter();
            Debug.Log(VR[Random.Range(0, VR.Length-1)]);
            StartCoroutine(Transition());
        } else {
            Player.CanMove = true;
        }
    }

    bool CheckForEncounters() {
        int x = Random.Range(0, 2879);
        Debug.Log(x + " : " + (16 * BaseEncounterRate > x));
        return (16 * BaseEncounterRate) > x;
    }

    int transitionSpeed = 2;
    IEnumerator Transition() {
        Intro.Play();
        while (Transitions[0].canvasRenderer.GetMaterial().GetFloat("Progress") > 0f) {
            Transitions[0].canvasRenderer.GetMaterial().SetFloat("Progress", Mathf.MoveTowards(Transitions[0].canvasRenderer.GetMaterial().GetFloat("Progress"), -0.1f, transitionSpeed * Time.deltaTime));
            yield return new WaitForSeconds(Intro.clip.length/120);
        }
        
        Transitions[0].canvasRenderer.SetMaterial(Resources.Load<Material>("Transitions/Transition - Intro"),0);
        Transitions[0].material = Transitions[0].canvasRenderer.GetMaterial();
        Transitions[0].canvasRenderer.GetMaterial().SetFloat("Progress", 0f);
        SceneManager.LoadScene("Battle", LoadSceneMode.Additive);
        yield return new WaitForSeconds(.5f);

        while (Transitions[0].canvasRenderer.GetMaterial().GetFloat("Progress") < 1.01f) {
            Transitions[0].canvasRenderer.GetMaterial().SetFloat("Progress", Mathf.MoveTowards(Transitions[0].canvasRenderer.GetMaterial().GetFloat("Progress"), 1.01f, transitionSpeed * Time.deltaTime));
            yield return new WaitForSeconds(Intro.clip.length / 240);
        }
        
        Transitions[0].canvasRenderer.SetMaterial(Resources.Load<Material>("Transitions/Wild Encounter"), 0);
        Transitions[0].material = Transitions[0].canvasRenderer.GetMaterial();
        Transitions[0].canvasRenderer.GetMaterial().SetFloat("Progress", 1.01f);
    }
}
