using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TrainerNPC : MonoBehaviour {

    public Player PC;
    public bool Enabled = true;
    public Image Transitions;
    public AudioSource Intro;

    public int TrainerID;

    public string Name;
    public string[] Pokemon;
    public int Difficulty;
    public Sprite[] LEFT;
    public Sprite[] RIGHT;
    public Sprite[] DOWN;
    public Sprite[] UP;

    public DialogueTrigger Dialogue;

    private void Start() {
        string[] Data = Resources.Load<TextAsset>("Trainers").text.Split('\n')[TrainerID].Split('_');
        Name = Data[1];
        Pokemon = new string[] { Data[4] };
        Difficulty = int.Parse(Data[0]);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject == PC.gameObject && Enabled) {
            Debug.Log("Battle Start!");
            Enabled = false;
            Player.CanMove = false;
            StartCoroutine(MoveNPC());
            Dialogue.TriggerDialogue();
            StartCoroutine(Transition());
        }
    }

    IEnumerator MoveNPC() {
        Debug.Log("NPC");
        yield return null;
    }


    int transitionSpeed = 2;
    IEnumerator Transition() {
        Intro.Play();
        while (Transitions.canvasRenderer.GetMaterial().GetFloat("Progress") > 0f) {
            Transitions.canvasRenderer.GetMaterial().SetFloat("Progress", Mathf.MoveTowards(Transitions.canvasRenderer.GetMaterial().GetFloat("Progress"), -0.1f, transitionSpeed * Time.deltaTime));
            yield return new WaitForSeconds(Intro.clip.length / 120);
        }

        Transitions.canvasRenderer.SetMaterial(Resources.Load<Material>("Transitions/Transition - Trainer"), 0);
        Transitions.material = Transitions.canvasRenderer.GetMaterial();
        //Transitions.canvasRenderer.GetMaterial().SetFloat("Progress", 0f);
        SceneManager.LoadScene("Battle", LoadSceneMode.Additive);
        yield return new WaitForSeconds(.5f);

        while (Transitions.canvasRenderer.GetMaterial().GetFloat("Progress") < 1.01f) {
            Transitions.canvasRenderer.GetMaterial().SetFloat("Progress", Mathf.MoveTowards(Transitions.canvasRenderer.GetMaterial().GetFloat("Progress"), 1.01f, transitionSpeed * Time.deltaTime));
            yield return new WaitForSeconds(Intro.clip.length / 240);
        }

        Transitions.canvasRenderer.SetMaterial(Resources.Load<Material>("Transitions/Wild Encounter"), 0);
        Transitions.material = Transitions.canvasRenderer.GetMaterial();
        Transitions.canvasRenderer.GetMaterial().SetFloat("Progress", 1.01f);
    }
}